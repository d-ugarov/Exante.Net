# ![Icon](https://github.com/d-ugarov/Exante.Net/blob/master/Exante.Net/Icon/icon.png?raw=true) Exante.Net

![Build status](https://github.com/d-ugarov/Exante.Net/actions/workflows/dotnet.yml/badge.svg)

A .NET wrapper for the Exante API as described on [Exante](https://api-live.exante.eu/api-docs/), including all features the API provides using clear and readable objects.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/d-ugarov/Exante.Net/issues)**


## Installation
![Nuget version](https://img.shields.io/nuget/v/exante.net.svg)  ![Nuget downloads](https://img.shields.io/nuget/dt/exante.net.svg)

Available on [Nuget](https://www.nuget.org/packages/Exante.Net/).
```
pm> Install-Package Exante.Net
```
To get started with Exante.Net first you will need to get the library itself. The easiest way to do this is to install the package into your project using  [NuGet](https://www.nuget.org/packages/Exante.Net/).


## Getting started
To get started we have to add the Exante.Net namespace: `using Exante.Net;`.

Exante.Net provides two clients to interact with the Exante API. The `ExanteClient` provides all rest API calls. The `ExanteStreamClient` provides functions to interact with the stream provided by the Exante API. Both clients are disposable and as such can be used in a `using` statement.


## Examples
Examples can be found in the [examples folder](https://github.com/d-ugarov/Exante.Net/blob/master/Exante.Net.Examples/Program.cs).

```C#
// Rest API

var exanteClient = new ExanteClient(clientId, applicationId, sharedKey);

var symbols = await exanteClient.GetSymbolsByExchangeAsync("NASDAQ");


// Stream API

var exanteStreamClient = new ExanteStreamClient(clientId, applicationId, sharedKey);

var subscription = await exanteStreamClient.GetQuoteStreamAsync(new[] {"BTC.EXANTE", "ETH.EXANTE"}, x =>
{
    Console.WriteLine($"{x.Date} " +
                      $"{x.SymbolId}: " +
                      $"{string.Join(", ", x.Bid.Select(b => b.Price))} (bid) / " +
                      $"{string.Join(", ", x.Ask.Select(b => b.Price))} (ask)");
});
```


## CryptoExchange.Net
Implementation is build upon the CryptoExchange.Net library, make sure to also check out the documentation on that: [docs](https://github.com/JKorf/CryptoExchange.Net)

JKorf CryptoExchange.Net implementations:
<table>
    <tr>
        <td>
            <a href="https://github.com/JKorf/Binance.Net"><img src="https://github.com/JKorf/Binance.Net/blob/master/Binance.Net/Icon/icon.png?raw=true"/></a>
            <br />
            <a href="https://github.com/JKorf/Binance.Net">Binance</a>
        </td>
        <td>
            <a href="https://github.com/JKorf/Bitfinex.Net"><img src="https://github.com/JKorf/Bitfinex.Net/blob/master/Bitfinex.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/JKorf/Bitfinex.Net">Bitfinex</a>
        </td>
        <td>
            <a href="https://github.com/JKorf/Bittrex.Net"><img src="https://github.com/JKorf/Bittrex.Net/blob/master/Bittrex.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/JKorf/Bittrex.Net">Bittrex</a>
        </td>
        <td>
            <a href="https://github.com/JKorf/CoinEx.Net"><img src="https://github.com/JKorf/CoinEx.Net/blob/master/CoinEx.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/JKorf/CoinEx.Net">CoinEx</a>
        </td>
        <td>
            <a href="https://github.com/JKorf/Huobi.Net"><img src="https://github.com/JKorf/Huobi.Net/blob/master/Huobi.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/JKorf/Huobi.Net">Huobi</a>
        </td>
        <td>
            <a href="https://github.com/JKorf/Kraken.Net"><img src="https://github.com/JKorf/Kraken.Net/blob/master/Kraken.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/JKorf/Kraken.Net">Kraken</a>
        </td>
        <td>
            <a href="https://github.com/JKorf/Kucoin.Net"><img src="https://github.com/JKorf/Kucoin.Net/blob/master/Kucoin.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/JKorf/Kucoin.Net">Kucoin</a>
        </td>
    </tr>
</table>

Implementations from third parties:
<table>
    <tr>
        <td>
            <a href="https://github.com/ridicoulous/Bitmex.Net"><img src="https://github.com/ridicoulous/Bitmex.Net/blob/master/Bitmex.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/ridicoulous/Bitmex.Net">Bitmex</a>
        </td>
        <td>
            <a href="https://github.com/intelligences/HitBTC.Net"><img src="https://github.com/intelligences/HitBTC.Net/blob/master/src/HitBTC.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/intelligences/HitBTC.Net">HitBTC</a>
        </td>
        <td>
            <a href="https://github.com/ridicoulous/LiquidQuoine.Net"><img src="https://github.com/ridicoulous/LiquidQuoine.Net/blob/master/Resources/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/ridicoulous/LiquidQuoine.Net">Liquid</a>
        </td>
        <td>
            <a href="https://github.com/EricGarnier/LiveCoin.Net"><img src="https://github.com/EricGarnier/LiveCoin.Net/blob/master/LiveCoin.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/EricGarnier/LiveCoin.Net">LiveCoin</a>
        </td>
        <td>
            <a href="https://github.com/Zaliro/Switcheo.Net"><img src="https://github.com/Zaliro/Switcheo.Net/blob/master/Resources/switcheo-coin.png?raw=true" /></a>
            <br />
            <a href="https://github.com/Zaliro/Switcheo.Net">Switcheo</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/OKEx.Net"><img src="https://github.com/burakoner/OKEx.Net/blob/master/Okex.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/OKEx.Net">OKEx</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/Chiliz.Net"><img src="https://github.com/burakoner/Chiliz.Net/blob/master/Chiliz.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/Chiliz.Net">Chiliz</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/BitMax.Net"><img src="https://github.com/burakoner/BitMax.Net/blob/master/BitMax.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/BitMax.Net">BitMax</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/BtcTurk.Net"><img src="https://github.com/burakoner/BtcTurk.Net/blob/master/BtcTurk.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/BtcTurk.Net">BtcTurk</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/Paribu.Net"><img src="https://github.com/burakoner/Paribu.Net/blob/master/Paribu.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/Paribu.Net">Paribu</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/Thodex.Net"><img src="https://github.com/burakoner/Thodex.Net/blob/master/Thodex.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/Thodex.Net">Thodex</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/Coinzo.Net"><img src="https://github.com/burakoner/Coinzo.Net/blob/master/Coinzo.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/Coinzo.Net">Coinzo</a>
        </td>
        <td>
            <a href="https://github.com/burakoner/Tatum.Net"><img src="https://github.com/burakoner/Tatum.Net/blob/master/Tatum.Net/Icon/icon.png?raw=true" /></a>
            <br />
            <a href="https://github.com/burakoner/Tatum.Net">Tatum</a>
        </td>
    </tr>
</table>
