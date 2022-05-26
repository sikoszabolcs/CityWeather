//  Project:          C-DAS
//  Product:          TrainTracker
//  FileName:         CityCountryWeather.cs
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
//  <copyright file="CityCountryWeather.cs" company="Siemens Mobility">
//     Copyright (c) 2022 Siemens Mobility Limited.  All rights reserved.
//  </copyright>
//  -----------------------------------------------------------------------

public class CityCountryWeather
{
    public City City { get; set; }
    public Country Country { get; set; }
    public WeatherRoot Weather { get; set; }
}