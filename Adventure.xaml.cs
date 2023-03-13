namespace Nice_bike;

public partial class Adventure : ContentPage
{
	public Adventure()
	{
		InitializeComponent();
	}

    private void UpdateLabel()
    {
        string selected1 = Size.SelectedItem.ToString();
        string selected2 = Color.SelectedItem.ToString();
        int number = 0;
        int.TryParse(QuantityEntry.Text, out number);
        MyLabel.Text = $"Size : {selected1} | Color : {selected2} | Number of bikes : {number}";
    }
    private void Picker1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateLabel();
    }

    private void Picker2_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateLabel();
    }

    private void MyEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateLabel();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Panier(MyLabel.Text));
    }
}