using CommandLine;
using Microsoft.Extensions.Logging;

if (args.Any(a => a == "auto"))
{
    await Host.CreateDefaultBuilder(args)
        .ConfigureLogging(config =>
        {
            config.AddFilter("Microsoft", LogLevel.Warning);
        })
        .ConfigureServices((_, services) =>
        {
            services.AddDbContext<Context>(options => options.UseSqlite("data source=database.db"));
            services.AddHostedService<CoordinationService>();
            services.AddSingleton<GameState>();
            services.AddSingleton<IngenuityFlyer>();
            services.AddSingleton<PerseveranceDriver>();
            services.AddSingleton((_) =>
            {
                var result = Parser.Default.ParseArguments<StartInfo>(args);
                return result.Value ?? throw new Exception("Invalid arguments");
            });
            services.AddSingleton((s) => new HttpClient { BaseAddress = new Uri(s.GetRequiredService<StartInfo>().ServerAddress) });
        })
        .RunConsoleAsync();
    return;
}

Console.WriteLine("Welcome to the Mars Rover Console Client");
Console.WriteLine("What server are you connecting to?");
var serverAddress = Console.ReadLine();

Console.WriteLine("What is your player name?");
var name = Console.ReadLine();

Console.WriteLine("What Game ID do you want to join? (e.g. a, b, c, d, etc.)");
var gameId = Console.ReadLine();

Console.WriteLine($"You'll be joining as {name} on game {gameId} at {serverAddress}");
Console.WriteLine("Press [Enter] when you're ready to join.  Press Ctrl+C to start over.");
Console.ReadLine();

var httpClient = new HttpClient { BaseAddress = new Uri(serverAddress) };

var r = await httpClient.GetAsync($"/game/join?gameId={gameId}&name={name}");
if (!r.IsSuccessStatusCode)
{
    Console.WriteLine("Unfortunately there was a problem joining that game. :(");
    Console.WriteLine("Error details:");
    Console.WriteLine(await r.Content.ReadAsStringAsync());
}
var joinResponse = await r.Content.ReadFromJsonAsync<JoinResponse>();

//hang on to these for later
int ingenuityX = joinResponse.StartingX;
int ingenuityY = joinResponse.StartingY;

await waitForGameToStartPlaying();

Console.WriteLine("Game on!  Start pressing arrow keys to turn right, turn left, go forward, or go backward.");
while (true)
{
    var key = Console.ReadKey(intercept: true);
    switch (key.Key)
    {
        case ConsoleKey.UpArrow:
            await move("Forward");
            break;
        case ConsoleKey.RightArrow:
            await move("Right");
            break;
        case ConsoleKey.LeftArrow:
            await move("Left");
            break;
        case ConsoleKey.DownArrow:
            await move("Reverse");
            break;
        case ConsoleKey.W:
            await moveHelicopter(ingenuityX, ingenuityY + 2);
            break;
        case ConsoleKey.D:
            await moveHelicopter(ingenuityX + 2, ingenuityY);
            break;
        case ConsoleKey.S:
            await moveHelicopter(ingenuityX, ingenuityY - 2);
            break;
        case ConsoleKey.A:
            await moveHelicopter(ingenuityX - 2, ingenuityY);
            break;
    }
}

//################################################################################################################

async Task moveHelicopter(int row, int col)
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

async Task move(string direction)
{
    var response = await httpClient.GetAsync($"/game/moveperseverance?token={joinResponse.Token}&direction={direction}");
    if (response.IsSuccessStatusCode)
    {
        var moveResult = await response.Content.ReadFromJsonAsync<MoveResponse>();

        //clear the output area
        var blankLine = new string(' ', Console.WindowWidth);
        Console.WriteLine(blankLine);
        Console.WriteLine(blankLine);
        Console.WriteLine(blankLine);
        Console.CursorTop -= 3;

        //update output
        Console.WriteLine($"Current row: {moveResult.X}; Current column: {moveResult.Y}; Targets: {string.Join("; ", joinResponse.Targets.Select(t => $"({t.X},{t.Y})"))}");
        Console.WriteLine($"Battery level: {moveResult.BatteryLevel}; Orientation: {moveResult.Orientation}");
        Console.WriteLine(moveResult.Message);

        if (moveResult.Message == "You made it to the target!")
        {
            Console.WriteLine();
            Console.WriteLine("** Congratulations!! **");
            Environment.Exit(0);
        }

        Console.CursorLeft = 0;
        Console.CursorTop -= 3;
    }
}

async Task waitForGameToStartPlaying()
{
    while (true)
    {
        var statusResult = await httpClient.GetFromJsonAsync<StatusResult>($"/game/status?token={joinResponse.Token}");
        if (statusResult.status == "Playing")
        {
            break;
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
