namespace Nice_bike;

public partial class MainPage : ContentPage
{
	

	public MainPage()
	{
		InitializeComponent();
	}

	
    private void Button_Open_Clicked(object sender, EventArgs e)
    {

        Navigation.PushAsync(new Users());
       

    }
}

//blablabla