// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStockRepository.cs" company="The Logans Ferry Software Co.">
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
    /// A read/write repository of stock data.
    /// </summary>
    public interface IStockRepository : IReadOnlyStockRepository
    {
        /// <summary>
        /// Tries to add the provided stocks to the repository.  Only new stocks will be added, and existing stocks will be skipped.
        /// </summary>
        /// <param name="stocksToAdd">The list of stocks to add.</param>
        /// <returns>
        /// The number of stocks that were successfully added.
        /// </returns>
        int TryAddNewStocks(IEnumerable<Stock> stocksToAdd);

        /// <summary>
        /// Updates the is excluded flag for the specified stock.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        /// <param name="isExcluded">The new flag value.</param>
        /// <returns>True, when the update is successful.</returns>
        bool UpdateIsExcludedFlag(string ticker, bool isExcluded);
    }
}
