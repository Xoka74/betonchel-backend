using Betonchel.Identity;

public class Program
{
    public static void Main(string[] args)
    {

        var host = CreateHostBuilder(args).Build();
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dataSeeder = services.GetRequiredService<DataSeeder>();
            dataSeeder.SeedDataAsync().Wait();
        }
        
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}