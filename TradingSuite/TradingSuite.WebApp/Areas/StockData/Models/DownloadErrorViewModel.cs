// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TickerErrorViewModel.cs" company="The Logans Ferry Software Co.">
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

namespace LogansFerry.TradingSuite.WebApp.Areas.StockData.Models
{
    /// <summary>
    /// A view model for displaying information about a price history download error.
    /// </summary>
    public class DownloadErrorViewModel
    {
        /// <summary>
        /// Gets or sets the ticker symbol.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Gets or sets the error message describing the error that occurred during the ticker's last download.
        /// An empty string indicates that no error occurred during the last download.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ticker is excluded from price-update operations.
        /// </summary>
        public bool IsExcluded { get; set; }
    }
}