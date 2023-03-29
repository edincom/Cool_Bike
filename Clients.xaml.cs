using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Maui.Controls;
using System.Diagnostics.Metrics;

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
                idclients = reader.GetString(0),
                name = reader.GetString(1),
                address = reader.GetString(2),
                TVA = reader.GetString(3)
            };
            dataList.Add(rowData);
        }

        // Fermer le lecteur et la connexion � la base de donn�es
        reader.Close();
        conn.Close();

        // Afficher les donn�es dans une liste dans l'interface utilisateur
        myListView.ItemsSource = dataList;

    }
    public async void Update(object sender, EventArgs e)
    {
        // R�cup�rer les donn�es de la ligne s�lectionn�e dans la liste
        var selectedItem = (sender as Button).BindingContext as MyTableData;

        // Cr�er une connexion � la base de donn�es MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        try
        {
            // Ouvrir la connexion � la base de donn�es
            await conn.OpenAsync();

            // Cr�er une requ�te UPDATE pour mettre � jour les donn�es dans la table
            string query = "UPDATE clients SET name = @name, address = @address, TVA = @TVA WHERE idclients = @idclients";

            // Cr�er un objet MySqlCommand pour ex�cuter la requ�te UPDATE
            MySqlCommand cmd = new MySqlCommand(query, conn);

            // Ajouter les param�tres de la requ�te UPDATE
            cmd.Parameters.AddWithValue("@idclients", selectedItem.idclients);
            cmd.Parameters.AddWithValue("@name", selectedItem.name);
            cmd.Parameters.AddWithValue("@address", selectedItem.address);
            cmd.Parameters.AddWithValue("@TVA", selectedItem.TVA);

            // Ex�cuter la requ�te UPDATE
            await cmd.ExecuteNonQueryAsync();

            // Afficher un message de confirmation
            await DisplayAlert("Success", "Data updated successfully.", "OK");
        }
        catch (Exception ex)
        {
            // Afficher un message d'erreur
            await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
        }
        finally
        {
            // Fermer la connexion � la base de donn�es
            conn.Close();
        }
    }
    public class MyTableData
    {
        public string idclients { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string TVA { get; set; }
    }
}

