using CommunityToolkit.Mvvm.Input;

namespace MarsClient;

public partial class MovePage : ContentPage
{
	public MovePage(MoveViewModel vm)
	{
		InitializeComponent();
		BindingContext= vm;
	}
}

public partial class MoveViewModel : BindableObject
{
    private readonly INetService netService;

    public MoveViewModel(INetService netService)
	{
        this.netService = netService;
    }

	[RelayCommand]
	private async Task MoveRight()
	{
		await netService.MoveAsync("Right");
	}


    [RelayCommand]
    private async Task MoveLeft()
    {
        await netService.MoveAsync("Left");
    }


    [RelayCommand]
    private async Task MoveForward()
    {
        await netService.MoveAsync("Forward");
    }


    [RelayCommand]
    private async Task MoveReverse()
    {
        await netService.MoveAsync("Reverse");
    }
}