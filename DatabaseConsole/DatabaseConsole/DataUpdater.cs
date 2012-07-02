// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataUpdater.cs" company="The Logans Ferry Software Co.">
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
    using System.Data;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// A utility class that will update data in the database.
    /// </summary>
    public class DataUpdater
    {
        /// <summary>
        /// Updates the list of stocks in the database by adding new stocks from the list of current stocks and
        /// by deleting stocks that are no longer listed in the list of current stocks.
        /// </summary>
        /// <param name="currentStocks">The current stocks.</param>
        public void MakeDbStockListCurrent(List<Stock> currentStocks)
        {
            foreach (var stock in currentStocks)
            {
                try
                {
                    using (var context = new StockScreenerEntities())
                    {
                        if (context.Stocks.Any(s => s.Ticker.Equals(stock.Ticker)))
                        {
                            var stockToUpdate = context.Stocks.First(s => s.Ticker.Equals(stock.Ticker));
                            if (!stockToUpdate.CompanyName.Equals(stock.CompanyName))
                            {
                                Console.WriteLine("Updating " + stock.Ticker + ":  " + stockToUpdate.CompanyName + " to " + stock.CompanyName);

                                stockToUpdate.CompanyName = stock.CompanyName;
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            context.Stocks.AddObject(stock);
                            context.SaveChanges();

                            Console.WriteLine("Added " + stock.Exchange + ":" + stock.Ticker);
                        }
                    }
                }
                catch (UpdateException ex)
                {
                    if (string.IsNullOrEmpty(ex.InnerException.Message) || !ex.InnerException.Message.Contains("Cannot insert duplicate key"))
                    {
                        throw;
                    }
                }
            }

            this.PruneStocksInDb(currentStocks);
        }

        /// <summary>
        /// Updates the list of stocks in the database with the most current data from Google.
        /// </summary>
        /// <param name="stocks">The stocks to update.</param>
        public void UpdateTickerHistories(IEnumerable<Stock> stocks)
        {
            var googleDataSource = new GoogleCsvDataSource();

            var failures = new List<Stock>();

            foreach (var stock in stocks)
            {
                Console.Write("Updating " + stock.Ticker + "...");

                // Determine date of most current record.
                var lastEntryDate = GetDateOfLastRecord(stock.Ticker);

                // Retrieve new records from Google, starting with the day after the last record's date.
                var startDate = lastEntryDate.Add(TimeSpan.FromDays(1));
                List<StockDaily> newRecords;
                try
                {
                    newRecords = googleDataSource.GetDataFromGoogle(stock, startDate, DateTime.Now);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    failures.Add(stock);
                    continue;
                }
                
                // Add records to the database.
                if (newRecords.Count > 0)
                {
                    UpdateDailiesInDatabase(newRecords);
                }
                else
                {
                    Console.WriteLine("Inserted 0 records.");
                }

                Thread.Sleep(1000);
            }

            Console.WriteLine("The following tickers failed to upate:");
            foreach (var stock in failures)
            {
                Console.WriteLine(stock.Ticker);
            }
        }

        /// <summary>
        /// Gets the date of the last record in the database for the ticker symbol.
        /// </summary>
        /// <param name="ticker">The ticker symbol to query.</param>
        /// <returns>
        /// The date of the last record for the ticker.
        /// </returns>
        private static DateTime GetDateOfLastRecord(string ticker)
        {
            using (var context = new StockScreenerEntities())
            {
                var history = context.StockDailies.Where(stock => ticker.Equals(stock.Ticker)).ToList();
                
                return history.Count > 0 
                    ? history.Max(stock => stock.Date)
                    : DateTime.Now.Subtract(TimeSpan.FromDays(1826));
            }
        }

        /// <summary>
        /// Updates the daily stock data in database.
        /// </summary>
        /// <param name="dailies">The daily data to insert into the database.</param>
        private static void UpdateDailiesInDatabase(IEnumerable<StockDaily> dailies)
        {
            using (var context = new StockScreenerEntities())
            {
                foreach (var daily in dailies)
                {
                    context.StockDailies.AddObject(daily);
                }

                var numRecords = context.SaveChanges();

                Console.WriteLine("Inserted " + numRecords + " records.");
            }
        }

        /// <summary>
        /// Prunes the stocks in the DB by removing all stocks that are not in the list of current stocks.
        /// </summary>
        /// <param name="currentStocks">The list of current stocks that should remain in the DB.</param>
        private void PruneStocksInDb(IEnumerable<Stock> currentStocks)
        {
            using (var context = new StockScreenerEntities())
            {
                var databaseStocks = context.Stocks.ToList();
                foreach (var stock in databaseStocks.Where(stock => currentStocks.Count(list => list.Ticker.Equals(stock.Ticker)) <= 0))
                {
                    try
                    {
                        context.Stocks.DeleteObject(stock);
                        context.SaveChanges();
                        Console.WriteLine("Removed " + stock.Exchange + ":" + stock.Ticker);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
    }
}
