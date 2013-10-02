﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStockListCsvDownloader.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.Data.
//   
//   TradingSuite.Data is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.Data is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.Data. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.DataFetchers.StockList
{
    /// <summary>
    /// A utility that will download a CSV listings of stocks from the web.
    /// </summary>
    public interface IStockListCsvDownloader
    {
        /// <summary>
        /// Downloads the CSV data from the web.
        /// </summary>
        /// <returns>
        /// The downloaded CSV as a single string.
        /// </returns>
        string DowloadCsvData();
    }
}
