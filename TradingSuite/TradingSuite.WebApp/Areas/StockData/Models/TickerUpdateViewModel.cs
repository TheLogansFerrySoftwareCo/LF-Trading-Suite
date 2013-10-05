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
    using System.Collections.Generic;

    /// <summary>
    /// A view model for displaying information about an update operation.
    /// </summary>
    public class TickerUpdateViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TickerUpdateViewModel"/> class.
        /// </summary>
        public TickerUpdateViewModel()
        {
            this.Errors = new List<TickerErrorViewModel>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether an update is in progress.
        /// </summary>
        /// <value>
        /// <c>true</c> if an update is in progress; otherwise, <c>false</c>.
        /// </value>
        public bool IsUpdateInProgress { get; set; }

        /// <summary>
        /// Gets a list of errors.
        /// </summary>
        public List<TickerErrorViewModel> Errors { get; private set; } 

        /// <summary>
        /// A view model for displaying error information about a ticker symbol.
        /// </summary>
        public class TickerErrorViewModel
        {
            /// <summary>
            /// Gets or sets the ticker symbol.
            /// </summary>
            public string Ticker { get; set; }

            /// <summary>
            /// Gets or sets the name of the exchange on which the ticker is traded.
            /// </summary>
            public string Exchange { get; set; }

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
}