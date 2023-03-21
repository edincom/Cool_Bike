using System.Collections.ObjectModel;


namespace Nice_bike;
/* private List<Client> _clients;
 private Client _selectedClient;

 public Client SelectedClient
 {
     get { return _selectedClient; }
     set { _selectedClient = value; }
 }*/
public class Bike
{
    public string Model { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; } = 1;

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
    /*     _clients = GetClients();
         clientsListBox.ItemsSource = _clients;*/

    public Panier()
    {

        InitializeComponent();


        // Afficher la liste des produits
        bikeListe.ItemsSource = bikes;

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
            // Envoyer la commande à un service de commande
            // ...
            /*  using MySql.Data.MySqlClient;

              ...

              private void SendCartToDatabase(List<Bike> cartItems)
              {
              string connectionString = "server=localhost;database=mydatabase;uid=myusername;password=mypassword;";

              try
              {
                  using (MySqlConnection connection = new MySqlConnection(connectionString))
                  {
                      connection.Open();

                      foreach (Bike item in cartItems)
                      {
                          string query = "INSERT INTO cart_items (model, size, color, price, quantity) VALUES (@model, @size, @color, @price, @quantity)";
                          MySqlCommand command = new MySqlCommand(query, connection);
                          command.Parameters.AddWithValue("@model", item.Model);
                          command.Parameters.AddWithValue("@size", item.Size);
                          command.Parameters.AddWithValue("@color", item.Color);
                          command.Parameters.AddWithValue("@price", item.Price);
                          command.Parameters.AddWithValue("@quantity", item.Quantity);
                          command.ExecuteNonQuery();
                      }
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine(ex.Message);
              }
          }*/

            // Vider le panier
            ClearCart();

            // Afficher un message de confirmation
            await DisplayAlert("Succès", "Votre commande a été passée avec succès !", "OK");


            // Naviguer vers la page d'accueil
            await Navigation.PopToRootAsync();
        }
    }

}