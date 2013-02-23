// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="The Logans Ferry Software Co.">
//   Copyright 2012, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of LfBackTesterPrototype.
//   
//   LfBackTesterPrototype is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   LfBackTesterPrototype is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   LfBackTesterPrototype. If not, see http://www.gnu.org/licenses/.
// </license>
// <author>Aaron Morris</author>
// <last_updated>04/28/2012 9:11 PM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The main program. 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The initial account size.
        /// </summary>
        private const float InitialAccountSize = 5000f;

        /// <summary>
        /// The writer that will write the results of backtesting to a worksheet.
        /// </summary>
        private static readonly WorksheetWriter Writer = new WorksheetWriter();

        /// <summary>
        /// The average returns from position trading.
        /// </summary>
        private static float positionReturns = 0.0f;

        /// <summary>
        /// The average returns from swing trading.
        /// </summary>
        private static float swingReturns = 0.0f;

        /// <summary>
        /// A flag that indicates whether the user has selected to exit.
        /// </summary>
        private bool isExiting;

        /// <summary>
        /// The main method.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        private void Run()
        {
            while (!this.isExiting)
            {
                this.DisplayMenu();
                this.ReadSelection();
            }
        }

        /// <summary>
        /// Displays the program menu.
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Back Tester Console");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1) Update Tickers in Watch List");
            Console.WriteLine("2) Look For Current Entrances");
            Console.WriteLine("3) Create Worksheet");
            Console.WriteLine("4) Candlestick Analysis");
            Console.WriteLine("5) Exit");
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
                    this.UpdateTickersInWatchList();
                    break;

                case 2:
                    this.LookForEntrances();
                    break;

                case 3:
                    this.CreateWorksheet();
                    break;

                case 4:
                    new CandlestickConsole().Run();
                    break;

                case 5:
                    this.isExiting = true;
                    break;

                default:
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine();
                    break;
            }
        }

        /// <summary>
        /// Updates the tickers in watch list.
        /// </summary>
        private void UpdateTickersInWatchList()
        {
            positionReturns = 0;

            this.DisplayWatchLists();
            Console.Write("Select ID of watch list to update. ([Blank] to cancel.) -->  ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine();
                return;
            }

            int id;
            if (!int.TryParse(input, out id))
            {
                Console.WriteLine("Invalid input...no watch list was updated.");
                Console.WriteLine();
                return;
            }

            List<Watchlist> watchLists;
            using (var context = new StockScreenerEntities())
            {
                watchLists = context.Watchlists.ToList();
            }

            if (watchLists.Count(list => list.ID == id) <= 0)
            {
                Console.WriteLine("Invalid input...no watch list was upated.");
                Console.WriteLine();
            }
            else
            {
                List<string> tickers;
                using (var context = new StockScreenerEntities())
                {
                    tickers = context.Stocks.Select(stock => stock.Ticker).ToList();
                }

                var tickersToAdd = new List<string>();

                foreach (var ticker in tickers)
                {
                    List<StockDaily> dailies;
                    using (var context = new StockScreenerEntities())
                    {
                        dailies = context.Stocks.First(stock => stock.Ticker.Equals(ticker)).StockDailies.ToList();
                    }

                    var history = dailies.Select(daily => new PriceData(daily)).ToList();

                    // Cleanup
                    const float Epsilon = 0.000001f;
                    history.RemoveAll(priceData => priceData.Volume == 0);
                    history.RemoveAll(priceData => Math.Abs(priceData.Open - 0.0f) < Epsilon);
                    history.RemoveAll(priceData => Math.Abs(priceData.High - 0.0f) < Epsilon);
                    history.RemoveAll(priceData => Math.Abs(priceData.Low - 0.0f) < Epsilon);
                    history.RemoveAll(priceData => Math.Abs(priceData.Close - 0.0f) < Epsilon);

                    if (history.Count <= 0)
                    {
                        continue;
                    }

                    var results = RunPositionTest(ticker, history);

                    Console.WriteLine(ticker + ":  {0:C}", results.NetUnrealizedProfitLosses);

                    positionReturns += results.NetUnrealizedProfitLosses;

                    if (results.NetUnrealizedProfitLosses > 0)
                    {
                        tickersToAdd.Add(ticker);
                    }
                }

                using (var context = new StockScreenerEntities())
                {
                    var listToUpdate = context.Watchlists.First(list => list.ID == id);
                    listToUpdate.Stocks.Clear();

                    foreach (var ticker in tickersToAdd)
                    {
                        var stockToAdd = context.Stocks.First(stock => stock.Ticker.Equals(ticker));
                        listToUpdate.Stocks.Add(stockToAdd);
                    }

                    context.SaveChanges();
                }

                Console.WriteLine("Watch list was updated.  Net Returns:   {0:C}", positionReturns);
                Console.WriteLine();
            }
        }

        private void LookForEntrances()
        {
            positionReturns = 0;

            this.DisplayWatchLists();
            Console.Write("Select ID of watch list to analzye. ([Blank] to cancel.) -->  ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine();
                return;
            }

            int id;
            if (!int.TryParse(input, out id))
            {
                Console.WriteLine("Invalid input...no watch list was selected.");
                Console.WriteLine();
                return;
            }

            List<Watchlist> watchLists;
            using (var context = new StockScreenerEntities())
            {
                watchLists = context.Watchlists.ToList();
            }

            if (watchLists.Count(list => list.ID == id) <= 0)
            {
                Console.WriteLine("Invalid input...no watch list was selected.");
                Console.WriteLine();
            }
            else
            {
                List<string> tickers;
                using (var context = new StockScreenerEntities())
                {
                    tickers =
                        context.Watchlists.First(list => list.ID == id).Stocks.Select(stock => stock.Ticker).ToList();
                }

                Console.WriteLine("Working...");

                foreach (var ticker in tickers)
                {
                    List<StockDaily> dailies;
                    using (var context = new StockScreenerEntities())
                    {
                        dailies = context.Stocks.First(stock => stock.Ticker.Equals(ticker)).StockDailies.ToList();
                    }

                    var history = dailies.Select(daily => new PriceData(daily)).ToList();

                    // Cleanup
                    const float Epsilon = 0.000001f;
                    history.RemoveAll(priceData => priceData.Volume == 0);
                    history.RemoveAll(priceData => Math.Abs(priceData.Open - 0.0f) < Epsilon);
                    history.RemoveAll(priceData => Math.Abs(priceData.High - 0.0f) < Epsilon);
                    history.RemoveAll(priceData => Math.Abs(priceData.Low - 0.0f) < Epsilon);
                    history.RemoveAll(priceData => Math.Abs(priceData.Close - 0.0f) < Epsilon);

                    if (history.Count <= 0)
                    {
                        continue;
                    }

                    positionReturns += RunPositionTest(ticker, history).NetUnrealizedProfitLosses;
                }

                Console.WriteLine("Watch list was analyized.  Net returns {0:C}", positionReturns);
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Displays a listing of the current watch lists.
        /// </summary>
        private void DisplayWatchLists()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Current Watch Lists");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();

            using (var context = new StockScreenerEntities())
            {
                var watchLists = context.Watchlists.ToList();

                if (watchLists.Count <= 0)
                {
                    Console.WriteLine("No Current Watch Lists");
                }

                foreach (var list in watchLists)
                {
                    Console.WriteLine(list.ID + "\t" + list.Name);
                }
            }

            Console.WriteLine();
        }

        private void CreateWorksheet()
        {
            Console.WriteLine();
            Console.Write("Enter the ticker symbol [BLANK to cancel] -->  ");

            var ticker = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(ticker))
            {
                return;
            }

            using (var context = new StockScreenerEntities())
            {
                if (context.Stocks.Count(stock => stock.Ticker.Equals(ticker)) <= 0)
                {
                    Console.WriteLine("Invalid Ticker Symbol");
                    Console.WriteLine();
                    return;
                }

                var history = context.Stocks.First(stock => stock.Ticker.Equals(ticker)).StockDailies.Select(daily => new PriceData(daily)).ToList();

                // Cleanup
                const float Epsilon = 0.000001f;
                history.RemoveAll(priceData => priceData.Volume == 0);
                history.RemoveAll(priceData => Math.Abs(priceData.Open - 0.0f) < Epsilon);
                history.RemoveAll(priceData => Math.Abs(priceData.High - 0.0f) < Epsilon);
                history.RemoveAll(priceData => Math.Abs(priceData.Low - 0.0f) < Epsilon);
                history.RemoveAll(priceData => Math.Abs(priceData.Close - 0.0f) < Epsilon);

                if (history.Count <= 0)
                {
                    Console.WriteLine("Ticker does not have valid price history records.");
                    Console.WriteLine();
                    return;
                }

                var results = RunPositionTest(ticker, history);
                var filename = ticker.Trim() + "_Worksheet.csv";
                new WorksheetWriter().WriteWorksheetToCsv(filename, results.Worksheet);
                System.Diagnostics.Process.Start(filename);
            }
        }

        /// <summary>
        /// Runs the comparison operation between two strategies on the provided ticker.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        private static void RunComparison(string ticker)
        {
            Console.WriteLine("Current ticker:  " + ticker);

            // TODO:  Create watch list
            //var stockDailies = from stockDaily in Database.StockDailies
            //              where stockDaily.Ticker.Equals(ticker)
            //              select stockDaily;
            //var history = stockDailies.Select(daily => new PriceData { Open = Convert.ToSingle(daily.OpenPrice), High = Convert.ToSingle(daily.HighPrice), Low = Convert.ToSingle(daily.LowPrice), Close = Convert.ToSingle(daily.ClosePrice), Volume = daily.Volume, IntervalOpenTime = daily.Date }).ToList();
            
            

            // TODO: Targeted Tickers
            //Writer.WriteWorksheetToCsv(ticker + "PositionWorksheet.csv", positionResults.Worksheet);
            
            // TODO:  Create watchlist
            //if (positionResults.NetUnrealizedProfitLosses > 0)
            //{
            //    var fileLine = string.Format(
            //        "{0}\t{1:C}" + Environment.NewLine, ticker, positionResults.NetUnrealizedProfitLosses);
            //    File.AppendAllText("watchlist.txt", fileLine);
            //}
        }

        /// <summary>
        /// Runs the position back test.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        /// <param name="history">The price history.</param>
        /// <returns>
        /// The results of the back test.
        /// </returns>
        private static BackTestResults RunPositionTest(string ticker, List<PriceData> history)
        {
            var broker = new Broker(InitialAccountSize);
            var backTester = new PositionBackTester(
                ticker,
                history,
                broker,
                new AverageDirectionalMovementCalculator(),
                new OnBalanceVolumeCalculator(),
                new MovingAverageCalculator());

            return backTester.RunBackTest();
        }

        /// <summary>
        /// Runs the swing back test.
        /// </summary>
        /// <param name="history">The price history.</param>
        /// <returns>
        /// The results of the back test.
        /// </returns>
        private static BackTestResults RunSwingTest(List<PriceData> history)
        {
            var broker = new Broker(InitialAccountSize);
            var backTester = new SwingBackTester(
                history,
                broker,
                new AverageDirectionalMovementCalculator(),
                new OnBalanceVolumeCalculator(),
                new MovingAverageCalculator());

            return backTester.RunBackTest();
        }
    }
}
