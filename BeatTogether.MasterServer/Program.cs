﻿using System.IO;
using BeatTogether.MasterServer.Kernel.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace BeatTogether.MasterServer
{
    public static class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                    configurationBuilder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true)
                        .AddJsonFile($"appsettings.{hostBuilderContext.HostingEnvironment.EnvironmentName}.json", true)
                        .AddEnvironmentVariables()
                )
                .UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom
                        .Configuration(hostBuilderContext.Configuration)
                )
                .ConfigureServices((hostBuilderContext, services) =>
                    MasterServerKernelBootstrapper.ConfigureServices(hostBuilderContext, services)
                );
    }
}
