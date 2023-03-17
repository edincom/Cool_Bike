

namespace Nice_bike;

public partial class Adventure : ContentPage
{

    public Adventure()
	{
		InitializeComponent();

    }

    private void UpdateSize()
    {
        string selected1 = Size.SelectedItem.ToString();
        MySize.Text = selected1;
    }
    private void UpdateColor()
    {
        string selected2 = Color.SelectedItem.ToString();
        MyColor.Text = selected2;
    }
    private void UpdateNumber()
    {
        int number = 0;
        int.TryParse(QuantityEntry.Text, out number);
        MyNumber.Text = $"Number of bikes : {number}";
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
        int number;
        if (int.TryParse(QuantityEntry.Text, out number))
        {
            MyNumber.Text = $"Quantity : {number}";
        }
        else
        {
            resultLabel.Text = "Invalid input. Please enter a valid number.";
        }
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        var Panier = new Panier();
        Panier.bikes.Add(new Bike("Adventure", MySize.Text, MyColor.Text, 200));
        await Navigation.PushAsync(Panier);
    }
    
}