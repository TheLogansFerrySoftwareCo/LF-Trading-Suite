// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleCsvDataSource.cs" company="The Logans Ferry Software Co.">
//   Copyright 2012, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of DatabaseConsole.
//   
//   DatabaseConsole is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   DatabaseConsole is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   DatabaseConsole. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.DatabaseConsole
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;

    /// <summary>
    /// A data source for retrieving stock data from a CSV file downloaded from Google Finance.
    /// </summary>
    public class GoogleCsvDataSource
    {
        /// <summary>
        /// The column index of the date field in a Google CSV file.
        /// </summary>
        private const int GoogleCsvDateColumnIndex = 0;

        /// <summary>
        /// The column index of the Open Price in a Google CSV file.
        /// </summary>
        private const int GoogleCsvOpenPriceColumnIndex = 1;

        /// <summary>
        /// The column index of the High Price in a Google CSV file.
        /// </summary>
        private const int GoogleCsvHighPriceColumnIndex = 2;

        /// <summary>
        /// The column index of the Low Price in a Google CSV file.
        /// </summary>
        private const int GoogleCsvLowPriceColumnIndex = 3;

        /// <summary>
        /// The column index of the Close Price in a Google CSV file.
        /// </summary>
        private const int GoogleCsvClosePriceColumnIndex = 4;

        /// <summary>
        /// The column index of the Volume field in a Google CSV file.
        /// </summary>
        private const int GoogleCsvVolumeColumnIndex = 5;

        /// <summary>
        /// Retrieves stock history data from Google for the specified stock.
        /// </summary>
        /// <param name="stock">The stock for which data will be retrieved.</param>
        /// <param name="startDate">The start date of the data set.</param>
        /// <param name="endDate">The end date of the data set.</param>
        /// <returns>
        /// The stock history read from Google.
        /// </returns>
        public List<StockDaily> GetDataFromGoogle(Stock stock, DateTime startDate, DateTime endDate)
        {
            var history = new List<StockDaily>();

            // Read all of the lines from the CSV file.
            var csvLines = GetCsvData(stock, startDate, endDate).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Iterate through each line to populate the price history...
            // ...skip the first line which contains the column headers.
            for (var index = 1; index < csvLines.Length; index++)
            {
                var fields = csvLines[index].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // Construct the interval data from the fields of the CSV line.
                var priceData = new StockDaily
                    {
                        Exchange = stock.Exchange,
                        Ticker = stock.Ticker,
                        Date = DateTime.Parse(fields[GoogleCsvDateColumnIndex]),
                        OpenPrice = Convert.ToSingle(fields[GoogleCsvOpenPriceColumnIndex]),
                        HighPrice = Convert.ToSingle(fields[GoogleCsvHighPriceColumnIndex]),
                        LowPrice = Convert.ToSingle(fields[GoogleCsvLowPriceColumnIndex]),
                        ClosePrice = Convert.ToSingle(fields[GoogleCsvClosePriceColumnIndex]),
                        Volume =
                            Convert.ToInt64(fields[GoogleCsvVolumeColumnIndex]) <= int.MaxValue
                                ? Convert.ToInt32(fields[GoogleCsvVolumeColumnIndex])
                                : int.MaxValue
                    };

                // Google sometimes provides data for non-trading days.  Weed these out based on volume.
                if (priceData.Volume > 0)
                {
                    // Google also sometimes has missing data points...we'll filter those out too.
                    const float Epsilon = 0.000001f;
                    if (Math.Abs(priceData.OpenPrice - 0.0f) < Epsilon
                        || Math.Abs(priceData.ClosePrice - 0.0f) < Epsilon
                        || Math.Abs(priceData.HighPrice - 0.0f) < Epsilon
                        || Math.Abs(priceData.LowPrice - 0.0f) < Epsilon)
                    {
                        Console.WriteLine("WARNING:  Filtering out data from " + priceData.Date + " due to missing price values!");
                        Console.WriteLine();
                    }
                    else
                    {
                        history.Add(priceData);    
                    }
                }
            }

            // Sort the history into chronological order (Google CSV's are in reverse-chronological order).
            history.Sort(CompareDailyStockIntervals);

            return history;
        }

        /// <summary>
        /// Compare two stock days by date.
        /// </summary>
        /// <param name="day1">Day 1</param>
        /// <param name="day2">Day 2</param>
        /// <returns>
        /// 0 when days are equal; 1 when day 2 is greater; -1 when Day 1 is greater.
        /// </returns>
        /// <remarks>
        /// This method is intended to be used as a Comparison method for sorting lists.
        /// </remarks>
        private static int CompareDailyStockIntervals(StockDaily day1, StockDaily day2)
        {
            return day1.Date.CompareTo(day2.Date);
        }

        /// <summary>
        /// Gets CSV data from Google containing 5 years of historical data for the specified stock..
        /// </summary>
        /// <param name="stock">The stock for which data will be retrieved.</param>
        /// <param name="startDate">The start date of the data set.</param>
        /// <param name="endDate">The end date of the data set.</param>
        /// <returns>
        /// The contents of the retrieved CSV file.
        /// </returns>
        private static string GetCsvData(Stock stock, DateTime startDate, DateTime endDate)
        {
            // Construct a Google URL with the correct parameters.
            var url = "http://www.google.com/finance/historical?q=" + stock.Exchange + "%3A" + stock.Ticker + "&startdate= " + startDate.ToShortDateString() + "&enddate=" + endDate.ToShortDateString() + "&output=csv";
                
            StreamReader reader = null;
            
            try
            {
                // Retrieve and return the raw CSV data served by Google.
                var webClient = new WebClient();
                var stream = webClient.OpenRead(url);
                Debug.Assert(stream != null, "stream != null");
                reader = new StreamReader(stream);

                return reader.ReadToEnd();
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
