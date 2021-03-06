﻿using CryptoExchange.Net.Objects;
using Exante.Net.Enums;
using System;

namespace Exante.Net
{
    public class ExanteClientOptions : RestClientOptions
    {
        private const string liveApi = "https://api-live.exante.eu/";
        private const string demoApi = "https://api-demo.exante.eu/";

        public ExanteApiCredentials? ExanteApiCredentials { get; set; }
        
        public TimeSpan ReconnectStreamTimeout { get; set; } = TimeSpan.FromSeconds(30);

        public ExanteClientOptions(ExanteApiCredentials credentials, ExantePlatformType platformType = ExantePlatformType.Live)
            : base(platformType == ExantePlatformType.Live ? liveApi : demoApi)
        {
            ExanteApiCredentials = credentials;
        }
    }
}