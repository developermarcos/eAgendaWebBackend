using eAgenda.Infra.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace eAgenda.Webapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
                CreateHostBuilder(args).Build().Run();
            ConfiguracaoLogseAgenda.ConfigurarEscritaLogs();

            Log.Logger.Information("Iniciando o servidor da alicação e-Agenda...");

            try
            {
            }catch(Exception ex)
            {
                Log.Logger.Fatal(ex, "O servidor da aplicação e-Agenda parou inesperadamente.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
