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
        MySqlConnection connection = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");
        connection.Open();
        string insertQuery = "INSERT INTO clients (name,address,tva) VALUES (@Name, @Address, @TVA)";
        MySqlCommand command = new MySqlCommand(insertQuery, connection);

        command.Parameters.AddWithValue("@Name", NomEntry.Text);
        command.Parameters.AddWithValue("@Address", AdresseEntry.Text);
        command.Parameters.AddWithValue("@TVA", TVAEntry.Text);


        command.ExecuteNonQuery();

        if (!string.IsNullOrWhiteSpace(nom))
        {
            Client client = new Client { Nom = nom, Adresse = adresse, TVA = tva };
            ClientManager.AjouterClient(client);

            await DisplayAlert("Succes", "the client has been sucessfully registered", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "the name of the client is mandatory", "OK");
        }
    }
}