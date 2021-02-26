﻿using Exante.Net;
using System;

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
//                                         LogVerbosity = LogVerbosity.Debug,
//                                         LogWriters = new List<TextWriter> {Console.Out},
//                                     });
//
// var exanteDemoClient = new ExanteClient(new ExanteClientOptions(credentials, ExantePlatformType.Demo));

var exchanges = await exanteClient.GetExchangesAsync();

var symbols = await exanteClient.GetSymbolsByExchangeAsync("NASDAQ");


// Stream API

var exanteStreamClient = new ExanteStreamClient(clientId, applicationId, sharedKey);

var subscription = await exanteStreamClient.GetQuoteStreamAsync(new[] {"BTC.EXANTE"}, quote =>
{
    Console.WriteLine($"New quote: {quote}");
});