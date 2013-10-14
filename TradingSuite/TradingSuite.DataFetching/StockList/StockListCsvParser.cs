// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockListCsvParser.cs" company="The Logans Ferry Software Co.">
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A utility that will parse CSV data for a listing of stocks.
    /// </summary>
    public class StockListCsvParser : IStockListCsvParser
    {
        /// <summary>
        /// The column name that will be used to identify the TICKER column.
        /// </summary>
        private readonly string tickerColumnName;

        /// <summary>
        /// The column name that will be used to identify the COMANY NAME column.
        /// </summary>
        private readonly string companyNameColumnName;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockListCsvParser"/> class.
        /// </summary>
        /// <param name="tickerColName">Name of the ticker column.</param>
        /// <param name="companyNameColName">Name of the company name column.</param>
        public StockListCsvParser(string tickerColName, string companyNameColName)
        {
            this.tickerColumnName = tickerColName;
            this.companyNameColumnName = companyNameColName;
        }

        /// <summary>
        /// Parses the CSV data for a listing of stocks.
        /// </summary>
        /// <param name="csvData">The CSV data to parse.</param>
        /// <returns>
        /// A listing of stocks parsed from the CSV data.
        /// </returns>
        public IList<Stock> ParseCsvData(string csvData)
        {
            if (string.IsNullOrWhiteSpace(csvData))
            {
                throw new DataFetchFailureException("The provided CSV data was empty.");
            }

            var csvLines = csvData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (csvLines.Length < 2)
            {
                throw new DataFetchFailureException("The provided CSV data does not have the minimum number of rows.  Data: " + csvData);
            }

            var headerFields = csvLines[0].Replace("\"", string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            var tickerIndex = GetColumnIndex(headerFields, this.tickerColumnName);
            if (tickerIndex < 0)
            {
                throw new DataFetchFailureException(
                    "The provided CSV data does not contain the expected column header for the TICKER column.  Expected column header: " + this.tickerColumnName + "; Actual headers: " + csvLines[0]);
            }

            var companyIndex = GetColumnIndex(headerFields, this.companyNameColumnName);
            if (companyIndex < 0)
            {
                throw new DataFetchFailureException(
                    "The provided CSV data does not contain the expected column header for the NAME column.  Expected column header: " + this.companyNameColumnName + "; Actual headers: " + csvLines[0]);
            }

            return this.ParseRows(csvLines, tickerIndex, companyIndex);
        }

        /// <summary>
        /// Gets the index of the specified column.
        /// </summary>
        /// <param name="headerFields">The header fields from the CSV data.</param>
        /// <param name="columnName">Name of the column to search for.</param>
        /// <returns>
        /// The index of the specified column.  -1 if the column is not found.
        /// </returns>
        private static int GetColumnIndex(string[] headerFields, string columnName)
        {
            for (var index = 0; index < headerFields.Length; index++)
            {
                if (headerFields[index].ToLower().Equals(columnName.ToLower()))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes extra comma characters from the CSV line so they do not interfere with parsing.
        /// </summary>
        /// <param name="csvLine">The CSV line to clean.</param>
        /// <returns>
        /// The line cleaned of commas (and of double quotes).
        /// </returns>
        /// <remarks>
        /// The CSV data is likely to contain string values enclosed in double quotes that contain non-CSV comma characters.
        /// Those characters interfere with simple CSV parsing, and so this method will scan for the quote-encapsulated sub-strings
        /// and remove all commas (and quotes) from those sub-strings.
        /// </remarks>
        private static string RemoveExtraCommaChars(string csvLine)
        {
            var cleanedData = string.Empty;

            var finished = false;

            while (!finished)
            {
                var spanStart = csvLine.IndexOf('\"', 0);

                if (spanStart >= 0)
                {
                    var spanEnd = csvLine.IndexOf('\"', spanStart + 1);

                    if (spanEnd < 0)
                    {
                        throw new DataFetchFailureException("The provided CSV data contains a record with an odd number of quote symbols:  " + csvLine);
                    }

                    // Chunk off the clean portion of the line.
                    cleanedData += csvLine.Substring(0, spanStart);
                    csvLine = csvLine.Remove(0, spanStart);

                    // Segregate the identified span for parsing.
                    var spanLength = spanEnd - spanStart + 1;
                    var span = csvLine.Substring(0, spanLength);
                    csvLine = csvLine.Remove(0, spanLength);

                    // Clean the span.
                    span = span.Replace(",", string.Empty);
                    span = span.Replace("\"", string.Empty);
                    cleanedData += span;
                }
                else
                {
                    // The remainder of the CSV line is clean.
                    cleanedData += csvLine;
                    finished = true;
                }
            }

            return cleanedData;
        }

        /// <summary>
        /// Parses the CSV rows into a listing of stocks.
        /// </summary>
        /// <param name="csvLines">The CSV lines.</param>
        /// <param name="tickerIndex">Index of the ticker column.</param>
        /// <param name="nameIndex">Index of the name column.</param>
        /// <returns>
        /// A listing of parsed stocks.
        /// </returns>
        private IList<Stock> ParseRows(string[] csvLines, int tickerIndex, int nameIndex)
        {
            var stocks = new List<Stock>();

            // Iterate through the stock records, skipping the first line which is a header.
            for (var index = 1; index < csvLines.Length; index++)
            {
                //// Create a new stock from each record.
                
                // Clean the CSV line of extra commas.
                var csvLine = RemoveExtraCommaChars(csvLines[index]);

                var csvFields = csvLine.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (csvFields.Length < 2)
                {
                    throw new DataFetchFailureException("The provided CSV data contains an unparseable line.  CSV Line:  " + csvLine);
                }

                var stock = new Stock { Ticker = csvFields[tickerIndex], CompanyName = csvFields[nameIndex] };
                stocks.Add(stock);
            }

            return stocks;
        }
    }
}
