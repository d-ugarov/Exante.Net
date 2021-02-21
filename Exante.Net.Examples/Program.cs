using Exante.Net;

const string clientId = "";
const string applicationId = "";
const string sharedKey = "";

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

var instruments = await exanteClient.GetExchangeInstrumentsAsync("NASDAQ");