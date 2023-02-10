using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MarsClient.Models;

namespace MarsClient;

public partial class MoveViewModel : ObservableObject
{
    public MoveViewModel(INetService netService, MapService mapService)
    {
        this.netService = netService;
        this.mapService = mapService;
    }

    private readonly INetService netService;
    private MapService mapService;

    [ObservableProperty]
    private MoveResponse result;

    [ObservableProperty]
    private ViewableCells viewableCells;

    [RelayCommand]
    private async Task Move(string direction)
    {
        Result = await netService.MoveAsync(direction);
        var statedOrientation = (int)Enum.Parse<Orientation>(result.orientation);
        statedOrientation = (statedOrientation + 5) % 4;
        ViewableCells = mapService.Map.GetCellsInView((Result.row, Result.column), (Orientation)statedOrientation);
    }
}
