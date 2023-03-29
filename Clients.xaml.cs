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
                idclients = reader.GetString(0),
                name = reader.GetString(1),
                address = reader.GetString(2),
                TVA = reader.GetString(3)
            };
            dataList.Add(rowData);
        }

        // Fermer le lecteur et la connexion à la base de données
        reader.Close();
        conn.Close();

        // Afficher les données dans une liste dans l'interface utilisateur
        myListView.ItemsSource = dataList;

    }
    public async void Update(object sender, EventArgs e)
    {
        // Récupérer les données de la ligne sélectionnée dans la liste
        var selectedItem = (sender as Button).BindingContext as MyTableData;

        // Créer une connexion à la base de données MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        try
        {
            // Ouvrir la connexion à la base de données
            await conn.OpenAsync();

            // Créer une requête UPDATE pour mettre à jour les données dans la table
            string query = "UPDATE clients SET name = @name, address = @address, TVA = @TVA WHERE idclients = @idclients";

            // Créer un objet MySqlCommand pour exécuter la requête UPDATE
            MySqlCommand cmd = new MySqlCommand(query, conn);

            // Ajouter les paramètres de la requête UPDATE
            cmd.Parameters.AddWithValue("@idclients", selectedItem.idclients);
            cmd.Parameters.AddWithValue("@name", selectedItem.name);
            cmd.Parameters.AddWithValue("@address", selectedItem.address);
            cmd.Parameters.AddWithValue("@TVA", selectedItem.TVA);

            // Exécuter la requête UPDATE
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
            // Fermer la connexion à la base de données
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

