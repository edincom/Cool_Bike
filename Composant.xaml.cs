using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace Nice_bike;
public partial class Composant : ContentPage
{
    List<MyTableData> dataList = new List<MyTableData>();
    public Composant()

    {
        InitializeComponent();
        // Cr�er une connexion � la base de donn�es MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        // Ouvrir la connexion � la base de donn�es
        conn.Open();

        // Cr�er une requ�te SELECT pour extraire les donn�es de la table
        string query = "SELECT * FROM stock";

        // Cr�er un objet MySqlCommand pour ex�cuter la requ�te SELECT
        MySqlCommand cmd = new MySqlCommand(query, conn);

        // Ex�cuter la requ�te SELECT et r�cup�rer les r�sultats
        MySqlDataReader reader = cmd.ExecuteReader();

        // Cr�er une liste d'objets pour stocker les donn�es de la table



        // Parcourir les r�sultats de la requ�te SELECT et stocker les donn�es dans la liste
        while (reader.Read())
        {
            MyTableData rowData = new MyTableData()
            {
                idstock = reader.GetString(0),
                components = reader.GetString(1),
                quantity = reader.GetString(2),
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
        var component = new MyTableData
        {
            components = NameOfComponent.SelectedItem.ToString(),
            quantity = Quantity.Text
        };

        // Cr�er une connexion � la base de donn�es MySQL
        MySqlConnection conn = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");

        MyTableData component_found = dataList.FirstOrDefault(c => c.components == component.components);
        if (component_found == null)
        {

            await conn.OpenAsync();

            // Cr�er une requ�te UPDATE pour mettre � jour les donn�es dans la table
            string query = "INSERT INTO stock (components,quantity) VALUES (@name, @Quantity)";

            // Cr�er un objet MySqlCommand pour ex�cuter la requ�te UPDATE
            MySqlCommand cmd = new MySqlCommand(query, conn);

            // Ajouter les param�tres de la requ�te UPDATE
            cmd.Parameters.AddWithValue("@quantity", component.quantity);
            cmd.Parameters.AddWithValue("@name", component.components);


            // Ex�cuter la requ�te UPDATE
            await cmd.ExecuteNonQueryAsync();

            // Afficher un message de confirmation
            await DisplayAlert("Success", "Data updated successfully.", "OK");

            string selectQuery = "SELECT * FROM stock";
            MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
            MySqlDataReader selectReader = selectCmd.ExecuteReader();

            // Clear the existing dataList
            dataList.Clear();

            // Fetch and update the dataList with the fresh data
            while (selectReader.Read())
            {
                MyTableData rowData = new MyTableData()
                {
                    idstock = selectReader.GetString(0),
                    components = selectReader.GetString(1),
                    quantity = selectReader.GetString(2),
                };
                dataList.Add(rowData);
            }
            selectReader.Close();

            // Close the connection
            conn.Close();

            // Notify the ListView that the data has changed
            myListView.ItemsSource = null;
            myListView.ItemsSource = dataList;
            return;

        }


        int quantity1 = int.Parse(component_found.quantity) + int.Parse(component.quantity);
        component_found.quantity = quantity1.ToString();

        try
        {
            // Ouvrir la connexion � la base de donn�es
            await conn.OpenAsync();

            // Cr�er une requ�te UPDATE pour mettre � jour les donn�es dans la table
            string query = "UPDATE stock SET quantity = @quantity WHERE components = @components";

            // Cr�er un objet MySqlCommand pour ex�cuter la requ�te UPDATE
            MySqlCommand cmd = new MySqlCommand(query, conn);

            // Ajouter les param�tres de la requ�te UPDATE
            cmd.Parameters.AddWithValue("@quantity", component_found.quantity);
            cmd.Parameters.AddWithValue("@components", component.components);


            // Ex�cuter la requ�te UPDATE
            await cmd.ExecuteNonQueryAsync();

            // Afficher un message de confirmation
            await DisplayAlert("Success", "Data updated successfully.", "OK");
            // Ex�cute la requ�te UPDATE
            await cmd.ExecuteNonQueryAsync();

            // Afficher un message de confirmation
            //await DisplayAlert("Success", "Data updated successfully.", "OK");

            // Re-fetch the data from the database and update the UI
            string selectQuery = "SELECT * FROM stock";
            MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
            MySqlDataReader selectReader = selectCmd.ExecuteReader();

            // Clear the existing dataList
            dataList.Clear();

            // Fetch and update the dataList with the fresh data
            while (selectReader.Read())
            {
                MyTableData rowData = new MyTableData()
                {
                    idstock = selectReader.GetString(0),
                    components = selectReader.GetString(1),
                    quantity = selectReader.GetString(2),
                };
                dataList.Add(rowData);
            }

            // Close the reader for the SELECT query
            selectReader.Close();

            // Close the connection
            conn.Close();

            // Notify the ListView that the data has changed
            myListView.ItemsSource = null;
            myListView.ItemsSource = dataList;
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
    private class MyTableData
    {
        public string idstock { get; set; }
        public string components { get; set; }
        public string quantity { get; set; }
    }
}