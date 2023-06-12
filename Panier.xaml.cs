using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace Nice_bike;

public class Bike
{
    public string Model { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; } = 1;

    public Bike(string model, string size, string color, double price, int quantity)
    {
        Model = model;
        Size = size;
        Color = color;
        Price = price;
        Quantity = quantity;
    }
}

public partial class Panier : ContentPage
{
    public static List<Bike> bikes = new List<Bike>();
    private List<Bike> cartItems = new List<Bike>();

    private ObservableCollection<Bike> cartItemsObservableCollection = new ObservableCollection<Bike>();
    private static int OrderNumber = new Random().Next(1000, 9999);


    public Panier()
    {
        InitializeComponent();


        // Afficher la liste des produits
        bikeList.ItemsSource = bikes;

        // Afficher le panier vide
        UpdateCartTotal();
        UpdateDeliveryDate();

    }

    private void AddToCart_Clicked(object sender, System.EventArgs e)
    {
        var button = sender as Button;
        var bike = button.BindingContext as Bike;

        // Ajouter l'article au panier
        cartItems.Add(bike);

        // Mettre à jour le total du panier
        UpdateCartTotal();
        UpdateTotal();

        // Mettre à jour la liste des articles dans le panier
        cartList.ItemsSource = null;
        cartList.ItemsSource = cartItems;
    }

    private void RemoveItem_Clicked(object sender, System.EventArgs e)
    {
        var button = sender as Button;
        var item = button.BindingContext as Bike;

        if (item.Quantity > 1)
        {
            item.Quantity--;
        }
        else
        {
            cartItems.Remove(item);
        }

        UpdateTotal();
        UpdateDeliveryDate(); // Mettre à jour la date de livraison
    }

    private void AddItem_Clicked(object sender, System.EventArgs e)
    {
        var button = sender as Button;
        var item = button.BindingContext as Bike;

        item.Quantity++;

        UpdateTotal();
        UpdateDeliveryDate(); // Mettre à jour la date de livraison
    }

    private void UpdateTotal()
    {
        var total = cartItems.Sum(item => item.Price * item.Quantity);
        cartTotal.Text = $"Total: €{total:N2}";

        cartItems.RemoveAll(item => item.Quantity == 0); // Supprime les éléments dont la quantité est zéro
        cartList.ItemsSource = null;
        cartList.ItemsSource = cartItems; // Met à jour la liste des articles dans le panier
    }

    private void UpdateCartTotal()
    {
        double total = 0;

        foreach (var item in cartItems)
        {
            total += item.Price;
        }

        cartTotal.Text = "Total: $" + total.ToString("N2");
    }
    private void ClearCart()
    {
        cartItems.Clear();
        cartItemsObservableCollection.Clear();
    }


