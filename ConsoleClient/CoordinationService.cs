using Microsoft.Extensions.Logging;

namespace ConsoleClient;

internal class CoordinationService : IHostedService
{
    private readonly StartInfo startInfo;
    private readonly ILogger<CoordinationService> logger;
    private HttpClient httpClient;
    private JoinResponse? joinResponse;
    private int ingenuityX;
    private int ingenuityY;

    public CoordinationService(StartInfo startInfo, ILogger<CoordinationService> logger)
    {
        this.startInfo = startInfo;
        this.logger = logger;
        httpClient = new HttpClient()
        {
            BaseAddress = new Uri(startInfo.ServerAddress),
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        joinResponse = await joinGameAsync();

        ingenuityX = joinResponse.StartingX;
        ingenuityY = joinResponse.StartingY;

        await waitForGameToStartPlaying();

        Task.Run(flyIngenuity);
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
            var statusResult = await httpClient.GetFromJsonAsync<StatusResult>($"/game/status?token={joinResponse?.Token}");
            if (statusResult.status == "Playing")
            {
                break;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    void flyIngenuity()
    {
        //head toward taget
    }

    async Task moveIngenuity(int row, int col)
    {
        var response = await httpClient.GetAsync($"/game/moveingenuity?token={joinResponse.Token}&destinationRow={row}&destinationColumn={col}");
        if (response.IsSuccessStatusCode)
        {
            var moveResponse = await response.Content.ReadFromJsonAsync<IngenuityMoveResponse>();
            ingenuityX = moveResponse.X;
            ingenuityY = moveResponse.Y;

            //update your internal high-res map with moveResponse.Neighbors
        }
    }

    async Task movePerseverance(string direction)
    {
        var response = await httpClient.GetAsync($"/game/moveperseverance?token={joinResponse.Token}&direction={direction}");
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
