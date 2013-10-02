// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITickerRepository.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.WebApp.
//   
//   TradingSuite.WebApp is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.WebApp is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.WebApp. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.WebApp.Areas.StockData.Models.Repositories
{
    using System.Collections.Generic;

    using LogansFerry.TradingSuite.WebApp.Areas.StockData.Models;

    /// <summary>
    /// A repository for ticker symbol objects.
    /// </summary>
    public interface ITickerRepository
    {
        /// <summary>
        /// Gets an enumeration of view models representing tickers that failed to update.
        /// </summary>
        /// <returns>
        /// An enumeration of ticker error view models.
        /// </returns>
        IEnumerable<TickerUpdateViewModel.TickerErrorViewModel> GetTickerErrorViewModels();

        /// <summary>
        /// Updates the IsExcluded flag for the specified ticker.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        /// <param name="isExcluded">The new flag value.</param>
        /// <returns>True, when the update is successfully completed.</returns>
        bool UpdateIsExcludedFlag(string ticker, bool isExcluded);
    }
}