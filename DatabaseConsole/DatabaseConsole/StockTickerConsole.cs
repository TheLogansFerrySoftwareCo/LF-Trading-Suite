// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockTickerConsole.cs" company="The Logans Ferry Software Co.">
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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// A console (menu) that provides options for managing stock tickers.
    /// </summary>
    public class StockTickerConsole
    {
        /// <summary>
        /// A flag that indicates whether the user has selected to exit.
        /// </summary>
        private bool isExiting;

        /// <summary>
        /// Runs the program.
        /// </summary>
        public void Run()
        {
            while (!this.isExiting)
            {
                this.DisplayMenu();
                this.ReadSelection();
            }
        }

        /// <summary>
        /// Displays the main program menu.
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Stock Ticker Menu");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1) Update Price History for All Tickers");
            Console.WriteLine("2) Re-populate Ticker List in Database");
            Console.WriteLine("3) Exit");
            Console.WriteLine();
            Console.Write("-->  ");
        }

        /// <summary>
        /// Reads the user's menu selection selection.
        /// </summary>
        private void ReadSelection()
        {
            var input = Console.ReadLine();

            int menuItem;
            if (!int.TryParse(input, out menuItem))
            {
                menuItem = 0;
            }

            switch (menuItem)
            {
                case 1:
                    this.UpdateAllTickers();
                    break;

                case 2:
                    this.RepopulateTickersInDatabase();
                    break;

                case 3:
                    this.isExiting = true;
                    break;

                default:
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine();
                    break;
            }
        }

        /// <summary>
        /// Updates the price history for all tickers in the database.
        /// </summary>
        private void UpdateAllTickers()
        {
            List<Stock> stocks;
            using (var context = new StockScreenerEntities())
            {
                stocks = context.Stocks.ToList();
            }

            var dataUpdater = new DataUpdater();
            dataUpdater.UpdateTickerHistories(stocks);

            Console.ReadLine();
        }

        /// <summary>
        /// Repopulates the list of tickers in the database from the CSV files downloaded from NASDAQ's website.
        /// </summary>
        private void RepopulateTickersInDatabase()
        {
            const string NyseFile = @"TickerLists\NyseCompanyList.csv";
            const string NasdaqFile = @"TickerLists\NasdaqCompanyList.csv";

            if (!File.Exists(NyseFile))
            {
                Console.WriteLine("Error!  Cannot find the file containing the list of NYSE stocks (" + NyseFile + ").");
                Console.WriteLine("No tickers were updated.");
                Console.WriteLine();
                return;
            }

            if (!File.Exists(NasdaqFile))
            {
                Console.WriteLine("Error!  Cannot find the file containing the list of NASDAQ stocks (" + NasdaqFile + ").");
                Console.WriteLine("No tickers were updated.");
                Console.WriteLine();
                return;
            }

            // Create a list of stocks currently listed on the NYSE and NASDAQ
            var currentStocks = this.ReadStockListForExchange("NYSE", NyseFile);
            currentStocks.AddRange(this.ReadStockListForExchange("NASDAQ", NasdaqFile));

            // Update the database with the list of current stocks.
            var dataUpdater = new DataUpdater();
            dataUpdater.MakeDbStockListCurrent(currentStocks);
        }

        /// <summary>
        /// Reads a list of stocks for the specified stock exchange from the specified file name.
        /// </summary>
        /// <param name="exchangeId">The ID of the stock exchange.</param>
        /// <param name="companyListFile">The file containing the list of stocks for the exchange.</param>
        /// <returns>
        /// A list of stocks for the specified stock exchange.
        /// </returns>
        private List<Stock> ReadStockListForExchange(string exchangeId, string companyListFile)
        {
            var stocks = new List<Stock>();
            var csvLines = File.ReadAllLines(companyListFile);
            var tickerRegEx = new Regex(@"^[A-Z]*$");
            const int MaxNameLength = 50;

            // Loop through each line of the CSV file and create a new Stock object.
            // Start with index 1 to skip the header line.
            for (var index = 1; index < csvLines.Length; index++)
            {
                var fields = csvLines[index].Replace("\"", string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                
                // Validate the ticker symbol
                var ticker = fields[0].ToUpper(CultureInfo.InvariantCulture);
                if (!tickerRegEx.IsMatch(ticker))
                {
                    // This isn't a standard ticker with only capital letters.
                    continue;
                }

                // Length-limit the company name.
                var name = fields[1];
                if (name.Length > MaxNameLength)
                {
                    name = name.Substring(0, MaxNameLength);
                }

                // Create and add the stock.
                var stock = new Stock { Exchange = exchangeId, Ticker = ticker, CompanyName = name };
                stocks.Add(stock);
            }

            return stocks;
        }
    }
}
