using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace Nice_bike;

public partial class AddClients : ContentPage
{
	public AddClients()
	{
		InitializeComponent();
	}
    private async void EnregistrerButton_Clicked(object sender, System.EventArgs e)
    {
        string nom = NomEntry.Text;
        string adresse = AdresseEntry.Text;
        string tva = TVAEntry.Text;
        MySqlConnection connection = new MySqlConnection("server=localhost;database=bovelo;uid=root;password=root");
        connection.Open();
        string insertQuery = "INSERT INTO save_client (name,adresse,tva) VALUES (@Name, @Adresse, @TVA)";
        MySqlCommand command = new MySqlCommand(insertQuery, connection);

        command.Parameters.AddWithValue("@Name", NomEntry.Text);
        command.Parameters.AddWithValue("@Adresse", AdresseEntry.Text);
        command.Parameters.AddWithValue("@TVA", TVAEntry.Text);


        command.ExecuteNonQuery();

        if (!string.IsNullOrWhiteSpace(nom))
        {
            Client client = new Client { Nom = nom, Adresse = adresse, TVA = tva };
            ClientManager.AjouterClient(client);

            await DisplayAlert("Succès", "Le client a été enregistré", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Erreur", "Le nom du client est obligatoire", "OK");
        }
    }
}