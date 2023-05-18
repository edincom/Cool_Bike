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

        // Fermer le lecteur et la connexion � la base de donn�es
        reader.Close();
        conn.Close();

        // Afficher les donn�es dans une liste dans l'interface utilisateur
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

                // Cr�er la requ�te DELETE pour supprimer l'�l�ment de la table commande
                string query = "DELETE FROM commande WHERE idcommande = @idcommande";
                MySqlCommand command1 = new MySqlCommand(query, conn1);
                command1.Parameters.AddWithValue("@idcommande", rowData.idcommande);

                command1.ExecuteNonQuery();

                conn1.Close();


                MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");
                conn.Open();
                // Ouvrir la connexion � la base de donn�es
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
