﻿using System.Security.Cryptography;
using BeatTogether.MasterServer.Data.Bootstrap;
using BeatTogether.MasterServer.Kernel.Abstractions;
using BeatTogether.MasterServer.Kernel.Abstractions.Providers;
using BeatTogether.MasterServer.Kernel.Abstractions.Security;
using BeatTogether.MasterServer.Kernel.Abstractions.Sessions;
using BeatTogether.MasterServer.Kernel.Configuration;
using BeatTogether.MasterServer.Kernel.Implementations;
using BeatTogether.MasterServer.Kernel.Implementations.MessageReceivers;
using BeatTogether.MasterServer.Kernel.Implementations.Providers;
using BeatTogether.MasterServer.Kernel.Implementations.Security;
using BeatTogether.MasterServer.Kernel.Implementations.Sessions;
using BeatTogether.MasterServer.Messaging.Bootstrap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Security;

namespace BeatTogether.MasterServer.Kernel.Bootstrap
{
    public static class MasterServerKernelBootstrapper
    {
        public static void ConfigureServices(HostBuilderContext hostBuilderContext, IServiceCollection services)
        {
            MasterServerMessagingBootstrapper.ConfigureServices(hostBuilderContext, services);
            MasterServerDataBootstrapper.ConfigureServices(hostBuilderContext, services);

            services.AddSingleton(
                hostBuilderContext
                    .Configuration
                    .GetSection("MasterServer")
                    .Get<MasterServerConfiguration>()
            );
            services.AddSingleton(
                hostBuilderContext
                    .Configuration
                    .GetSection("Messaging")
                    .Get<MessagingConfiguration>()
            );
            services.AddSingleton(
                hostBuilderContext
                    .Configuration
                    .GetSection("SessionLifetime")
                    .Get<SessionLifetimeConfiguration>()
            );

            services.AddTransient<SecureRandom>();
            services.AddTransient<RNGCryptoServiceProvider>();

            services.AddSingleton<ICookieProvider, CookieProvider>();
            services.AddSingleton<IRandomProvider, RandomProvider>();
            services.AddSingleton<ICertificateProvider, CertificateProvider>();
            services.AddSingleton<IServerCodeProvider, ServerCodeProvider>();

            services.AddSingleton<IDiffieHellmanService, DiffieHellmanService>();
            services.AddSingleton<ICertificateSigningService, CertificateSigningService>();

            services.AddSingleton<IMessageDispatcher, MessageDispatcher>();

            services.AddSingleton<HandshakeMessageReceiver>();
            services.AddSingleton<UserMessageReceiver>();

            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IMultipartMessageService, MultipartMessageService>();
            services.AddScoped<IHandshakeService, HandshakeService>();
            services.AddScoped<IUserService, UserService>();

            services.AddHostedService<SessionTickService>();
            services.AddHostedService<Implementations.MasterServer>();
        }
    }
}
