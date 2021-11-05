using Exante.Net;
using System;
using System.Linq;

const string clientId = "";
const string applicationId = "";
const string sharedKey = "";


// Rest API

var exanteClient = new ExanteClient(clientId, applicationId, sharedKey);

// var credentials = new ExanteApiCredentials(clientId, applicationId, sharedKey);
//
// var exanteClient = new ExanteClient(credentials);
//
// var exanteClient = new ExanteClient(new ExanteClientOptions(credentials)
//                                     {
//                                         LogLevel = LogLevel.Debug,
//                                         LogWriters = new List<ILogger> {new DebugLogger()},
//                                     });
//
// var exanteDemoClient = new ExanteClient(new ExanteClientOptions(credentials, ExantePlatformType.Demo));

var exchanges = await exanteClient.GetExchangesAsync();

var symbols = await exanteClient.GetSymbolsByExchangeAsync("NASDAQ");


// Stream API

var exanteStreamClient = new ExanteStreamClient(clientId, applicationId, sharedKey);

var subscription = await exanteStreamClient.GetFeedQuoteStreamAsync(new[] {"BTC.EXANTE", "ETH.EXANTE"}, x =>
{
    Console.WriteLine($"{x.Date} " +
                      $"{x.SymbolId}: " +
                      $"{string.Join(", ", x.Bid.Select(b => b.Price))} (bid) / " +
                      $"{string.Join(", ", x.Ask.Select(b => b.Price))} (ask)");
});