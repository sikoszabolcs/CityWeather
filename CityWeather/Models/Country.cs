//  Project:          C-DAS
//  Product:          TrainTracker
//  FileName:         Country.cs
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
//  <copyright file="Country.cs" company="Siemens Mobility">
//     Copyright (c) 2022 Siemens Mobility Limited.  All rights reserved.
//  </copyright>
//  -----------------------------------------------------------------------

/// <summary>
/// Country codes according to ISO 3166
/// </summary>
public class Country
{
    public string Alpha2Code { get; set; }
    public string Alpha3Code { get; set; }
}