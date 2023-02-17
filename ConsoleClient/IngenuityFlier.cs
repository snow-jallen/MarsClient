using Microsoft.Extensions.Logging;

namespace ConsoleClient;

internal class IngenuityFlier
{
    private readonly ILogger<IngenuityFlier> logger;
    private readonly GameState gameState;
    private readonly HttpClient httpClient;

    public IngenuityFlier(ILogger<IngenuityFlier> logger, GameState gameState, HttpClient httpClient)
    {
        this.logger = logger;
        this.gameState = gameState;
        this.httpClient = httpClient;
    }

    internal async Task StartFlyingAsync()
    {
        for (int i = 0; i < 200; i++)
        {
            await moveIngenuity(gameState.IngenuityX + 2, gameState.IngenuityY + 2);
        }
    }

    async Task moveIngenuity(int row, int col)
    {
        var response = await httpClient.GetAsync($"/game/moveingenuity?token={gameState.Token}&destinationRow={row}&destinationColumn={col}");
        if (response.IsSuccessStatusCode)
        {
            var moveResponse = await response.Content.ReadFromJsonAsync<IngenuityMoveResponse>();
            gameState.IngenuityX = moveResponse.X;
            gameState.IngenuityY = moveResponse.Y;

            //update your internal high-res map with moveResponse.Neighbors
        }
    }
}
