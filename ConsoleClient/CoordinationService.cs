using Microsoft.Extensions.Logging;

namespace ConsoleClient;

internal class CoordinationService : IHostedService
{
    private readonly StartInfo startInfo;
    private readonly ILogger<CoordinationService> logger;
    private readonly GameState gameState;
    private readonly IngenuityFlyer ingenuity;
    private readonly PerseveranceDriver perseverance;
    private HttpClient httpClient;

    public CoordinationService(StartInfo startInfo, ILogger<CoordinationService> logger, GameState gameState, IngenuityFlyer ingenuity, PerseveranceDriver perseverance)
    {
        this.startInfo = startInfo;
        this.logger = logger;
        this.gameState = gameState;
        this.ingenuity = ingenuity;
        this.perseverance = perseverance;
        httpClient = new HttpClient()
        {
            BaseAddress = new Uri(startInfo.ServerAddress),
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        gameState.JoinResponse = await joinGameAsync();

        logger.LogInformation("Waiting for game to start...");
        await waitForGameToStartPlaying();

        logger.LogInformation("Game started.  Beginning to fly ingenuity.");
        //_ = Task.Run(ingenuity.StartFlyingAsync);
        _ = Task.Run(perseverance.StartDrivingAsync);

        logger.LogInformation("Now that ingenuity is flying, I can start moving the rover...");
    }

    private async Task<JoinResponse> joinGameAsync()
    {
        startInfo.Name += DateTime.Now.ToString("mm.ss");
        logger.LogInformation("Joining game {gameId} as {name}", startInfo.GameId, startInfo.Name);
        var response = await httpClient.GetAsync($"/game/join?gameid={startInfo.GameId}&name={startInfo.Name}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<JoinResponse>();
        }

        logger.LogError("Unable to join game. {httpResponse}", response);
        throw new Exception("Unable to join game.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    async Task waitForGameToStartPlaying()
    {
        while (true)
        {
            var statusResult = await httpClient.GetFromJsonAsync<StatusResult>($"/game/status?token={gameState.Token}");
            if (statusResult.status == "Playing")
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
