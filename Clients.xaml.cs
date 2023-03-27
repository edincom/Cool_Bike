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
        // Cr�er une connexion � la base de donn�es MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        // Ouvrir la connexion � la base de donn�es
        conn.Open();

        // Cr�er une requ�te SELECT pour extraire les donn�es de la table
        string query = "SELECT * FROM clients";

        // Cr�er un objet MySqlCommand pour ex�cuter la requ�te SELECT
        MySqlCommand cmd = new MySqlCommand(query, conn);

        // Ex�cuter la requ�te SELECT et r�cup�rer les r�sultats
        MySqlDataReader reader = cmd.ExecuteReader();

        // Cr�er une liste d'objets pour stocker les donn�es de la table

        List<MyTableData> dataList = new List<MyTableData>();

        // Parcourir les r�sultats de la requ�te SELECT et stocker les donn�es dans la liste
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

        // Fermer le lecteur et la connexion � la base de donn�es
        reader.Close();
        conn.Close();

        // Afficher les donn�es dans une liste dans l'interface utilisateur
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