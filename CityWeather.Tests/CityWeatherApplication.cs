//  Project:          C-DAS
//  Product:          TrainTracker
//  FileName:         CityWeatherApplication.cs
// 
//  Description:  Refer to XML comments
// 
//  (c) Copyright 2022 Siemens Mobility Limited.
//  This software is protected by copyright, the design of any article recorded in the software is
//  protected by design right and the information contained in the software is confidential. This
//  software may not be copied, any design may not be reproduced and the information contained in the
//  software may not be used or disclosed except with the prior written permission of and in a manner
//  permitted by the proprietors Siemens Siemens Mobility Limited (c) 2022
// 
//      Copyright Holders:
//         Siemens Mobility Limited,
//         PO Box 79,
//         Chippenham,
//         Wiltshire,
//         SN15 1JD
//         ENGLAND
//         Tel : +44 1249 441441
//         Fax : +44 1249 652322
// 
//  -----------------------------------------------------------------------
//  <copyright file="CityWeatherApplication.cs" company="Siemens Mobility">
//     Copyright (c) 2022 Siemens Mobility Limited.  All rights reserved.
//  </copyright>
//  -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CityWeather.Tests;

class CityWeatherApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();
 
        builder.ConfigureServices(services => 
        {
            services.AddScoped(sp =>
            {
                // Replace SQLite with the in memory provider for tests
                return new DbContextOptionsBuilder<CityDb>()
                    .UseInMemoryDatabase("Tests", root)
                    .UseApplicationServiceProvider(sp)
                    .Options;
            });
        });
 
        return base.CreateHost(builder);
    }
}

internal class WeatherApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;

    public WeatherApplication(string environment = "Development")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp =>
            {
                // Replace SQLite with in-memory database for tests
                return new DbContextOptionsBuilder<CityDb>()
                    .UseInMemoryDatabase("Tests")
                    .UseApplicationServiceProvider(sp)
                    .Options;
            });
            services.AddSingleton<WeatherService>();
            services.AddSingleton<CountryInfoService>();
        });

        return base.CreateHost(builder);
    }
}