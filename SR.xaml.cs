namespace Nice_bike;

public partial class SR : ContentPage
{
	public SR()
	{
		InitializeComponent();
	}
    private void Button_Adventure_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Adventure());

    }
    private void Button_Explorer_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Explorer());

    }
    private void Button_City_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new City());

    }
    private void Button_Pannier_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Pannier());

    }
    
}