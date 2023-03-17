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

        if (!string.IsNullOrWhiteSpace(nom))
        {
            Client client = new Client { Nom = nom, Adresse = adresse, TVA = tva };
            ClientManager.AjouterClient(client);

            await DisplayAlert("Succ�s", "Le client a �t� enregistr�", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Erreur", "Le nom du client est obligatoire", "OK");
        }
    }
}