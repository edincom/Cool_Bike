using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Maui.Controls;
namespace Nice_bike;

public partial class Clients : ContentPage
{

    public Clients()

    {
        InitializeComponent();
        // Créer une connexion à la base de données MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        // Ouvrir la connexion à la base de données
        conn.Open();

        // Créer une requête SELECT pour extraire les données de la table
        string query = "SELECT * FROM clients";

        // Créer un objet MySqlCommand pour exécuter la requête SELECT
        MySqlCommand cmd = new MySqlCommand(query, conn);

        // Exécuter la requête SELECT et récupérer les résultats
        MySqlDataReader reader = cmd.ExecuteReader();

        // Créer une liste d'objets pour stocker les données de la table

        List<MyTableData> dataList = new List<MyTableData>();

        // Parcourir les résultats de la requête SELECT et stocker les données dans la liste
        while (reader.Read())
        {
            MyTableData rowData = new MyTableData()
            {
                Propriete1 = reader.GetString(0),
                Propriete2 = reader.GetString(1),
                Propriete3 = reader.GetString(2),
                Propriete4 = reader.GetString(3)
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
        public string Propriete1 { get; set; }
        public string Propriete2 { get; set; }
        public string Propriete3 { get; set; }
        public string Propriete4 { get; set; }
    }
}