// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleCsvDataSource.cs" company="The Logans Ferry Software Co.">
//   Copyright 2012, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of LF-Trading-Suite.
//   
//   LF-Trading-Suite is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   LF-Trading-Suite is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   LF-Trading-Suite. If not, see http://www.gnu.org/licenses/.
// </license>
// <author>Aaron Morris</author>
// <last_updated>04/28/2012 2:55 PM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
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
        /// <param name="ticker">The ticker symbol.</param>
        /// <returns>
        /// The stock history read from Google.
        /// </returns>
        public List<PriceData> GetDataFromGoogle(string ticker)
        {
            var history = new List<PriceData>();

            // Read all of the lines from the CSV file.
            var csvLines = GetCsvData(ticker).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Iterate through each line to populate the price history...
            // ...skip the first line which contains the column headers.
            for (var index = 1; index < csvLines.Length; index++)
            {
                var fields = csvLines[index].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // Construct the interval data from the fields of the CSV line.
                var priceData = new PriceData
                    { 
                        IntervalOpenTime = DateTime.Parse(fields[GoogleCsvDateColumnIndex]),
                        Open = Convert.ToSingle(fields[GoogleCsvOpenPriceColumnIndex]),
                        High = Convert.ToSingle(fields[GoogleCsvHighPriceColumnIndex]),
                        Low = Convert.ToSingle(fields[GoogleCsvLowPriceColumnIndex]),
                        Close = Convert.ToSingle(fields[GoogleCsvClosePriceColumnIndex]),
                        Volume = Convert.ToInt64(fields[GoogleCsvVolumeColumnIndex])
                    };

                // Google sometimes provides data for non-trading days.  Weed these out based on volume.
                if (priceData.Volume > 0)
                {
                    // Google also sometimes has missing data points...we'll filter those out too and display a warning.
                    const float Epsilon = 0.000001f;
                    if (Math.Abs(priceData.Open - 0.0f) < Epsilon
                        || Math.Abs(priceData.Close - 0.0f) < Epsilon
                        || Math.Abs(priceData.High - 0.0f) < Epsilon
                        || Math.Abs(priceData.Low - 0.0f) < Epsilon)
                    {
                        Console.WriteLine("WARNING:  Filtering out data from " + priceData.IntervalOpenTime + " due to missing price values!");
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
        private static int CompareDailyStockIntervals(PriceData day1, PriceData day2)
        {
            return day1.IntervalOpenTime.CompareTo(day2.IntervalOpenTime);
        }

        /// <summary>
        /// Gets CSV data from Google containing 5 years of historical data for the specified stock..
        /// </summary>
        /// <param name="ticker">The ticker to retrieve.</param>
        /// <returns>
        /// The contents of the retrieved CSV file.
        /// </returns>
        private static string GetCsvData(string ticker)
        {
            // The start date should be 5 years ago (1826 days).
            var startDate = DateTime.Now.Subtract(TimeSpan.FromDays(1826)).ToShortDateString();
            
            // The end date is today (so we have the most current data).
            var endDate = DateTime.Now.ToShortDateString();

            // Construct a Google URL with the correct parameters.
            var url = "http://www.google.com/finance/historical?q=" + ticker + "&startdate= " + startDate + "&enddate=" + endDate + "&output=csv";
                
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
