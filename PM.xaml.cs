namespace Nice_bike;

public partial class PM : ContentPage
{
    public PM()
    {
        InitializeComponent();
    }
    private void Button_PP_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new PP());

    }

    private void Button_Stock_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Stock());

    }
    public void Button_DetailOrder_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Order_Details());
    }

    private void Button_Stockcomp_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Composant());

    }

}