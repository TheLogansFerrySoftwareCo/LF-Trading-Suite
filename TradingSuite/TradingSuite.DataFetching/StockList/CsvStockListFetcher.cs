// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvStockListFetcher.cs" company="The Logans Ferry Software Co.">
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A utility that will fetch listings of stocks from the web.
    /// </summary>
    public class CsvStockListFetcher : IStockListFetcher
    {
        /// <summary>
        /// The utility that will be used to download the raw CSV data from the web.
        /// </summary>
        private readonly IStockListCsvDownloader csvDownloader;

        /// <summary>
        /// The utility that will be used to parse the downloaded CSV data into stock objects.
        /// </summary>
        private readonly IStockListCsvParser csvParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStockListFetcher"/> class.
        /// </summary>
        /// <param name="csvDownloader">The CSV downloader.</param>
        /// <param name="csvParser">The CSV parser.</param>
        public CsvStockListFetcher(IStockListCsvDownloader csvDownloader, IStockListCsvParser csvParser)
        {
            if (csvDownloader == null)
            {
                throw new ArgumentNullException("csvDownloader");
            }

            if (csvParser == null)
            {
                throw new ArgumentNullException("csvParser");
            }

            this.csvDownloader = csvDownloader;
            this.csvParser = csvParser;
        }

        /// <summary>
        /// Fetches a listing of stocks from the web.
        /// </summary>
        /// <returns>
        /// A listing of stocks.
        /// </returns>
        public IList<Stock> FetchStockList()
        {
            return this.csvParser.ParseCsvData(this.csvDownloader.DowloadCsvData());
        }
    }
}
