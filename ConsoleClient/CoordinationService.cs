﻿using Microsoft.Extensions.Logging;

namespace ConsoleClient;

internal class CoordinationService : IHostedService
{
    private readonly StartInfo startInfo;
    private readonly ILogger<CoordinationService> logger;
    private readonly GameState gameState;
    private readonly IngenuityFlier ingenuity;
    private HttpClient httpClient;
    private Task ingenuityTask;

    public CoordinationService(StartInfo startInfo, ILogger<CoordinationService> logger, GameState gameState, IngenuityFlier ingenuity)
    {
        this.startInfo = startInfo;
        this.logger = logger;
        this.gameState = gameState;
        this.ingenuity = ingenuity;
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
        ingenuityTask = Task.Run(ingenuity.StartFlyingAsync);

        logger.LogInformation("Now that ingenuity is flying, I can start moving the rover...");
    }

    private async Task<JoinResponse> joinGameAsync()
    {
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

    async Task movePerseverance(string direction)
    {
        var response = await httpClient.GetAsync($"/game/moveperseverance?token={gameState.Token}&direction={direction}");
        if (response.IsSuccessStatusCode)
        {
            var moveResult = await response.Content.ReadFromJsonAsync<MoveResponse>();

            if (moveResult.Message == "You made it to the target!")
            {
                Console.WriteLine();
                Console.WriteLine("** Congratulations!! **");
                Environment.Exit(0);
            }
        }
    }
}
