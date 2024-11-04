namespace Logic.Infrastructure;

public class Config(bool isDev)
{
    public string DatabaseConnectionString { get; } = isDev
        ? "Server=localhost;Database=ArtsofteTestAutumn2024;Port=5432;User Id=postgres;Password=1"
        : Environment.GetEnvironmentVariable("Connection");

    public string JwtIssuer { get; } = "ArtApp";

    public string JwtAudience { get; } = "ArtUsers";

    public string JwtKey { get; } = "dKt3Y#9^3nTv%2GpB&y8U@C*#w!WqS6D";
}