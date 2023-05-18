using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Maui.Controls;
namespace Nice_bike;

public partial class Order_Details : ContentPage
{
    List<MyTableData> dataList = new List<MyTableData>();
    public Order_Details()
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



        // Parcourir les résultats de la requête SELECT et stocker les données dans la liste
        while (reader.Read())
        {
            MyTableData rowData = new MyTableData()
            {
                id = reader.IsDBNull(0) ? null : reader.GetString(0),
                model = reader.IsDBNull(1) ? null : reader.GetString(1),
                size = reader.IsDBNull(2) ? null : reader.GetString(2),
                color = reader.IsDBNull(3) ? null : reader.GetString(3),
                price = reader.IsDBNull(4) ? null : reader.GetString(4),
                quantity = reader.IsDBNull(5) ? null : reader.GetString(5)

            };
            dataList.Add(rowData);
        }

        // Fermer le lecteur et la connexion à la base de données
        reader.Close();
        conn.Close();

        // Afficher les données dans une liste dans l'interface utilisateur
        myListView.ItemsSource = dataList;

    }
    public class MyTableData
    {
        public string id { get; set; }
        public string model { get; set; }
        public string size { get; set; }
        public string color { get; set; }

        public string price { get; set; }

        public string quantity { get; set; }
        public int rowData { get; set; }
    }
}
