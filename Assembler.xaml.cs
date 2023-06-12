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
        string query = "SELECT * FROM biketodo";

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
                model = reader.IsDBNull(0) ? null : reader.GetString(0),
                size = reader.IsDBNull(1) ? null : reader.GetString(1),
                color = reader.IsDBNull(2) ? null : reader.GetString(2),
                price = reader.IsDBNull(3) ? null : reader.GetString(3),
                customer = reader.IsDBNull(4) ? null : reader.GetString(4),
                numOrder = reader.IsDBNull(5) ? null : reader.GetString(5),
                idbike = reader.GetInt32(6)

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
        public string numOrder { get; set; }

        public string customer { get; set; }
        public string model { get; set; }
        public string size { get; set; }
        public string color { get; set; }

        public string price { get; set; }

        public int idbike { get; set; }


        public int rowData { get; set; }
    }

    private MyBike rowData;

    public void Switch_Finished_Toggled(object sender, ToggledEventArgs e)
    {
        rowData = this.myListView.SelectedItem as MyBike;

        var selectedItem = myListView.SelectedItem;



        if (selectedItem != null)
        {

            MySqlConnection conn1 = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");
            conn1.Open();

            // Créer la requête DELETE pour supprimer l'élément de la table commande
            string query = "DELETE FROM biketodo WHERE idbike = @idbike";
            MySqlCommand command1 = new MySqlCommand(query, conn1);
            command1.Parameters.AddWithValue("@idbike", rowData.idbike);

            command1.ExecuteNonQuery();

            conn1.Close();


            MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");
            conn.Open();
            // Ouvrir la connexion à la base de données
            query = "INSERT INTO bikedone (model, size, color, price,customer,numOrder,idbike) VALUES (@model, @size, @color, @price,@customer,@NumOrder,@idbike)";
            MySqlCommand command = new MySqlCommand(query, conn);

            // Enregistrerla nouvelle table
            command.Parameters.AddWithValue("@model", rowData.model);
            command.Parameters.AddWithValue("@size", rowData.size);
            command.Parameters.AddWithValue("@color", rowData.color);
            command.Parameters.AddWithValue("@price", rowData.price);
            command.Parameters.AddWithValue("@customer", rowData.customer);
            command.Parameters.AddWithValue("@NumOrder", rowData.numOrder);
            command.Parameters.AddWithValue("@idbike", rowData.idbike);
            command.ExecuteNonQuery();

            conn.Close();
            dataList.Remove(rowData);

            myListView.ItemsSource = null;
            myListView.ItemsSource = dataList;

        }


    }


    private async Task SubtractUnitsFromStock(List<string> components)
    {
        using MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        try
        {
            await conn.OpenAsync();

            Dictionary<string, int> unitsToSubtract = new Dictionary<string, int>()
            {
                { "NUT 1", 2 },
                { "NUT 2", 2 },
                { "NUT 3", 2 },
                { "NUT 4", 2 },
                { "NUT 5", 2 },
                { "NUT 6", 2 },
                { "NUT 7", 2 },
                { "NUT 8", 2 },
                { "Frame", 1 },
                { "Brakes", 2 },
                { "Tires", 2 },
                { "Inner Tubes", 2 },
                { "Shock Absorber", 2 },
                { "SCREW 1", 2 },
                { "SCREW 2", 2 },
                { "SCREW 3", 2 },
                { "SCREW 4", 2 },
                { "SCREW 5", 2 },
                { "SCREW 6", 2 },
                { "SCREW 7", 2 },
                { "SCREW 8", 2 },
                { "Red Painting", 1 },
                { "Blue Painting", 1},
                { "Orange Painting", 1},
                { "Black Painting", 1},
                { "Yellow Painting", 1},
                { "Purple Painting", 1},
                { "White Painting", 1}



            };

            foreach (var component in components)
            {
                int units = unitsToSubtract.GetValueOrDefault(component, 0);

                if (units > 0)
                {
                    // Faire une requête UPDATE pour soustraire la quantité dans la table stock
                    string query = $"UPDATE stock SET quantity = GREATEST(quantity - @unitsToSubtract, 0) WHERE components = @component";
                    using MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@unitsToSubtract", units);
                    cmd.Parameters.AddWithValue("@component", component);

                    // Exécuter la requête UPDATE
                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error: {ex.Message}", "OK");
        }
        finally
        {
            conn.Close();
        }
    }




    private async void Switch_Loading_Toggled(object sender, ToggledEventArgs e)
    {
        rowData = this.myListView.SelectedItem as MyBike;
        var selectedItem = myListView.SelectedItem;

        List<string> componentsToSubtract = new List<string>()
        {
            "NUT 1",
            "NUT 2",
            "NUT 3",
            "NUT 4",
            "NUT 5",
            "NUT 6",
            "NUT 7",
            "NUT 8",
            "Frame",
            "Brakes",
            "Tires",
            "Inner Tubes",
            "Shock Absorber",
            "SCREW 1",
            "SCREW 2",
            "SCREW 3",
            "SCREW 4",
            "SCREW 5",
            "SCREW 6",
            "SCREW 7",
            "SCREW 8"
        };

        if (selectedItem != null)
        {
            if (rowData.color == "Red")
            {
                componentsToSubtract.Add("Red Painting");
            }

            else if (rowData.color == "Blue")
            {
                componentsToSubtract.Add("Blue Painting");
            }
            else if (rowData.color == "Green")
            {
                componentsToSubtract.Add("Green Painting");
            }
            else if (rowData.color == "White")
            {
                componentsToSubtract.Add("White Painting");
            }
            else if (rowData.color == "Orange")
            {
                componentsToSubtract.Add("Orange Painting");
            }
            else if (rowData.color == "Yellow")
            {
                componentsToSubtract.Add("Yellow Painting");
            }
            else if (rowData.color == "Black")
            {
                componentsToSubtract.Add("Black Painting");
            }
            else if (rowData.color == "Purple")
            {
                componentsToSubtract.Add("Purple Painting");
            }

            if (e.Value)
            {
                await SubtractUnitsFromStock(componentsToSubtract);
            }

        }
    }


}

