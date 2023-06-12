using Microsoft.Maui.Controls;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Nice_bike
{
    public partial class Order_Details : ContentPage
    {
        public Order_Details()
        {
            InitializeComponent();
            LoadData();
            this.BindingContext = this;
        }

        private List<Order> Orders { get; set; }

        private async void LoadData()
        {
            try
            {
                var connectionString = "server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT Model, Size, Color, Price, customer, NumOrder, idbike FROM commande";
                    var command = new MySqlCommand(query, connection);

                    var dataTable = new DataTable();
                    var dataAdapter = new MySqlDataAdapter(command);
                    dataAdapter.Fill(dataTable);

                    var orders = GroupOrders(dataTable);

                    var doneQuery = "SELECT idbike FROM bikedone";
                    var doneCommand = new MySqlCommand(doneQuery, connection);
                    var doneReader = await doneCommand.ExecuteReaderAsync();

                    while (doneReader.Read())
                    {
                        var doneBikeId = doneReader["idbike"].ToString();
                        var existingBike = orders.SelectMany(o => o.Bikes).FirstOrDefault(b => b.IDBike == doneBikeId);

                        if (existingBike != null)
                        {
                            existingBike.IsBikeDone = true;
                        }
                    }

                    doneReader.Close();

                    orderListView.ItemsSource = orders;
                }
            }
            catch (Exception ex)
            {
                // Gérer les exceptions
                await Application.Current.MainPage.DisplayAlert("Error", $"Error has been detected : {ex.Message}", "OK");
            }

        }

        private List<Order> GroupOrders(DataTable dataTable)
        {
            var orders = new List<Order>();

            foreach (DataRow row in dataTable.Rows)
            {
                var model = row["Model"].ToString();
                var size = row["Size"].ToString();
                var color = row["Color"].ToString();
                var price = Convert.ToDecimal(row["Price"]);
                var cus = row["customer"].ToString();
                var numorder = row["NumOrder"].ToString();
                var id = row["idbike"].ToString();

                var bike = new Bike(model, size, color, price, cus, id);

                var existingOrder = orders.Find(o => o.NumOrder == numorder);
                if (existingOrder != null)
                {
                    existingOrder.Bikes.Add(bike);
                }
                else
                {
                    var newOrder = new Order(numorder, cus);
                    newOrder.Bikes.Add(bike);
                    orders.Add(newOrder);
                }
            }

            return orders;
        }

        private async void OnToggleSwitchToggled(object sender, ToggledEventArgs e)
        {
            var toggledSwitch = (Switch)sender;
            var order = (Order)toggledSwitch.BindingContext;

            if (order != null)
            {
                try
                {
                    var connectionString = "server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;";
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        var deleteQuery = "DELETE FROM bikedone WHERE NumOrder = @numOrder";
                        var deleteCommand = new MySqlCommand(deleteQuery, connection);
                        deleteCommand.Parameters.AddWithValue("@numOrder", order.NumOrder);
                        await deleteCommand.ExecuteNonQueryAsync();

                        var doneQuery = "SELECT idbike FROM bikedone";
                        var doneCommand = new MySqlCommand(doneQuery, connection);
                        var doneReader = await doneCommand.ExecuteReaderAsync();

                        var updatedBikeIds = new HashSet<string>();

                        while (doneReader.Read())
                        {
                            var doneBikeId = doneReader["idbike"].ToString();
                            updatedBikeIds.Add(doneBikeId);

                            var existingBike = order.Bikes.FirstOrDefault(b => b.IDBike == doneBikeId);
                            if (existingBike != null)
                            {
                                existingBike.IsBikeDone = true;
                            }
                        }

                        doneReader.Close();

                        foreach (var bike in order.Bikes)
                        {
                            if (!updatedBikeIds.Contains(bike.IDBike))
                            {
                                bike.IsBikeDone = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Gérer les exceptions
                    await Application.Current.MainPage.DisplayAlert("Error", $"Error has been detected : {ex.Message}", "OK");
                }
            }
        }

        private class Bike : INotifyPropertyChanged
        {
            public string Model { get; set; }
            public string Size { get; set; }
            public string Color { get; set; }
            public decimal Price { get; set; }
            public string Customer { get; set; }
            public string IDBike { get; set; }

            private bool isBikeDone;
            public bool IsBikeDone
            {
                get { return isBikeDone; }
                set
                {
                    if (isBikeDone != value)
                    {
                        isBikeDone = value;
                        OnPropertyChanged("IsBikeDone");
                    }
                }
            }

            public Bike(string model, string size, string color, decimal price, string customer, string idbike)
            {
                Model = model;
                Size = size;
                Color = color;
                Price = price;
                Customer = customer;
                IDBike = idbike;
            }

            // Implement the INotifyPropertyChanged interface
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private class Order : INotifyPropertyChanged
        {
            public string NumOrder { get; }
            public string Customer { get; }
            public List<Bike> Bikes { get; }
            public bool IsOrderSelected { get; set; }


            public Order(string numOrder, string customer)
            {
                NumOrder = numOrder;
                Customer = customer;
                Bikes = new List<Bike>();
            }

            // Implement the INotifyPropertyChanged interface
            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
