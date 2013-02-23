// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatternManagementConsole.cs" company="The Logans Ferry Software Co.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A console for candlestick pattern management activities.
    /// </summary>
    public class PatternManagementConsole
    {
        private int whiteMarubozuLumenRating = 0;
        private int whiteMarubozuTotalLumens = 0;
        private int whiteMarubozuNumOccurences = 0;

        private int closingWhiteMarubozuLumenRating = 0;
        private int closingWhiteMarubozuTotalLumens = 0;
        private int closingWhiteMarubozuNumOccurences = 0;

        private int openingWhiteMarubozuLumenRating = 0;
        private int openingWhiteMarubozuTotalLumens = 0;
        private int openingWhiteMarubozuNumOccurences = 0;

        private int dragonflyDojiLumenRating = 0;
        private int dragonflyDojiTotalLumens = 0;
        private int dragonflyDojiNumOccurences = 0;

        /// <summary>
        /// A flag that indicates whether the user has selected to exit.
        /// </summary>
        private bool isExiting;

        /// <summary>
        /// Runs the console.
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
        /// Displays the console menu.
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Pattern Management Console");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1) Update Candlestick Statistics");
            Console.WriteLine("2) List Candlestick Patterns");
            Console.WriteLine("3) Add New Candlestick Pattern to DB");
            Console.WriteLine("4) Exit");
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
                    this.UpdateStatistics();
                    break;

                case 2:
                    this.ListPatterns();
                    break;

                case 3:
                    this.AddCandlestickPattern();
                    break;

                case 4:
                    this.isExiting = true;
                    break;

                default:
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine();
                    break;
            }
        }

        /// <summary>
        /// Lists the candlestick patterns stored in the database.
        /// </summary>
        private void ListPatterns()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Candlestick Patterns");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();

            using (var context = new StockScreenerEntities())
            {
                var candlesticks = context.Candlesticks.ToList();

                if (candlesticks.Count <= 0)
                {
                    Console.WriteLine("No Candlestick Patterns");
                }

                foreach (var candlestick in candlesticks)
                {
                    Console.WriteLine("{0} - {1}\t\t Frequency: {2}\t\t Lumens: {3}", candlestick.ID, candlestick.Name, candlestick.NumOccurrences, candlestick.LumenRating);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Adds a new candlestick pattern.
        /// </summary>
        private void AddCandlestickPattern()
        {
            // First, display a list of existing watch lists.
            this.ListPatterns();

            // Prompt the user to enter a name for the new watch list.
            Console.Write("Enter new candlestick name ([Blank] to cancel) -->  ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                // The user opted to cancel without entering a name.
                Console.WriteLine();
                return;
            }

            // Prompt the user to confirm the inputted name.
            Console.Write("Create candlestick named '" + name + "'?  (Y/N) -->  ");
            var isConfirmed = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(isConfirmed) || !isConfirmed.ToUpper().Equals("Y"))
            {
                // The user did not confirm the name.
                Console.WriteLine("Candlestick not created.");
                Console.WriteLine();
                return;
            }

            // Create a new entry in the database using the inputted name.
            using (var context = new StockScreenerEntities())
            {
                try
                {
                    context.Candlesticks.AddObject(new Candlestick { Name = name });
                    context.SaveChanges();
                    Console.WriteLine("Candlestick was created.");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void UpdateStatistics()
        {
            List<string> tickers;
            using (var context = new StockScreenerEntities())
            {
                tickers = context.Stocks.Select(stock => stock.Ticker).ToList();
            }

            foreach (var ticker in tickers)
            {
                using (var context = new StockScreenerEntities())
                {
                    var tickerEntries = context.CandlesticksToStocks.Where(entry => entry.Ticker.Equals(ticker));
                    foreach (var entry in tickerEntries)
                    {
                        context.CandlesticksToStocks.DeleteObject(entry);
                    }

                    context.SaveChanges();
                }

                List<StockDaily> dailies;
                using (var context = new StockScreenerEntities())
                {
                    dailies = context.Stocks.First(stock => stock.Ticker.Equals(ticker)).StockDailies.ToList();
                }

                if (dailies.Count > 0)
                {
                    var averageDailyRange = dailies.Average(daily => Math.Abs(daily.HighPrice - daily.LowPrice));

                    for (var index = 0; index < dailies.Count; index++)
                    {
                        var daily = dailies[index];
                        var candlestick = new CandlestickAnalyzer(
                            daily.Exchange, ticker, daily.OpenPrice, daily.HighPrice, daily.LowPrice, daily.ClosePrice, averageDailyRange);

                        this.LookForWhiteMarubozus(candlestick, dailies, index, daily.Date);
                        this.LookForClosingWhiteMarubozus(candlestick, dailies, index, daily.Date);
                        this.LookForOpeningWhiteMarubozus(candlestick, dailies, index, daily.Date);
                        this.LookForDragonflyDoji(candlestick, dailies, index, daily.Date);
                    }
                }
            }

            using (var context = new StockScreenerEntities())
            {
                var whiteMarubozu = context.Candlesticks.First(candle => candle.ID == PatternIds.WhiteMarubozu);
                var whiteClosingMarubozu = context.Candlesticks.First(candle => candle.ID == PatternIds.WhiteClosingMarubozu);
                var whiteOpeningMarubozu = context.Candlesticks.First(candle => candle.ID == PatternIds.WhiteOpeningMarubozu);
                var dragonflyDoji = context.Candlesticks.First(candle => candle.ID == PatternIds.DragonflyDoji);

                whiteMarubozu.LumenRating = this.whiteMarubozuLumenRating;
                whiteMarubozu.NumOccurrences = this.whiteMarubozuNumOccurences;

                whiteClosingMarubozu.LumenRating = this.closingWhiteMarubozuLumenRating;
                whiteClosingMarubozu.NumOccurrences = this.closingWhiteMarubozuNumOccurences;

                whiteOpeningMarubozu.LumenRating = this.openingWhiteMarubozuLumenRating;
                whiteOpeningMarubozu.NumOccurrences = this.openingWhiteMarubozuNumOccurences;

                dragonflyDoji.LumenRating = this.dragonflyDojiLumenRating;
                dragonflyDoji.NumOccurrences = this.dragonflyDojiNumOccurences;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Looks for white marubozus.
        /// </summary>
        private void LookForWhiteMarubozus(CandlestickAnalyzer candlestick, List<StockDaily> dailies, int index, DateTime date)
        {
            if (candlestick.IsWhiteMarubozu())
            {
                var peekIndex = index + 1;
                var lumens = 0;
                while (peekIndex < dailies.Count)
                {
                    if (dailies[peekIndex].LowPrice > candlestick.Low)
                    {
                        lumens++;
                    }
                    else
                    {
                        break;
                    }

                    if (peekIndex - index >= 100)
                    {
                        break;
                    }

                    peekIndex++;
                }

                this.whiteMarubozuNumOccurences++;
                this.whiteMarubozuTotalLumens += lumens;
                this.whiteMarubozuLumenRating = Convert.ToInt32(this.whiteMarubozuTotalLumens / this.whiteMarubozuNumOccurences);

                Console.WriteLine("White Marubozus -- Symbol=" + candlestick.TickerSymbol + ";  Date=" + date.ToShortDateString() + ";  Lumens=" + lumens + "; Rating=" + this.whiteMarubozuLumenRating);

                using (var context = new StockScreenerEntities())
                {
                    if (context.CandlesticksToStocks.Count(entry => entry.CandlestickId == PatternIds.WhiteMarubozu && entry.Ticker.Equals(candlestick.TickerSymbol)) > 0)
                    {
                        var candlestickToStock = context.CandlesticksToStocks.First(
                            entry =>
                            entry.CandlestickId == PatternIds.WhiteMarubozu
                            && entry.Ticker.Equals(candlestick.TickerSymbol));

                        var newLumenRating = ((candlestickToStock.LumenRating * candlestickToStock.NumOccurrences)
                                              + lumens) / ++candlestickToStock.NumOccurrences;

                        candlestickToStock.LumenRating = newLumenRating;
                    }
                    else
                    {
                        context.CandlesticksToStocks.AddObject(new CandlesticksToStock
                        {
                            CandlestickId = PatternIds.WhiteMarubozu,
                            Exchange = candlestick.Exchange,
                            Ticker = candlestick.TickerSymbol,
                            LumenRating = lumens,
                            NumOccurrences = 1
                        });
                    }

                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Looks for closing white marubozus.
        /// </summary>
        private void LookForClosingWhiteMarubozus(CandlestickAnalyzer candlestick, List<StockDaily> dailies, int index, DateTime date)
        {
            if (candlestick.IsClosingWhiteMarubozu())
            {
                var peekIndex = index + 1;
                var lumens = 0;
                while (peekIndex < dailies.Count)
                {
                    if (dailies[peekIndex].LowPrice > candlestick.Low)
                    {
                        lumens++;
                    }
                    else
                    {
                        break;
                    }

                    if (peekIndex - index >= 100)
                    {
                        break;
                    }

                    peekIndex++;
                }

                this.closingWhiteMarubozuNumOccurences++;
                this.closingWhiteMarubozuTotalLumens += lumens;
                this.closingWhiteMarubozuLumenRating = Convert.ToInt32(this.closingWhiteMarubozuTotalLumens / this.closingWhiteMarubozuNumOccurences);

                Console.WriteLine("Closing White Marubozus -- Symbol=" + candlestick.TickerSymbol + ";  Date=" + date.ToShortDateString() + ";  Lumens=" + lumens + "; Rating=" + this.closingWhiteMarubozuLumenRating);

                using (var context = new StockScreenerEntities())
                {
                    if (context.CandlesticksToStocks.Count(entry => entry.CandlestickId == PatternIds.WhiteClosingMarubozu && entry.Ticker.Equals(candlestick.TickerSymbol)) > 0)
                    {
                        var candlestickToStock = context.CandlesticksToStocks.First(
                            entry =>
                            entry.CandlestickId == PatternIds.WhiteClosingMarubozu
                            && entry.Ticker.Equals(candlestick.TickerSymbol));

                        var newLumenRating = ((candlestickToStock.LumenRating * candlestickToStock.NumOccurrences)
                                              + lumens) / ++candlestickToStock.NumOccurrences;

                        candlestickToStock.LumenRating = newLumenRating;
                    }
                    else
                    {
                        context.CandlesticksToStocks.AddObject(new CandlesticksToStock
                        {
                            CandlestickId = PatternIds.WhiteClosingMarubozu,
                            Exchange = candlestick.Exchange,
                            Ticker = candlestick.TickerSymbol,
                            LumenRating = lumens,
                            NumOccurrences = 1
                        });
                    }

                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Looks for opening white marubozus.
        /// </summary>
        private void LookForOpeningWhiteMarubozus(CandlestickAnalyzer candlestick, List<StockDaily> dailies, int index, DateTime date)
        {
            if (candlestick.IsOpeningWhiteMarubozu())
            {
                var peekIndex = index + 1;
                var lumens = 0;
                while (peekIndex < dailies.Count)
                {
                    if (dailies[peekIndex].LowPrice > candlestick.Low)
                    {
                        lumens++;
                    }
                    else
                    {
                        break;
                    }

                    if (peekIndex - index >= 100)
                    {
                        break;
                    }

                    peekIndex++;
                }

                this.openingWhiteMarubozuNumOccurences++;
                this.openingWhiteMarubozuTotalLumens += lumens;
                this.openingWhiteMarubozuLumenRating = Convert.ToInt32(this.openingWhiteMarubozuTotalLumens / this.openingWhiteMarubozuNumOccurences);

                Console.WriteLine("Opening White Marubozus -- Symbol=" + candlestick.TickerSymbol + ";  Date=" + date.ToShortDateString() + ";  Lumens=" + lumens + "; Rating=" + this.openingWhiteMarubozuLumenRating);

                using (var context = new StockScreenerEntities())
                {
                    if (context.CandlesticksToStocks.Count(entry => entry.CandlestickId == PatternIds.WhiteOpeningMarubozu && entry.Ticker.Equals(candlestick.TickerSymbol)) > 0)
                    {
                        var candlestickToStock = context.CandlesticksToStocks.First(
                            entry =>
                            entry.CandlestickId == PatternIds.WhiteOpeningMarubozu
                            && entry.Ticker.Equals(candlestick.TickerSymbol));

                        var newLumenRating = ((candlestickToStock.LumenRating * candlestickToStock.NumOccurrences)
                                              + lumens) / ++candlestickToStock.NumOccurrences;

                        candlestickToStock.LumenRating = newLumenRating;
                    }
                    else
                    {
                        context.CandlesticksToStocks.AddObject(new CandlesticksToStock
                            {
                                CandlestickId = PatternIds.WhiteOpeningMarubozu, 
                                Exchange = candlestick.Exchange,
                                Ticker = candlestick.TickerSymbol, 
                                LumenRating = lumens, 
                                NumOccurrences = 1
                            });
                    }

                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Looks for opening white marubozus.
        /// </summary>
        private void LookForDragonflyDoji(CandlestickAnalyzer candlestick, List<StockDaily> dailies, int index, DateTime date)
        {
            if (candlestick.IsDragonflyDoji())
            {
                candlestick.IsDragonflyDoji();

                var peekIndex = index + 1;
                var lumens = 0;
                while (peekIndex < dailies.Count)
                {
                    if (dailies[peekIndex].LowPrice > candlestick.Low)
                    {
                        lumens++;
                    }
                    else
                    {
                        break;
                    }

                    if (peekIndex - index >= 100)
                    {
                        break;
                    }

                    peekIndex++;
                }

                this.dragonflyDojiNumOccurences++;
                this.dragonflyDojiTotalLumens += lumens;
                this.dragonflyDojiLumenRating = Convert.ToInt32(this.dragonflyDojiTotalLumens / this.dragonflyDojiNumOccurences);

                Console.WriteLine("Dragonfly Doji -- Symbol=" + candlestick.TickerSymbol + ";  Date=" + date.ToShortDateString() + ";  Lumens=" + lumens + "; Rating=" + this.dragonflyDojiLumenRating);

                using (var context = new StockScreenerEntities())
                {
                    if (context.CandlesticksToStocks.Count(entry => entry.CandlestickId == PatternIds.DragonflyDoji && entry.Ticker.Equals(candlestick.TickerSymbol)) > 0)
                    {
                        var candlestickToStock = context.CandlesticksToStocks.First(
                            entry =>
                            entry.CandlestickId == PatternIds.DragonflyDoji
                            && entry.Ticker.Equals(candlestick.TickerSymbol));

                        var newLumenRating = ((candlestickToStock.LumenRating * candlestickToStock.NumOccurrences)
                                              + lumens) / ++candlestickToStock.NumOccurrences;

                        candlestickToStock.LumenRating = newLumenRating;
                    }
                    else
                    {
                        context.CandlesticksToStocks.AddObject(new CandlesticksToStock
                        {
                            CandlestickId = PatternIds.DragonflyDoji,
                            Exchange = candlestick.Exchange,
                            Ticker = candlestick.TickerSymbol,
                            LumenRating = lumens,
                            NumOccurrences = 1
                        });
                    }

                    context.SaveChanges();
                }
            }
        }

        private static class PatternIds
        {
            public const int WhiteMarubozu = 1;

            public const int WhiteClosingMarubozu = 2;

            public const int WhiteOpeningMarubozu = 3;

            public const int DragonflyDoji = 4;
        }
    }
}
