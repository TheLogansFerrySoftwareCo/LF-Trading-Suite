// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandlestickConsole.cs" company="The Logans Ferry Software Co.">
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogansFerry.BackTesterPrototype
{
    /// <summary>
    /// A command console for candlestick analysis.
    /// </summary>
    public class CandlestickConsole
    {
        /// <summary>
        /// A flag that indicates whether the user has selected to exit.
        /// </summary>
        private bool isExiting;

        private int whiteMarubozuLumenRating = 0;
        private long whiteMarubozuTotalLumens = 0;
        private long whiteMarubozuNumOccurences = 0;

        private int closingWhiteMarubozuLumenRating = 0;
        private long closingWhiteMarubozuTotalLumens = 0;
        private long closingWhiteMarubozuNumOccurences = 0;

        private int openingWhiteMarubozuLumenRating = 0;
        private long openingWhiteMarubozuTotalLumens = 0;
        private long openingWhiteMarubozuNumOccurences = 0;

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
            Console.WriteLine("Candlestick Console");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1) Look for Examples");
            Console.WriteLine("2) Pattern Management");
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
                    this.LookForExamples();
                    break;

                case 2:
                    new PatternManagementConsole().Run();
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
        /// Look for candlestick examples.
        /// </summary>
        private void LookForExamples()
        {
            this.DisplayCandlestickTypes();

            var input = Console.ReadLine();

            int menuItem;
            if (!int.TryParse(input, out menuItem))
            {
                menuItem = 0;
            }

            switch (menuItem)
            {
                case 1:
                    this.LookForMarubozus();
                    break;

                case 2:
                    return;

                default:
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine();
                    break;
            }
        }

        /// <summary>
        /// Displays the candlestick types.
        /// </summary>
        private void DisplayCandlestickTypes()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Candlestick Types");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1)  Marubozus");
            Console.WriteLine("2) Exit");
            Console.WriteLine();
            Console.Write("-->  ");
        }

        private void LookForMarubozus()
        {
            List<string> tickers;
            using (var context = new StockScreenerEntities())
            {
                tickers = context.Stocks.Select(stock => stock.Ticker).ToList();
            }

            foreach (var ticker in tickers)
            {
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
                    }
                }
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

                    if (peekIndex - index >= 20)
                    {
                        break;
                    }

                    peekIndex++;
                }

                this.whiteMarubozuNumOccurences++;
                this.whiteMarubozuTotalLumens += lumens;
                this.whiteMarubozuLumenRating = Convert.ToInt32(this.whiteMarubozuTotalLumens / this.whiteMarubozuNumOccurences);

                Console.WriteLine("White Marubozus -- Symbol=" + candlestick.TickerSymbol + ";  Date=" + date.ToShortDateString() + ";  Lumens=" + lumens + "; Rating=" + this.whiteMarubozuLumenRating);
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

                    if (peekIndex - index >= 20)
                    {
                        break;
                    }

                    peekIndex++;
                }

                this.closingWhiteMarubozuNumOccurences++;
                this.closingWhiteMarubozuTotalLumens += lumens;
                this.closingWhiteMarubozuLumenRating = Convert.ToInt32(this.closingWhiteMarubozuTotalLumens / this.closingWhiteMarubozuNumOccurences);

                Console.WriteLine("Closing White Marubozus -- Symbol=" + candlestick.TickerSymbol + ";  Date=" + date.ToShortDateString() + ";  Lumens=" + lumens + "; Rating=" + this.closingWhiteMarubozuLumenRating);
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

                    if (peekIndex - index >= 20)
                    {
                        break;
                    }

                    peekIndex++;
                }

                this.openingWhiteMarubozuNumOccurences++;
                this.openingWhiteMarubozuTotalLumens += lumens;
                this.openingWhiteMarubozuLumenRating = Convert.ToInt32(this.openingWhiteMarubozuTotalLumens / this.openingWhiteMarubozuNumOccurences);

                Console.WriteLine("Opening White Marubozus -- Symbol=" + candlestick.TickerSymbol + ";  Date=" + date.ToShortDateString() + ";  Lumens=" + lumens + "; Rating=" + this.openingWhiteMarubozuLumenRating);
            }
        }

        /// <summary>
        /// Adds the candlestick pattern to the database.
        /// </summary>
        private void AddCandlestickPattern()
        {
            
        }
    }
}
