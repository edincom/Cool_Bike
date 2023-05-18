
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Microsoft.Maui.Controls;


namespace Nice_bike;

public partial class PP : ContentPage
{
    List<MyTableData> dataList = new List<MyTableData>();

    public PP()

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
                id = reader.IsDBNull(0) ? null : reader.GetString(0),
                model = reader.IsDBNull(1) ? null : reader.GetString(1),
                size = reader.IsDBNull(2) ? null : reader.GetString(2),
                color = reader.IsDBNull(3) ? null : reader.GetString(3),
                price = reader.IsDBNull(4) ? null : reader.GetString(4),
                quantity = reader.IsDBNull(5) ? null : reader.GetString(5)

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
        public string id { get; set; }
        public string model { get; set; }
        public string size { get; set; }
        public string color { get; set; }

        public string price { get; set; }

        public string quantity { get; set; }
        public int rowData { get; set; }
    }

    public void myListView_DeleteClick(object sender, EventArgs e)
    {

        MyTableData rowData = this.myListView.SelectedItem as MyTableData;
        var selectedItem = myListView.SelectedItem;

        try
        {
            if (selectedItem != null)
            {
                dataList.Remove(rowData);

                myListView.ItemsSource = null;
                myListView.ItemsSource = dataList;
            }
            else
            {
                DisplayAlert("Error", "Commande non s�lectionn�", "OK");
            }


        }
        catch
        {
            DisplayAlert("Error", "Commande non s�lectionn�", "OK");
        }
    }
    public void myListView_UpClick(object sender, EventArgs e)
    {
        MyTableData rowData = this.myListView.SelectedItem as MyTableData;
        var selectedItem = myListView.SelectedItem;
        var oldIndex = dataList.IndexOf(rowData);
        try
        {
            if (oldIndex > 0)
            {
                dataList.Remove(rowData);
                dataList.Insert((oldIndex - 1), rowData);

                myListView.ItemsSource = null;
                myListView.ItemsSource = dataList;

            }
            else if (oldIndex != 0)
            {
                DisplayAlert("Error", "Commande non s�lectionn�", "OK");
            }
        }
        catch
        {
            DisplayAlert("Error", "Commande non s�lectionn�", "OK");
        }



    }
    public void myListView_DownClick(object sender, EventArgs e)
    {
        MyTableData rowData = this.myListView.SelectedItem as MyTableData;
        var selectedItem = myListView.SelectedItem;
        var oldIndex = dataList.IndexOf(rowData);
        try
        {
            if ((oldIndex < dataList.Count - 1) && (oldIndex >= 0))
            {
                dataList.Remove(rowData);
                dataList.Insert((oldIndex + 1), rowData);

                myListView.ItemsSource = null;
                myListView.ItemsSource = dataList;

            }
            else if (oldIndex != dataList.Count - 1)
            {
                DisplayAlert("Error", "Commande non s�lectionn�", "OK");
            }


        }
        catch
        {
            DisplayAlert("Error", "Commande non s�lectionn�", "OK");
        }
    }

    public void myListView_SaveClick(object sender, EventArgs e)
    {
        // Ouvrir une connexion � la base de donn�es MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        // Ouvrir la connexion � la base de donn�es
        conn.Open();

        // vider la table d'abord
        string query = "DELETE FROM commande";
        MySqlCommand command = new MySqlCommand(query, conn);
        command.ExecuteNonQuery();

        // Enregistrerla nouvelle table
        foreach (MyTableData rowData in dataList)
        {
            query = "INSERT INTO commande (model, size, color, price, quantity) VALUES (@model, @size, @color, @price, @quantity)";
            command = new MySqlCommand(query, conn);
            command.Parameters.AddWithValue("@model", rowData.model);
            command.Parameters.AddWithValue("@size", rowData.size);
            command.Parameters.AddWithValue("@color", rowData.color);
            command.Parameters.AddWithValue("@price", rowData.price);
            command.Parameters.AddWithValue("@quantity", rowData.quantity);
            command.ExecuteNonQuery();
        }
        conn.Close();
    }


}