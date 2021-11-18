using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Exante.Net
{
    public class ExanteAuthenticationProvider : AuthenticationProvider
    {
        public ExanteApiCredentials ExanteCredentials { get; }

        private static readonly string[] tokenPermissions =
        {
            "symbols",
            "ohlc",
            "feed",
            "change",
            "crossrates",
            "orders",
            "summary",
            "accounts",
            "transactions",
        };
        
        public ExanteAuthenticationProvider(ExanteApiCredentials credentials)
            : base(credentials)
        {
            ExanteCredentials = credentials;
        }

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, 
            HttpMethod method, Dictionary<string, object> parameters, bool signed,
            HttpMethodParameterPosition parameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if (!signed)
                return new Dictionary<string, string>();
            
            if (ExanteCredentials.ApplicationId is null ||
                ExanteCredentials.ClientId is null ||
                ExanteCredentials.SharedKey is null)
                throw new ArgumentException("No valid API credentials");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ExanteCredentials.SharedKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var payload = new JwtPayload(ExanteCredentials.ClientId,
                null,
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, ExanteCredentials.ApplicationId),
                    new Claim(JwtRegisteredClaimNames.Iss, ExanteCredentials.ClientId),
                },
                new Dictionary<string, object> {{JwtRegisteredClaimNames.Aud, tokenPermissions}},
                null,
                DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow
            );
            var securityToken = new JwtSecurityToken(new JwtHeader(credentials), payload);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new Dictionary<string, string>
                   {
                       {"Authorization", $"Bearer {token}"}
                   };
        }
    }
}