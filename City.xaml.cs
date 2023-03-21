namespace Nice_bike;

public partial class City : ContentPage
{

    public City()
    {
        InitializeComponent();

    }

    private void UpdateSize()
    {
        string selected1 = Size1.SelectedItem.ToString();
        MySize1.Text = selected1;
    }
    private void UpdateColor()
    {
        string selected2 = Color1.SelectedItem.ToString();
        MyColor1.Text = selected2;
    }
    private void UpdateNumber()
    {
        int number = 0;
        int.TryParse(QuantityEntry1.Text, out number);
        MyNumber1.Text = $"Number of bikes : {number}";
    }


    private void Picker1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSize();
    }

    private void Picker2_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateColor();
    }

    private void MyEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateNumber();
    }

    private void OnGetNumberClicked(object sender, EventArgs e)
    {
        int number1;
        if (int.TryParse(QuantityEntry1.Text, out number1))
        {
            MyNumber1.Text = number1.ToString();
        }
        else
        {
            resultLabel1.Text = "Invalid input. Please enter a valid number.";
        }
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var Panier = new Panier();
        Panier.bikes.Add(new Bike("City", MySize1.Text, MyColor1.Text, 200, Convert.ToInt32(MyNumber1.Text)));
        await Navigation.PushAsync(Panier);
    }

}