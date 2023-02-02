namespace MarsClient;

public partial class WaitingPage : ContentPage
{
    private readonly INetService netService;

    public WaitingPage(INetService netService)
    {
        InitializeComponent();
        this.netService = netService;
        Loaded += WaitingPage_Loaded;
    }

    private async void WaitingPage_Loaded(object sender, EventArgs e)
    {
        while (await netService.SeeIfGameHasStarted() == false)
        {
            await Task.Delay(1000);
        }
        await Shell.Current.GoToAsync("/Move");
    }
}