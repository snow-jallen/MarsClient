using System.Net.Http.Json;

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
int ingenuityRow = joinResponse.StartingX;
int ingenuityCol = joinResponse.StartingY;

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
            await moveHelicopter(ingenuityRow, ingenuityCol + 2);
            break;
        case ConsoleKey.D:
            await moveHelicopter(ingenuityRow + 2, ingenuityCol);
            break;
        case ConsoleKey.S:
            await moveHelicopter(ingenuityRow, ingenuityCol - 2);
            break;
        case ConsoleKey.A:
            await moveHelicopter(ingenuityRow - 2, ingenuityCol);
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
        ingenuityRow = moveResponse.X;
        ingenuityCol = moveResponse.Y;

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
        Console.WriteLine($"Current row: {moveResult.Row}; Current column: {moveResult.Column}; Target row: {joinResponse.TargetX}; Target column: {joinResponse.TargetY}");
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
            break;
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}

public class MoveResponse
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int BatteryLevel { get; set; }
    public Neighbor[] Neighbors { get; set; }
    public string Message { get; set; }
    public string Orientation { get; set; }
}

public class IngenuityMoveResponse
{
    public int X { get; set; }
    public int Y { get; set; }
    public int BatteryLevel { get; set; }
    public IEnumerable<Neighbor> Neighbors { get; set; }
    public string Message { get; set; }
}

public class StatusResult
{
    public string status { get; set; }
}

public class JoinResponse
{
    public string Token { get; set; }
    public int StartingX { get; set; }
    public int StartingY { get; set; }
    public int TargetX { get; set; }
    public int TargetY { get; set; }
    public Neighbor[] Neighbors { get; set; }
    public Lowresolutionmap[] LowResolutionMap { get; set; }
    public string Orientation { get; set; }
}

public class Neighbor
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Difficulty { get; set; }
}

public class Lowresolutionmap
{
    public int LowerLeftX { get; set; }
    public int LowerLeftY { get; set; }
    public int UpperRightX { get; set; }
    public int UpperRightY { get; set; }
    public int AverageDifficulty { get; set; }
}
