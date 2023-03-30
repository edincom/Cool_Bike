using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace Nice_bike;

public class Bike
{
    public string Model { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; } = 1 ;

    public Bike(string model, string size, string color, double price, int quantity)
    {
        Model = model;
        Size = size;
        Color = color;
        Price = price;
        Quantity = quantity;
    }
}

public partial class Panier : ContentPage
{
    public static List<Bike> bikes = new List<Bike>();
    private List<Bike> cartItems = new List<Bike>();

    private ObservableCollection<Bike> cartItemsObservableCollection = new ObservableCollection<Bike>();
    private static int OrderNumber = new Random().Next(1000, 9999);


    public Panier()
    {
        InitializeComponent();


        // Ajouter des produits
        bikes.Add(new Bike("City", "28", "purple", 200, 67));

        // Afficher la liste des produits
        bikeList.ItemsSource = bikes;

        // Afficher le panier vide
        UpdateCartTotal();

    }

    private void AddToCart_Clicked(object sender, System.EventArgs e)
    {
        var button = sender as Button;
        var bike = button.BindingContext as Bike;

        // Ajouter l'article au panier
        cartItems.Add(bike);

        // Mettre à jour le total du panier
        UpdateCartTotal();

        // Mettre à jour la liste des articles dans le panier
        cartList.ItemsSource = null;
        cartList.ItemsSource = cartItems;
    }

    private void RemoveItem_Clicked(object sender, System.EventArgs e)
    {
        var button = sender as Button;
        var item = button.BindingContext as Bike;

        if (item.Quantity > 1)
        {
            item.Quantity--;
        }
        else
        {
            cartItems.Remove(item);
        }

        UpdateTotal();
    }

    private void AddItem_Clicked(object sender, System.EventArgs e)
    {
        var button = sender as Button;
        var item = button.BindingContext as Bike;

        item.Quantity++;

        UpdateTotal();
    }

    private void UpdateTotal()
    {
        var total = cartItems.Sum(item => item.Price * item.Quantity);
        cartTotal.Text = $"Total: €{total:N2}";

        cartItems.RemoveAll(item => item.Quantity == 0); // Supprime les éléments dont la quantité est zéro
        cartList.ItemsSource = null;
        cartList.ItemsSource = cartItems; // Met à jour la liste des articles dans le panier
    }

    private void UpdateCartTotal()
    {
        double total = 0;

        foreach (var item in cartItems)
        {
            total += item.Price;
        }

        cartTotal.Text = "Total: $" + total.ToString("N2");
    }
    private void ClearCart()
    {
        cartItems.Clear();
        cartItemsObservableCollection.Clear();
    }


    private async void Checkout_Clicked(object sender, EventArgs e)
    {
        // Afficher une confirmation pour confirmer la commande
        bool answer = await DisplayAlert("Confirmation", "Confirmez-vous votre commande ?", "Oui", "Non");

        if (answer)
        {
            
            SendCartToDatabase(cartItems);
            // Vider le panier
            ClearCart();

            // Afficher un message de confirmation
            await DisplayAlert("Succès", "Votre commande a été passée avec succès !", "OK");


            // Naviguer vers la page d'accueil
            await Navigation.PopToRootAsync();
        }
    }
    private void SendCartToDatabase(List<Bike> cartItems)
    {
        string connectionString = "server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                int NumOrder = GenerateOrderNumber();
                foreach (Bike item in cartItems)
                {
                    for (int i=0; i<item.Quantity;i++)
                    {
                        string query = "INSERT INTO commande (model, size, color, price, NumOrder) VALUES (@model, @size, @color, @price, @NumOrder)";
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@model", item.Model);
                        command.Parameters.AddWithValue("@size", item.Size);
                        command.Parameters.AddWithValue("@color", item.Color);
                        command.Parameters.AddWithValue("@price", item.Price);
                        command.Parameters.AddWithValue("@NumOrder", NumOrder);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadClients();
    }

    private async void LoadClients()
    {
        try
        {
            List<Client> clients = await Task.Run(() =>
            {
                // Charger les clients à partir de la base de données
                return GetClientsFromDatabase();
            });

            // Mettre à jour l'interface utilisateur
            Device.BeginInvokeOnMainThread(() =>
            {
                ClientsPicker.ItemsSource = clients.OrderBy(c => c.Nom).ToList();
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }

    private List<Client> GetClientsFromDatabase()
    {
        string connectionString = "server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;";
        List<Client> clients = new List<Client>();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT name, address, TVA FROM clients";
            MySqlCommand command = new MySqlCommand(query, connection);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Client client = new Client
                    {
                        Nom = reader.GetString(0),
                        Adresse = reader.GetString(1),
                        TVA = reader.GetString(2)
                    };
                    clients.Add(client);
                }
            }
        }
        return clients;
    }

    private async void EnregistrerButton_Clicked(object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new AddClients());
    }

    private void ClientsPicker_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        Client selectedClient = (Client)ClientsPicker.SelectedItem;
        if (selectedClient != null)
        {
            DisplayAlert("Client sélectionné", selectedClient.Nom, "OK");
        }
    }
    private int GenerateOrderNumber()
    {
        Random rnd = new Random();
        return rnd.Next(100000, 999999);
    }
}