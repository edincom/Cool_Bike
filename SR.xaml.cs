namespace Nice_bike;

public partial class SR : ContentPage
{
    public SR()
    {
        InitializeComponent();
    }
 
    private void Button_Catalog_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Catalog());

    }
    
    private void Button_Clients_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Clients());

    }


}