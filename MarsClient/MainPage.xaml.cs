using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

namespace MarsClient;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}


public class JoinResponse
{
    public string token { get; set; }
    public int startingRow { get; set; }
    public int startingColumn { get; set; }
    public int targetRow { get; set; }
    public int targetColumn { get; set; }
    public Neighbor[] neighbors { get; set; }
    public Lowresolutionmap[] lowResolutionMap { get; set; }
    public string orientation { get; set; }
}

public class Neighbor
{
    public int row { get; set; }
    public int column { get; set; }
    public int difficulty { get; set; }
}

public class Lowresolutionmap
{
    public int lowerLeftRow { get; set; }
    public int lowerLeftColumn { get; set; }
    public int upperRightRow { get; set; }
    public int upperRightColumn { get; set; }
    public int averageDifficulty { get; set; }
}


public partial class MainViewModel : ObservableObject
{
    public MainViewModel(INetService netService)
    {
        this.netService = netService;
    }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string playerName;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinGameCommand))]
    private string gameId;

    private readonly INetService netService;

    [RelayCommand(CanExecute = nameof(CanJoinGame))]
    private async Task JoinGame()
    {
        await netService.JoinGameAsync(GameId, PlayerName);
    }

    private bool CanJoinGame()
    {
        return !string.IsNullOrWhiteSpace(PlayerName) && !string.IsNullOrWhiteSpace(GameId);
    }
}

