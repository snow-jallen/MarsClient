using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MarsClient;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel(INetService netService, INavService navService)
    {
        this.netService = netService;
        this.navService = navService;
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string playerName;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string gameId;

    private readonly INetService netService;
    private readonly INavService navService;

    [RelayCommand]
    private async Task Loaded()
    {
        //if (await netService.AlreadyJoinedActiveGame())
        //{
        //    await navService.GoToAsync("/Move");
        //}
    }

    [RelayCommand(CanExecute = nameof(CanJoinGame))]
    private async Task JoinGame()
    {
        await netService.JoinGameAsync(GameId, PlayerName);
        await navService.GoToAsync("/Waiting");
    }

    private bool CanJoinGame()
    {
        return !string.IsNullOrWhiteSpace(PlayerName) && !string.IsNullOrWhiteSpace(GameId);
    }
}

public interface INavService
{
    Task GoToAsync(string destination);
}

public class MauiNavService : INavService
{
    public async Task GoToAsync(string destination)
    {
        await Shell.Current.GoToAsync(destination);
    }
}
