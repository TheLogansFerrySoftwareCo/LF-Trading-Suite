// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockListItemViewModel.cs" company="The Logans Ferry Software Co.">
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
    /// A view model for displaying information about a stock as a list item..
    /// </summary>
    public class StockListItemViewModel
    {
        /// <summary>
        /// Gets or sets the ticker.
        /// </summary>
        /// <value>
        /// The ticker.
        /// </value>
        public string Ticker { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the stock is excluded from price update operations.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this stock is excluded; otherwise, <c>false</c>.
        /// </value>
        public bool IsExcluded { get; set; }
    }
}