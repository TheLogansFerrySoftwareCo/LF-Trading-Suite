﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPriceHistoryRepository.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.Core.
//   
//   TradingSuite.Core is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.Core is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.Core. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.Repositories
{
    using System.Collections.Generic;

    /// <summary>
    /// A read/write repository of price history data.
    /// </summary>
    public interface IPriceHistoryRepository
    {
        /// <summary>
        /// Adds the new historic prices to the repository.
        /// </summary>
        /// <param name="historicPrices">The historic prices to add.</param>
        /// <returns>
        /// True, when the price histories are successfully added.
        /// </returns>
        bool AddNewHistoricPrices(ISet<HistoricPrice> historicPrices);
    }
}
