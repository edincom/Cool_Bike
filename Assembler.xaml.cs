using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Maui.Controls;


namespace Nice_bike;

public partial class Assembler : ContentPage
{
    List<MyBike> dataList = new List<MyBike>();

    public Assembler()

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
            MyBike rowData = new MyBike()
            {
                idcommande = reader.IsDBNull(0) ? null : reader.GetString(0),
                model = reader.IsDBNull(1) ? null : reader.GetString(1),
                size = reader.IsDBNull(2) ? null : reader.GetString(2),
                color = reader.IsDBNull(3) ? null : reader.GetString(3),
                price = reader.IsDBNull(4) ? null : reader.GetString(4),

            };
            dataList.Add(rowData);
        }

        // Fermer le lecteur et la connexion à la base de données
        reader.Close();
        conn.Close();

        // Afficher les données dans une liste dans l'interface utilisateur
        myListView.ItemsSource = dataList;

    }
    public class MyBike
    {
        public string idcommande { get; set; }
        public string model { get; set; }
        public string size { get; set; }
        public string color { get; set; }

        public string price { get; set; }


        public int rowData { get; set; }
    }


    public void Switch_Toggled(object sender, EventArgs e)
    {
        MyBike rowData = this.myListView.SelectedItem as MyBike;
        var selectedItem = myListView.SelectedItem;
        Switch switchControl = (Switch)sender; // Convertir l'objet sender en Switch



        if (selectedItem != null)
        {
            try
            {
                MySqlConnection conn1 = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");
                conn1.Open();

                // Créer la requête DELETE pour supprimer l'élément de la table commande
                string query = "DELETE FROM commande WHERE idcommande = @idcommande";
                MySqlCommand command1 = new MySqlCommand(query, conn1);
                command1.Parameters.AddWithValue("@idcommande", rowData.idcommande);

                command1.ExecuteNonQuery();

                conn1.Close();


                MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");
                conn.Open();
                // Ouvrir la connexion à la base de données
                query = "INSERT INTO bike_done (idcommande,model, size, color, price) VALUES (@idcommande,@model, @size, @color, @price )";
                MySqlCommand command = new MySqlCommand(query, conn);

                // Enregistrerla nouvelle table
                command.Parameters.AddWithValue("@idcommande", rowData.idcommande);
                command.Parameters.AddWithValue("@model", rowData.model);
                command.Parameters.AddWithValue("@size", rowData.size);
                command.Parameters.AddWithValue("@color", rowData.color);
                command.Parameters.AddWithValue("@price", rowData.price);
                command.ExecuteNonQuery();

                conn.Close();
                dataList.Remove(rowData);

                myListView.ItemsSource = null;
                myListView.ItemsSource = dataList;
            }
            catch
            {
                DisplayAlert("Error", "Id already exist in database", "OK");
            }

        }

        else
        {
            switchControl.IsToggled = false;

        }


    }





}
