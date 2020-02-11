using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MatcherChief.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // TODO: break apart CreateDefaultBuilder since it has unnecessary configuration
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
