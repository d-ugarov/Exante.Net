using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using JWT.Algorithms;
using JWT.Builder;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Exante.Net
{
    public class ExanteAuthenticationProvider : AuthenticationProvider
    {
        public ExanteApiCredentials ExanteCredentials;
        
        public ExanteAuthenticationProvider(ExanteApiCredentials credentials)
            : base(credentials)
        {
            ExanteCredentials = credentials;
        }

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, 
            HttpMethod method, Dictionary<string, object> parameters, bool signed,
            PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if (!signed)
                return new Dictionary<string, string>();
            
            if (ExanteCredentials.ApplicationId is null ||
                ExanteCredentials.ClientId is null ||
                ExanteCredentials.SharedKey is null)
                throw new ArgumentException("No valid API credentials");

            var token = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm())
                                        .WithSecret(ExanteCredentials.SharedKey)
                                        .AddClaim("sub", ExanteCredentials.ApplicationId)
                                        .AddClaim("iss", ExanteCredentials.ClientId)
                                        .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                                        .AddClaim("aud", new[]
                                                         {
                                                             "feed",
                                                             "symbols",
                                                             "ohlc",
                                                             "crossrates",
                                                         })
                                        .Encode();

            return new Dictionary<string, string>
                   {
                       {"Authorization", $"Bearer {token}"}
                   };
        }
    }
}