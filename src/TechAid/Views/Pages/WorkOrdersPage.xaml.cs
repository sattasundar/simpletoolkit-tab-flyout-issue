namespace TechAid.Views.Pages;

public partial class WorkOrdersPage : ContentPage
{
	public WorkOrdersPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		Shell.Current.FlyoutIsPresented = true;
    }
}