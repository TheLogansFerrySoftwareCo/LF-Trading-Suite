// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStockListCsvParser.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.DataFetchers.
//   
//   TradingSuite.DataFetchers is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.DataFetchers is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.DataFetchers. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.DataFetchers.StockList
{
    using System.Collections.Generic;

    /// <summary>
    /// A utility that will parse CSV data for a listing of stocks.
    /// </summary>
    public interface IStockListCsvParser
    {
        /// <summary>
        /// Parses the CSV data for a listing of stocks.
        /// </summary>
        /// <param name="csvData">The CSV data to parse.</param>
        /// <returns>
        /// A listing of stocks parsed from the CSV data.
        /// </returns>
        IList<Stock> ParseCsvData(string csvData);
    }
}
