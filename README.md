# Broker App
Broker App gets rates data from an external service API and determines what would be the best revenue for a specified historical period.

## Getting Started

### Technologies
Project is created with:

- Visual Studio 2022
- .NET 6.0 / C#
- ASP.NET Core Web API
- Swagger
- Entity Framework Core
- SQL Server
- Xunit / Moq
- Exchange Rates API service

### Installation

1. Clone this repo locally
2. Open `Broker.sln` solution using Visual Studio
3. Set `Broker.Infrastructure` as Startup Project 
4. Open the Package Manager Console (Tools -> Nuget Package Manager) and run `update-database` command to init the DB
5. Create an account in [Exchange Rates API](https://apilayer.com/marketplace/exchangerates_data-api) service
6. Copy the **API Key** from your "Exchange Rates API" account
7. Go back to Visual Studio and paste the **API Key** into the `ApiKey` field in all `appsettings.json` files so that the app can connect to their API
	```json
	  "Integrations": {
		"ExchangeRatesApi": {
		  "ApiUrl": "https://api.apilayer.com/exchangerates_data/",
		  "ApiKey": "<YOUR API KEY GOES HERE>"
		}
	  }
	```

All done!

### Run
1. Set `Broker.Services.WebApi` as Startup Project 
2. Press F5 to run the Web API

## License
[MIT](https://choosealicense.com/licenses/mit/)