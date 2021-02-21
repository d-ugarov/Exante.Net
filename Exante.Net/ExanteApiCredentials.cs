using CryptoExchange.Net.Authentication;
using System;

namespace Exante.Net
{
    public class ExanteApiCredentials : ApiCredentials
    {
        public string ClientId { get; set; }
        public string ApplicationId { get; set; }
        public string SharedKey { get; set; }

        public ExanteApiCredentials(string clientId, string applicationId, string sharedKey)
            : base(clientId, applicationId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("ClientId can't be empty");
            if (string.IsNullOrEmpty(applicationId))
                throw new ArgumentException("ApplicationId can't be empty");
            if (string.IsNullOrEmpty(sharedKey))
                throw new ArgumentException("SharedKey can't be empty");

            ClientId = clientId;
            ApplicationId = applicationId;
            SharedKey = sharedKey;
        }
    }
}