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


    private async void Window_Activated(object sender, EventArgs e)
    {
#if WINDOWS
        const int DefaultWidth = 600;
        const int DefaultHeight = 600;

        var window = sender as Window;

        // change window size.
        window.Width = DefaultWidth;
        window.Height = DefaultHeight;

        // give it some time to complete window resizing task.
        await window.Dispatcher.DispatchAsync(() => { });

        var disp = DeviceDisplay.Current.MainDisplayInfo;

        // move to screen center
        window.X = (disp.Width / disp.Density - window.Width) / 2;
        window.Y = (disp.Height / disp.Density - window.Height) / 2;
#endif
    }

}