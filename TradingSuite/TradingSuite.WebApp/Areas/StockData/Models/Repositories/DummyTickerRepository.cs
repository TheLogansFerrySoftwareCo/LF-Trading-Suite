// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyTickerRepository.cs" company="The Logans Ferry Software Co.">
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
    /// A repository for dummy Ticker objects.
    /// </summary>
// ReSharper disable ClassNeverInstantiated.Global -- (This class is instantiated by StructureMap)
    public class DummyTickerRepository : ITickerRepository
// ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// A list of tickers managed by this repository.
        /// </summary>
        /// <remarks>
        /// This is implemented as a static collection in order to facilitate persistence across controller calls.
        /// </remarks>
        private static List<TickerUpdateViewModel.TickerErrorViewModel> tickers;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="DummyTickerRepository"/> class.
        /// </summary>
        public DummyTickerRepository()
        {
            InitializeRepository();
        }

        /// <summary>
        /// Gets an enumeration of all ticker symbol view models.
        /// </summary>
        /// <returns>
        /// An enumeration of all ticker symbol view models.
        /// </returns>
        public IEnumerable<TickerUpdateViewModel.TickerErrorViewModel> GetTickerErrorViewModels()
        {
            return tickers;
        }

        /// <summary>
        /// Updates the IsExcluded flag for the specified ticker.
        /// </summary>
        /// <param name="tickerSymol">The ticker.</param>
        /// <param name="isExcluded">The new flag value.</param>
        /// <returns>True, when the update is successfully completed.</returns>
        public bool UpdateIsExcludedFlag(string tickerSymol, bool isExcluded)
        {
            var ticker = tickers.Find(x => x.Ticker.Equals(tickerSymol));

            if (ticker != null)
            {
                ticker.IsExcluded = isExcluded;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Initializes the repository.
        /// </summary>
        private static void InitializeRepository()
        {
            if (tickers == null)
            {
                tickers = new List<TickerUpdateViewModel.TickerErrorViewModel>
                    {
                        new TickerUpdateViewModel.TickerErrorViewModel { Exchange = "NYSE", Ticker = "AAA" },
                        new TickerUpdateViewModel.TickerErrorViewModel { Exchange = "NYSE", Ticker = "ABC", ErrorMessage = "401 Error" },
                        new TickerUpdateViewModel.TickerErrorViewModel { Exchange = "NASDAQ", Ticker = "DDD" },
                        new TickerUpdateViewModel.TickerErrorViewModel { Exchange = "NASDAQ", Ticker = "EFG", ErrorMessage = "402 Error" },
                        new TickerUpdateViewModel.TickerErrorViewModel { Exchange = "NYSE", Ticker = "MMM" },
                        new TickerUpdateViewModel.TickerErrorViewModel
                            {
                                Exchange = "NASDAQ", Ticker = "XYZ", ErrorMessage = "Invalid Symbol", IsExcluded = true 
                            },
                        new TickerUpdateViewModel.TickerErrorViewModel { Exchange = "NYSE", Ticker = "YYY", ErrorMessage = "403 Error" },
                        new TickerUpdateViewModel.TickerErrorViewModel
                            {
                                Exchange = "NYSE", Ticker = "ZZZ", ErrorMessage = "Invalid Symbol", IsExcluded = true 
                            }
                    };
            }
        }
    }
}