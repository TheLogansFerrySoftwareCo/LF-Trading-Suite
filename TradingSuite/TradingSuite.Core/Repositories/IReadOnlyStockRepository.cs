// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyStockRepository.cs" company="The Logans Ferry Software Co.">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A read-only repository of stock data.
    /// </summary>
    public interface IReadOnlyStockRepository
    {
        /// <summary>
        /// Returns queryable collection of all stocks from the repository.
        /// </summary>
        /// <returns>
        /// A queryable collection of all stock from the repository.
        /// </returns>
        IQueryable<Stock> All();

        /// <summary>
        /// Gets a stock by the specified ticker.
        /// </summary>
        /// <param name="ticker">The ticker to use as a selection criteria.</param>
        /// <returns>
        /// The specified stock when it exists in the repository; otherwise, NULL. 
        /// </returns>
        Stock GetByTicker(string ticker);
    }
}
