﻿using Microsoft.Extensions.Logging;

namespace ConsoleClient;

public class PerseveranceDriver
{
    private readonly ILogger<PerseveranceDriver> logger;
    private readonly GameState gameState;
    private readonly HttpClient httpClient;

    public PerseveranceDriver(ILogger<PerseveranceDriver> logger, GameState gameState, HttpClient httpClient)
    {
        this.logger = logger;
        this.gameState = gameState;
        this.httpClient = httpClient;
    }

    public async Task StartDrivingAsync()
    {
        while (true)
        {
            var direction = determineDirection(gameState.Orientation, gameState.Target);
            var response = await httpClient.GetAsync($"/game/moveperseverance?token={gameState.Token}&direction={direction}");
            if (response.IsSuccessStatusCode)
            {
                var moveResult = await response.Content.ReadFromJsonAsync<MoveResponse>();
                gameState.Orientation = moveResult.Orientation;
                gameState.PerseveranceBatteryLevel = moveResult.BatteryLevel;
                gameState.UpdateMap(moveResult.Neighbors);
                gameState.Perseverance = (moveResult.Row, moveResult.Column);
            }
        }
    }

    private string determineDirection(string orientation, (int x, int y) rover, (int TargetX, int TargetY) target)
    {
        if (target.TargetY > rover.y)
        {
            return orientation switch
            {
                "North" => "Forward",
                "East" => "Left",
                "West" => "Right",
                "South" => "Left"
            };
        }
        else
        {
            return orientation switch
            {
                "North" => "Forward",
                "East" => "Left",
                "West" => "Right",
                "South" => "Left"
            };
        }
    }
}