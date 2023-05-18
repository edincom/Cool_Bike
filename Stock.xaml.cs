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
        string query = "SELECT * FROM commande";


        // Ouvrir un objet MySqlCommand pour exécuter la requête SELECT
        MySqlCommand cmd = new MySqlCommand(query, conn);

        // Exécuter la requête SELECT et récupérer les résultats
        MySqlDataReader reader = cmd.ExecuteReader();

        // Créer une liste d'objets pour stocker les données de la table

        List<MyTableData> dataList = new List<MyTableData>();

        // Parcourir les résultats de la requête SELECT et stocker les données dans la liste
        while (reader.Read())
        {
            string cust = reader.IsDBNull(7) ? null : reader.GetString(7);

            if (cust == "Nice_Bike")
            {
                MyTableData rowData = new MyTableData()
                {
                    model = reader.IsDBNull(1) ? null : reader.GetString(1),
                    size = reader.IsDBNull(2) ? null : reader.GetString(2),
                    color = reader.IsDBNull(3) ? null : reader.GetString(3),
                    customer = reader.IsDBNull(7) ? null : reader.GetString(7),
                };

            dataList.Add(rowData);


        }
    }
        // Fermer le lecteur et la connexion à la base de données
        reader.Close();
        conn.Close();

        // Afficher les données dans une liste dans l'interface utilisateur
        myListView.ItemsSource = dataList;

    }

    public class MyTableData
    {
        public string model { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public string customer { get; set; }


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