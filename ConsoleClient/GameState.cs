using Microsoft.Extensions.Logging;

namespace ConsoleClient;

internal class GameState
{
    private readonly ILogger<GameState> gameState;

    public GameState(ILogger<GameState> gameState)
    {
        this.gameState = gameState;
    }

    private JoinResponse? joinResponse;
    public JoinResponse? JoinResponse
    {
        get => joinResponse;
        internal set
        {
            ArgumentNullException.ThrowIfNull(nameof(JoinResponse));

            joinResponse = value;

            IngenuityX = joinResponse.StartingX;
            IngenuityY = joinResponse.StartingY;
            Token = joinResponse.Token;
        }
    }

    public int IngenuityY { get; internal set; }
    public int IngenuityX { get; internal set; }
    public string Token { get; internal set; }
}
