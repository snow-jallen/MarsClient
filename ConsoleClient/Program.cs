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

await waitForGameToStartPlaying();

Console.WriteLine("Game on!  Start pressing arrow keys to turn right, turn left, go forward, or go backward.");
while (true)
{
    var key = Console.ReadKey();
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
    }
}



//################################################################################################################

async Task move(string direction)
{
    var response = await httpClient.GetAsync($"/game/moveperseverance?token={joinResponse.Token}&direction={direction}");
    if (response.IsSuccessStatusCode)
    {
        var moveResult = await response.Content.ReadFromJsonAsync<MoveResponse>();
        Console.WriteLine($"Current row: {moveResult.Row}; Current column: {moveResult.Column}; Target row: {joinResponse.TargetRow}; Target column: {joinResponse.TargetColumn}");
        Console.WriteLine(moveResult.Message);
        Console.CursorLeft = 0;
        Console.CursorTop -= 2;
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

public class StatusResult
{
    public string status { get; set; }
}

public class JoinResponse
{
    public string Token { get; set; }
    public int StartingRow { get; set; }
    public int StartingColumn { get; set; }
    public int TargetRow { get; set; }
    public int TargetColumn { get; set; }
    public Neighbor[] Neighbors { get; set; }
    public Lowresolutionmap[] LowResolutionMap { get; set; }
    public string Orientation { get; set; }
}

public class Neighbor
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int Difficulty { get; set; }
}

public class Lowresolutionmap
{
    public int LowerLeftRow { get; set; }
    public int LowerLeftColumn { get; set; }
    public int UpperRightRow { get; set; }
    public int UpperRightColumn { get; set; }
    public int AverageDifficulty { get; set; }
}