    private async void Checkout_Clicked(object sender, EventArgs e)
    {
        // Afficher une confirmation pour confirmer la commande
        bool answer = await DisplayAlert("Confirmation", "Do you want to confirm the order ?", "Yes", "No");

        if (answer)
        {

            SendCartToDatabase(cartItems);
            // Vider le panier
            ClearCart();

            // Afficher un message de confirmation
            await DisplayAlert("Succes", "Your order has been succesfully received !", "OK");


            // Naviguer vers la page d'accueil
            await Navigation.PopToRootAsync();
        }
    }
    static void CopyElementsFromCommandeToBikeTodo(string connectionString)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Copy elements from commande to biketodo
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO biketodo (Model, Size, Color, Price, customer, NumOrder, idbike) SELECT Model, Size, Color, Price, customer, NumOrder, idbike FROM commande";
                command.ExecuteNonQuery();
            }
        }
    }
    static void CopyNewElementsFromCommandeToBikeTodo(string connectionString)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Copy new elements from commande to biketodo
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO biketodo (Model, Size, Color, Price, customer, NumOrder, idbike) SELECT Model, Size, Color, Price, customer, NumOrder, idbike FROM commande WHERE (Model, Size, Color, Price, customer, NumOrder, idbike) NOT IN (SELECT Model, Size, Color, Price, customer, NumOrder, idbike FROM biketodo)";
                command.ExecuteNonQuery();
            }
        }
    }

    static void CompareAndModifyElements(string connectionString)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            

            // Compare elements in biketodo with bikedone
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM biketodo WHERE EXISTS (SELECT 1 FROM bikedone WHERE bikedone.Model = biketodo.Model AND bikedone.Size = biketodo.Size AND bikedone.Color = biketodo.Color AND bikedone.Price = biketodo.Price AND bikedone.customer = 'Nice_Bike')";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string model = reader.GetString(0);
                        string size = reader.GetString(1);
                        string color = reader.GetString(2);
                        string price = reader.GetString(3);
                        string customer = reader.GetString(4);
                        string numOrder = reader.GetString(5);
                        int idbike = reader.GetInt32(6);

                        // Modify the corresponding element in bikedone
                        ModifyBikeDoneElement(connectionString, model, size, color, price, customer, numOrder, idbike);

                        // Delete the element from biketodo
                        DeleteBikeTodoElement(connectionString, model, size, color, price);
                    }
                }
            }
        }
    }
    static void ModifyBikeDoneElement(string connectionString, string model, string size, string color, string price, string customer, string numOrder, int idbike)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Modify the corresponding element in bikedone
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE bikedone SET Customer = @customer, NumOrder = @numOrder,idbike = @idbike WHERE Model = @model AND Size = @size AND Color = @color AND Price = @price AND Customer = 'Nice_Bike' LIMIT 1";
                command.Parameters.AddWithValue("@customer", customer);
                command.Parameters.AddWithValue("@numOrder", numOrder);
                command.Parameters.AddWithValue("@model", model);
                command.Parameters.AddWithValue("@size", size);
                command.Parameters.AddWithValue("@color", color);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@idbike", idbike);
                command.ExecuteNonQuery();
            }
        }
    }
    static void DeleteBikeTodoElement(string connectionString, string model, string size, string color, string price)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            // Delete the element from biketodo
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM biketodo WHERE Model = @model AND Size = @size AND Color = @color AND Price = @price";
                command.Parameters.AddWithValue("@model", model);
                command.Parameters.AddWithValue("@size", size);
                command.Parameters.AddWithValue("@color", color);
                command.Parameters.AddWithValue("@price", price);
                command.ExecuteNonQuery();
            }
        }
    }
    private void SendCartToDatabase(List<Bike> cartItems)
    {
        string connectionString = "server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                int NumOrder = GenerateOrderNumber();
                Client selectedClient = (Client)ClientsPicker.SelectedItem;
                foreach (Bike item in cartItems)
                {
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        string query = "INSERT INTO commande (model, size, color, price, NumOrder, customer) VALUES (@model, @size, @color, @price, @NumOrder, @customer)";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@model", item.Model);
                        command.Parameters.AddWithValue("@size", item.Size);
                        command.Parameters.AddWithValue("@color", item.Color);
                        command.Parameters.AddWithValue("@price", item.Price);
                        command.Parameters.AddWithValue("@NumOrder", NumOrder);
                        command.Parameters.AddWithValue("@customer", selectedClient.Nom);
                        command.ExecuteNonQuery();
                        CopyNewElementsFromCommandeToBikeTodo(connectionString);
                        CompareAndModifyElements(connectionString);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadClients();
    }

    private async void LoadClients()
    {
        try
        {
            List<Client> clients = await Task.Run(() =>
            {
                // Charger les clients à partir de la base de données
                return GetClientsFromDatabase();
            });

            // Mettre à jour l'interface utilisateur
            Device.BeginInvokeOnMainThread(() =>
            {
                ClientsPicker.ItemsSource = clients.OrderBy(c => c.Nom).ToList();
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private List<Client> GetClientsFromDatabase()
    {
        string connectionString = "server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;";
        List<Client> clients = new List<Client>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT name, address, TVA FROM clients";
            MySqlCommand command = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Client client = new Client
                    {
                        Nom = reader.GetString(0),
                        Adresse = reader.GetString(1),
                        TVA = reader.GetString(2)
                    };
                    clients.Add(client);
                }
            }
        }
        return clients;
    }

    private async void EnregistrerButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new AddClients());
    }

    private void ClientsPicker_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        Client selectedClient = (Client)ClientsPicker.SelectedItem;
        if (selectedClient != null)
        {
            DisplayAlert("Selected Client", selectedClient.Nom, "OK");
        }
    }
    private int GenerateOrderNumber()
    {
        Random rnd = new Random();
        return rnd.Next(100000, 999999);
    }
    private void UpdateDeliveryDate()
    {
        int totalBikesOrdered = cartItems.Sum(item => item.Quantity);
        int totalDaysNeeded = (int)Math.Ceiling((double)totalBikesOrdered / 21);
        totalDaysNeeded++;
        DateTime estimatedDeliveryDate = DateTime.Now.AddDays(totalDaysNeeded);

        deliveryDateLabel.Text = "Estimated Delivery Date: " + estimatedDeliveryDate.ToString("yyyy-MM-dd");
    }
}