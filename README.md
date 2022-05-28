# City Weather

## Description

City Weather is a web API which provides HTTP endpoints for CRUD operation on cities. It can also retreive weather and country information on the cities stored in the internal data store.

## Project structure

The CityWeather project is structured into a solution with the following project files:
- *CityWeather* - the CityWeather project which contains the model definitions and a test HTTP endpoint written with .NET 6 minimal APIs, EFCore and an in memory database.
- CityWeather.Tests - all the test cases for the CityWeather project and the EFCore HTTP endpoint.
- CityWeather.Dappre - the main HTTP web API written with .NET 6 minimal APIs, the Dapper ORM and supported by an Sqlite database.
- CityWeather.Dapper.Tests - all the test cases for the CityWeather.Dapper project and the main HTTP web API.

## API Keys, Build & Run

City Weather uses two web APIs internally. One is the api.openweathermap.org, which must be called with an API key. You can generate it by visiting the https://openweathermap.org/ website and creating an account. Afterwards, add your API key to the .NET secret store by going to the /CityWeather project folder and initialising the .NET secrets store.
```sh
dotnet user-secrets init

dotnet user-secrets set "OpenWeatherMap:ApiKey" "<YOUR-API-KEY>"
```
You can list your user secret to make sure they are stored.
```sh
dotnet user-secrets list
```
When you're done and no longer need the API key you can clear it from the secrets store.
```sh
dotnet user-secrets remove "OpenWeatherMap:ApiKey"
```

The other web API used is restcountries.com, and it will not require any authentication token.

After configuring the API Keys, the project can be built from the CityWeather/CityWeather folder.

```sh
dotnet build
```

Then, run the CityWeather.Dapper web API from the CityWeather/CityWeather.Dapper folder, via:

```sh
dotnet run
```

## Features

Supports CRUD operations on the following endpoints:
- /city         - POST      - Add a city
- /city/{id}    - PUT       - Update a city
- /city/{id}    - DELETE    - Delete a city

Supports searching for city, country and weather information by city name on the endpoint:
- /searchByName/{name} - GET    - Searches for city by name

## Tests

Run the all test cases on the HTTP endpoints.

```sh 
dotnet test
```

## Issues / Deviations

1) The countries API at https://restcountries.eu/#api-endpoints-all is no longer active. Had to use the https://restcountries.com (see https://github.com/apilayer/restcountries/issues/253). Within the the restcountries.com endpoints there is a 'name' API endpoint, which shall be used in preference to the 'all' endpoint.
2) Cities have an established date, however a great deal of cities have been establised B.C. and .NET doesn't have a way of representing B.C. dates. e.g. Paris was established around 250 B.C. Due of this, CityWeather uses a string type instead of a DateTime or DateOnly, mostly because there is no need to do any operations with the date. The downside is that we won't be able to do any meaningful validation e.g. to reject cities with an established date set in the future. As a workaround, in the future we could use Jon Skeet's NodaTime library, which can represent dates between years -9998 and 9999. This I found to be a good enough compromise. 
3) Note that a country may have more than one official currency. The City Weather API will return all official currencies, separated by a comma.
4) The validation could be improved, we currently rely on the Sqlite DB to reject inputs based on the table constraints and return that error message in the HTTP response.

## License

This project is licensed under the GPL-3.0-or-later.

## Credits

Shout out to the some very nice people for creating amazing learning reasources on ASP.NET:

- [Scott Hanselman's blog posts](https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests)
- [Damian Edward - Minimal API playground](https://github.com/DamianEdwards/MinimalApiPlayground)
- [Nick Chapsas - How to unit test Minimal APIs in .NET 6 (and why it's hard)](https://www.youtube.com/watch?v=VuFQtyRmS0E&t=163s)
- [David Fowler - Minimal APIs at a glance](https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727)

This is by no means an exhaustive list, but these are the most helpful for the task at hand.

## References

I found the following blog posts, tutorials, videos and tools extremely useful and informative:
- [Tutorial: Create a minimal web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio)
- [Swagger](https://swagger.io/)
- [Postman](https://www.postman.com/)
- [Dapper](https://github.com/DapperLib/Dapper)
- [OpenWeatherMap](https://openweathermap.org/)
- [RestCountries](https://restcountries.com)
- [DB Browser for SQLite](https://sqlitebrowser.org/)