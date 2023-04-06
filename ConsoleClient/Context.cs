namespace ConsoleClient;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
    }

    public DbSet<GameScenario> GameScenarios { get; set; }
}

public class GameScenario
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string ServerAddress { get; set; }
    public int PerseveranceX { get; set; }
    public int PerseveranceY { get; set; }
    public int IngenuityX { get; set; }
    public int IngenuityY { get; set; }
}


