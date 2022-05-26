//  Project:          C-DAS
//  Product:          TrainTracker
//  FileName:         City.cs
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
//  <copyright file="City.cs" company="Siemens Mobility">
//     Copyright (c) 2022 Siemens Mobility Limited.  All rights reserved.
//  </copyright>
//  -----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityWeather.Models.City;

public enum TouristRating
{
    Bad = 1,
    Meh = 2,
    Good = 3,
    VeryGood = 4,
    Amazing = 5
}

public class City
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public TouristRating Rating { get; set; }
    public DateTime EstablishedDate { get; set; }
    public UInt32 EstimatedPopulation { get; set; }
}