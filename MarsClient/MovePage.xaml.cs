namespace MarsClient;

public partial class MovePage : ContentPage
{
    public MovePage(MoveViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
