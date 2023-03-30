using Microsoft.Extensions.Logging.Abstractions;
using MySql.Data.MySqlClient;

namespace Nice_bike;

public partial class Stock : ContentPage
{
    public Stock()
    {
        InitializeComponent();
    }
    private void OnGetNumberClicked2(object sender, EventArgs e)
    {
        int number;
        if (int.TryParse(QuantityEntry.Text, out number))
        {
            resultLabel.Text = number.ToString();
        }
        else
        {
            resultLabel.Text = "Invalid input. Please enter a valid number.";
        }
    }


    private async void Button_Clicked2(object sender, EventArgs e)
    {
        MySqlConnection connection = new MySqlConnection("server=pat.infolab.ecam.be;port=63324;database=bike2;uid=user2;password=12345;");
        connection.Open();
        string insertQuery = "INSERT INTO components (component,quantity) VALUES (@Component, @Quantity)";
        MySqlCommand command = new MySqlCommand(insertQuery, connection);

        command.Parameters.AddWithValue("@Name", MyComp.Text);
        command.Parameters.AddWithValue("@Quantity", resultLabel.Text);

        command.ExecuteNonQuery();
    }

    private void UpdateComp()
    {
        string selected1_0 = NameOfComponent.SelectedItem.ToString();
        MyComp.Text = selected1_0;
    }

    private void Picker1_0_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateComp();
    }

    private void Picker1_1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateSize1();
    }

    private void UpdateSize1()
    {
        string selected1 = Size.SelectedItem.ToString();
        MySize.Text = selected1;
    }

    private void Picker1_2_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateColor1();
    }

    private void UpdateColor1()
    {
        string selected1 = Color.SelectedItem.ToString();
        MyColor.Text = selected1;
    }

    private void Picker1_3_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateModel();
    }

    private void UpdateModel()
    {
        string selected1 = Model.SelectedItem.ToString();
        MyModel.Text = selected1;
    }

    private void OnGetNumberClicked(object sender, EventArgs e)
    {
        int number;
        if (int.TryParse(QuantityEntry2.Text, out number))
        {
            resultLabel2.Text = number.ToString();
        }
        else
        {
            resultLabel2.Text = "Invalid input. Please enter a valid number.";
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var Panier = new Panier();
        Panier.bikes.Add(new Bike(MyModel.Text, MySize.Text, MyColor.Text, 200, Convert.ToInt32(resultLabel2.Text)));
        await Navigation.PushAsync(Panier);
    }
}