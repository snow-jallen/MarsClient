using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MarsClient;

public partial class MoveViewModel : ObservableObject
{
    private readonly INetService netService;

    [ObservableProperty]
    private MoveResponse result;

    public MoveViewModel(INetService netService)
    {
        this.netService = netService;
    }

    [RelayCommand]
    private async Task MoveRight()
    {
        Result = await netService.MoveAsync("Right");
    }


    [RelayCommand]
    private async Task MoveLeft()
    {
        Result = await netService.MoveAsync("Left");
    }


    [RelayCommand]
    private async Task MoveForward()
    {
        Result = await netService.MoveAsync("Forward");
    }


    [RelayCommand]
    private async Task MoveReverse()
    {
        Result = await netService.MoveAsync("Reverse");
    }
}