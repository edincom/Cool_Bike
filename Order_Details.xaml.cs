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
        // Ouvrir une connexion � la base de donn�es MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        // Ouvrir la connexion � la base de donn�es
        conn.Open();

        // Cr�er une requ�te SELECT pour extraire les donn�es de la table
        string query = "SELECT * FROM commande";

        // Ouvrir un objet MySqlCommand pour ex�cuter la requ�te SELECT
        MySqlCommand cmd = new MySqlCommand(query, conn);

        // Ex�cuter la requ�te SELECT et r�cup�rer les r�sultats
        MySqlDataReader reader = cmd.ExecuteReader();

        // Cr�er une liste d'objets pour stocker les donn�es de la table



        // Parcourir les r�sultats de la requ�te SELECT et stocker les donn�es dans la liste
        while (reader.Read())
        {
            MyTableData rowData = new MyTableData()
            {
                id = reader.GetInt32(0),
                model = reader.GetString(1),
                size = reader.GetInt32(2),
                color = reader.GetString(3),
                price = reader.GetInt32(4),
                quantity = reader.GetInt32(5)

            };
            dataList.Add(rowData);
        }

        // Fermer le lecteur et la connexion � la base de donn�es
        reader.Close();
        conn.Close();

        // Afficher les donn�es dans une liste dans l'interface utilisateur
        myListView.ItemsSource = dataList;

    }
    public class MyTableData
    {
        public int id { get; set; }
        public string model { get; set; }
        public int size { get; set; }
        public string color { get; set; }

        public int price { get; set; }

        public int quantity { get; set; }
        public int rowData { get; set; }
    }
}
