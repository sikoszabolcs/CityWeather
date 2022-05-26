# City Weather

## Description

City Weather is a web API which provides HTTP endpoints for CRUD operation on cities. It can also retreive weather and country information on the cities stored in the internal data store.

## Table of Contents (Optional)

- [Installation](#installation)
- [Usage](#usage)
- [Credits](#credits)
- [License](#license)

## Installation

What are the steps required to install your project? Provide a step-by-step description of how to get the development environment running.

## Usage

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


To add a screenshot, create an `assets/images` folder in your repository and upload your screenshot to it. Then, using the relative filepath, add it to your README using the following syntax:

    ```md
    ![alt text](assets/images/screenshot.png)
    ```

## Credits

Shout out to the some very nice people for creating amazing learning reasources on ASP.NET:

- [Scott Hanselman's blog posts](https://www.hanselman.com/blog/minimal-apis-in-net-6-but-where-are-the-unit-tests)
- [Damian Edward - Minimal API playground](https://github.com/DamianEdwards/MinimalApiPlayground)
- [Nick Chapsas - How to unit test Minimal APIs in .NET 6 (and why it's hard)](https://www.youtube.com/watch?v=VuFQtyRmS0E&t=163s)
- [David Fowler - Minimal APIs at a glance](https://gist.github.com/davidfowl/ff1addd02d239d2d26f4648a06158727)

This is by no means an exhaustive list, but these are the most helpful for the task at hand.

## License

This project is licensed under the GPL-3.0-or-later.

## Badges

[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg)](code_of_conduct.md) 

Badges aren't necessary, per se, but they demonstrate street cred. Badges let other developers know that you know what you're doing. Check out the badges hosted by [shields.io](https://shields.io/). You may not understand what they all represent now, but you will in time.

## Features

If your project has a lot of features, list them here.

## How to Contribute

If you created an application or package and would like other developers to contribute it, you can include guidelines for how to do so. The [Contributor Covenant](https://www.contributor-covenant.org/) is an industry standard, but you can always write your own if you'd prefer.

## Tests

Run the all test cases on the HTTP endpoints.

```sh 
dotnet test
```

## Issues

1) The countries API at https://restcountries.eu/#api-endpoints-all is no longer active. Had to use the https://restcountries.com (see https://github.com/apilayer/restcountries/issues/253)
2) Cities have an established date, however a great deal of cities have been establised B.C. and .NET doesn't have a way of representing B.C. dates. e.g. Paris was established around 250 B.C. As a workaround I had to use Jon Skeet's NodaTime library, which can represent dates between years -9998 and 9999. This I found to be a good enough compromise.
3) Note that a country may have more than one official currency. The City Weather API will return all official currencies, separated by a comma.

## References

I found the following blog posts, tutorials, videos and tools extremely useful and informative:
- [Tutorial: Create a minimal web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-6.0&tabs=visual-studio)
- [Swagger](https://swagger.io/)
- [Postman](https://www.postman.com/)