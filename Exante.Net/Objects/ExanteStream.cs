using CryptoExchange.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Exante.Net.Objects
{
    internal class ExanteStream
    {
        public Guid Id { get; set; }
        public Uri Uri { get; set; } = null!;
        public Dictionary<string, object>? Parameters { get; set; }

        public Task StreamTask { get; set; } = null!;

        public IResponse Response { get; }
        public Stream ResponseStream { get; }
        
        public ExanteStream(IResponse response, Stream responseStream)
        {
            Response = response;
            ResponseStream = responseStream;
        }
    }
}