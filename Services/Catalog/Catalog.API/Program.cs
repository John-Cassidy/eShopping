namespace Catalog.API;

public class Program {
    public static void Main(string[] args) {
        CreateHostBuilder(args).Build().Run(); // <--- This is the line that changed

    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
                webBuilder.UseStartup<Startup>();
            }); // <--- This is the line that changed
}