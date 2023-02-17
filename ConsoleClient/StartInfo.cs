using CommandLine;

namespace ConsoleClient;

public class StartInfo
{
    [Option('s', "serverAddress", Default = "https://snow-rover.azurewebsites.net", HelpText = "Address of the game server")]
    public string ServerAddress { get; set; }

    [Option('g', "gameId", HelpText = "ID of the game you want to join", Required = true)]
    public string GameId { get; set; }

    [Option('n', "name", HelpText = "Your player name", Required = true)]
    public string Name { get; set; }
}
