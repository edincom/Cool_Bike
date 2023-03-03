namespace Nice_bike;

public partial class Users : ContentPage
{
	public Users()
	{
		InitializeComponent();
	}
    private void Button_SR_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new SR());

    }
    private void Button_PM_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new PM());

    }
    private void Button_Assembler_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Assembler());

    }
}