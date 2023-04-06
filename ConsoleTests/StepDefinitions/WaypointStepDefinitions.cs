using ConsoleClient;
using Microsoft.Extensions.Logging;

namespace ConsoleTests.StepDefinitions;

[Binding]
public sealed class WaypointStepDefinitions
{
    private readonly ScenarioContext context;

    public WaypointStepDefinitions(ScenarioContext context)
    {
        this.context = context;
    }

    [Given(@"the following map where every cell is (.*) cells")]
    public void GivenTheFollowingMapWhereEveryCellIsCells(int cellSize, Table table)
    {
        int targetX = 0;
        int targetY = 0;
        int startX = 0;
        int startY = 0;

        for (int row = 0; row < table.RowCount; row++)
        {
            var y = table.RowCount - 1 - row;
            for (int x = 0; x < table.Rows[row].Count; x++)
            {
                if (table.Rows[row][x] == "T")
                {
                    targetX = x * cellSize;
                    targetY = y * cellSize;
                }

                if (table.Rows[row][x] == "S")
                {
                    startX = x * cellSize;
                    startY = y * cellSize;
                }
            }
        }

        var gameState = new GameState(new LoggerFactory().CreateLogger<GameState>());
        gameState.JoinResponse = new JoinResponse
        {
            StartingX = startX,
            StartingY = startY,
            TargetX = targetX,
            TargetY = targetY,
            LowResolutionMap = new[]
            {
                new Lowresolutionmap
                {
                    AverageDifficulty=1,
                    LowerLeftX=0,
                    LowerLeftY = 0,
                    UpperRightX = table.Rows[0].Count*cellSize,
                    UpperRightY = table.RowCount*cellSize
                }
            }
        };

        context.Set(gameState);
    }

    [When(@"I generate the waypoints")]
    public void WhenIGenerateTheWaypoints()
    {
        var gameState = context.Get<GameState>();
        var logger = new LoggerFactory().CreateLogger<IngenuityFlyer>();
        var httpClient = new HttpClient();
        var ingenuityFlyer = new IngenuityFlyer(logger, gameState, httpClient);
        var waypoints = ingenuityFlyer.BuildWaypointList(gameState);

        context.Set(waypoints);
    }

    [Then(@"the waypoints should be as follows")]
    public void ThenTheWaypointsShouldBeAsFollows(Table table)
    {
        var actualWaypoints = context.Get<Queue<(int x, int y)>>();
        var waypoints = new Dictionary<int, (int x, int y)>();

        for (int row = 0; row < table.RowCount; row++)
        {
            var y = table.RowCount - 1 - row;
            for (int x = 0; x < table.Rows[row].Count; x++)
            {
                if (int.TryParse(table.Rows[row][x], out int waypointNumber))
                {
                    waypoints.Add(waypointNumber, (x * 10, y * 10));
                }
            }
        }

        var expectedWaypoints = new Queue<(int x, int y)>(from p in waypoints
                                                          orderby p.Key
                                                          select p.Value);
        actualWaypoints.Should().BeEquivalentTo(expectedWaypoints);
    }

}