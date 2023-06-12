using Microsoft.Extensions.Logging.Abstractions;
using MySql.Data.MySqlClient;
using Microsoft.Maui.Controls.PlatformConfiguration;


namespace Nice_bike;

public partial class Stock : ContentPage
{
    public Stock()
    {
        InitializeComponent();

        // Ouvrir une connexion à la base de données MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        // Ouvrir la connexion à la base de données
        conn.Open();


        // Créer une requête SELECT pour extraire les données de la table
        string query = "SELECT * FROM bikedone";


        // Ouvrir un objet MySqlCommand pour exécuter la requête SELECT
        MySqlCommand cmd = new MySqlCommand(query, conn);

        // Exécuter la requête SELECT et récupérer les résultats
        MySqlDataReader reader = cmd.ExecuteReader();

        // Créer une liste d'objets pour stocker les données de la table

        List<MyTableData> dataList1 = new List<MyTableData>();
        List<MyTableData> dataList2 = new List<MyTableData>();
        List<MyTableData> dataList3 = new List<MyTableData>();

        // Parcourir les résultats de la requête SELECT et stocker les données dans la liste
        while (reader.Read())
        {
            string cust = reader.IsDBNull(4) ? null : reader.GetString(4);
            string model = reader.IsDBNull(0) ? null : reader.GetString(0);

            if (cust == "Nice_Bike" & model == "Adventure")
            {
                MyTableData rowData1 = new MyTableData()
                {
                    size1 = reader.IsDBNull(1) ? null : reader.GetString(1),
                    color1 = reader.IsDBNull(2) ? null : reader.GetString(2),
                };
                dataList1.Add(rowData1);

            }
            else if (cust == "Nice_Bike" & model == "Explorer")
            {
                MyTableData rowData2 = new MyTableData()
                {
                    size2 = reader.IsDBNull(1) ? null : reader.GetString(1),
                    color2 = reader.IsDBNull(2) ? null : reader.GetString(2),
                };


                dataList2.Add(rowData2);

            }
            else if (cust == "Nice_Bike" & model == "City")
            {
                MyTableData rowData3 = new MyTableData()
                {
                    size3 = reader.IsDBNull(1) ? null : reader.GetString(1),
                    color3 = reader.IsDBNull(2) ? null : reader.GetString(2),
                };
                dataList3.Add(rowData3);

            }


        }
        // Fermer le lecteur et la connexion à la base de données
        reader.Close();
        conn.Close();

        // Afficher les données dans une liste dans l'interface utilisateur
        myListView1.ItemsSource = dataList1;
        myListView2.ItemsSource = dataList2;
        myListView3.ItemsSource = dataList3;

    }

    public class MyTableData
    {
        public string size1 { get; set; }
        public string color1 { get; set; }

        public string size2 { get; set; }
        public string color2 { get; set; }

        public string size3 { get; set; }
        public string color3 { get; set; }

        public string Size { get; set; }
        public string Color { get; set; }

        public string Model { get; set; }

        public int? Quantity { get; set; }

        public MyTableData()
        {
            Quantity = null;
        }


    }


    private async void Checkout_Clicked(object sender, EventArgs e)
    {
        // Afficher une confirmation pour confirmer la commande
        bool answer = await DisplayAlert("Confirmation", "Do you want to confirm th order ?", "Yes", "No");

        if (answer)
        {

            SendCartToDatabase();

            // Afficher un message de confirmation
            await DisplayAlert("Succes", "The order has been successfully registered!", "OK");


            // Naviguer vers la page d'accueil
            await Navigation.PopToRootAsync();
        }
    }

    private int GenerateOrderNumber()
    {
        Random rnd = new Random();
        return rnd.Next(100000, 999999);
    }
    private void SendCartToDatabase()
    {
        string connectionString = "server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;";
        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                int quantity = int.Parse(resultLabel2.Text);
                int NumOrder = GenerateOrderNumber();
                int idbike = GenerateOrderNumber();
                for (int i = 0; i < quantity; i++)
                {
                    string query = "INSERT INTO biketodo (Model, Size, Color, Price, customer, NumOrder, idbike) VALUES (@model, @size, @color, @price, @customer, @NumOrder, @idbike)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@model", MyModel.Text);
                    command.Parameters.AddWithValue("@size", MySize.Text);
                    command.Parameters.AddWithValue("@color", MyColor.Text);
                    command.Parameters.AddWithValue("@price", "200");
                    command.Parameters.AddWithValue("@customer", "Nice_Bike");
                    command.Parameters.AddWithValue("@NumOrder", NumOrder);
                    command.Parameters.AddWithValue("@idbike", idbike);
                    command.ExecuteNonQuery();
                }
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    private void Picker1_1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSize1();
    }

    private void UpdateSize1()
    {
        string selected1 = Size.SelectedItem.ToString();
        MySize.Text = selected1;
    }

    private void Picker1_2_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateColor1();
    }

    private void UpdateColor1()
    {
        string selected1 = Color.SelectedItem.ToString();
        MyColor.Text = selected1;
    }

    private void Picker1_3_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateModel();
    }

    private void UpdateModel()
    {
        string selected1 = Model.SelectedItem.ToString();
        MyModel.Text = selected1;
    }

    private void OnGetNumberClicked(object sender, EventArgs e)
    {
        int number;
        if (int.TryParse(QuantityEntry2.Text, out number))
        {
            resultLabel2.Text = number.ToString();
        }
        else
        {
            resultLabel2.Text = "Invalid input. Please enter a valid number.";
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var Panier = new Panier();
        Panier.bikes.Add(new Bike(MyModel.Text, MySize.Text, MyColor.Text, 200, Convert.ToInt32(resultLabel2.Text)));
        await Navigation.PushAsync(Panier);
    }
}