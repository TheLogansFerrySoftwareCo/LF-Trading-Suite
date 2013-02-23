// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionBackTester.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>04/28/2012 9:13 PM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The main class of this prototype that will manage the overall backtesting operation.
    /// </summary>
    public class ExperimentalPositionBackTester
    {
        /// <summary>
        /// The price data that will be used for the back test.
        /// </summary>
        private readonly List<PriceData> priceHistory;

        /// <summary>
        /// The broker that will facilitate trades during back-testing.
        /// </summary>
        private readonly Broker broker;

        /// <summary>
        /// A list of short-term low swing points.
        /// </summary>
        /// <remarks>
        /// A short-term low swing point is a day with a lower low and a lower high than preceeding and following days.
        /// </remarks>
        private readonly List<float> shortTermLows;

        /// <summary>
        /// A list of intermediate-term low swing points.
        /// </summary>
        /// <remarks>
        /// An intermediate low swing point is a short-term low that is lower than the preceeding and following short-term lows.
        /// </remarks>
        private readonly List<float> intermediateLows;

        /// <summary>
        /// A list of long-term low swing points.
        /// </summary>
        /// <remarks>
        /// A long-term low swing point is an intermediate-term low that is lower than the preceeding and following intermediate lows.
        /// </remarks>
        private readonly List<float> longTermLows;

        /// <summary>
        /// A list of short-term high swing points.
        /// </summary>
        /// <remarks>
        /// A short-term high swing point is a day with a higher low and a higher high than preceeding and following days.
        /// </remarks>
        private readonly List<float> shortTermHighs;

        /// <summary>
        /// A list of intermediate-term high swing points.
        /// </summary>
        /// <remarks>
        /// An intermediate high swing point is a short-term high that is higher than the preceeding and following short-term highs.
        /// </remarks>
        private readonly List<float> intermediateHighs;

        /// <summary>
        /// A list of long-term high swing points.
        /// </summary>
        /// <remarks>
        /// A long-term high swing point is an intermediate-term high that is higher than the preceeding and following intermediate highs.
        /// </remarks>
        private readonly List<float> longTermHighs;

        /// <summary>
        /// The investment quantity that will be used to build pyramid-sized positions.
        /// </summary>
        private readonly int[] pyramidLevels = { 200, 100 };

        /// <summary>
        /// The index of the current simulated day in the price history.
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// The 52-week high price.
        /// </summary>
        private float current52WeekHigh;

        /// <summary>
        /// The 52-week low price.
        /// </summary>
        private float current52WeekLow;

        /// <summary>
        /// The location of the current price within the 52 week range.
        /// </summary>
        private float current52WeekPercentage;

        /// <summary>
        /// The worksheet entry for the current simulated day.
        /// </summary>
        private WorksheetEntry currentWorksheetEntry;

        /// <summary>
        /// The currently configured activation price for the current stop order.
        /// </summary>
        private float currentStopPrice;

        /// <summary>
        /// The last day that was a low swing day.
        /// </summary>
        private DateTime lastIntermediateLowSwingDate;

        /// <summary>
        /// The last day that was a high swing day.
        /// </summary>
        private DateTime lastIntermediHighSwingDate;

        /// <summary>
        /// The tested ticker symbol.
        /// </summary>
        private string ticker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperimentalPositionBackTester"/> class.
        /// </summary>
        /// <param name="ticker">The ticker.</param>
        /// <param name="priceHistory">The price history.</param>
        /// <param name="broker">The broker.</param>
        /// <param name="adxCalculator">The ADX calculator.</param>
        /// <param name="obvCalculator">The OBV calculator.</param>
        /// <param name="emaCalculator">The EMA calculator.</param>
        public ExperimentalPositionBackTester(string ticker, List<PriceData> priceHistory, Broker broker, AverageDirectionalMovementCalculator adxCalculator, OnBalanceVolumeCalculator obvCalculator, MovingAverageCalculator emaCalculator)
        {
            this.priceHistory = priceHistory;
            this.broker = broker;
            this.ticker = ticker;

            this.shortTermHighs = new List<float>();
            this.shortTermLows = new List<float>();
            this.intermediateHighs = new List<float>();
            this.intermediateLows = new List<float>();
            this.longTermHighs = new List<float>();
            this.longTermLows = new List<float>();

            // Perform technical analysis calculations in advance of the simulation.
            adxCalculator.PopulateHistoricalAdxValues(this.priceHistory);
            obvCalculator.PopulateHistoricalObvValues(this.priceHistory);
            emaCalculator.PopulateHistoricalEmaValues(this.priceHistory);
        }

        /// <summary>
        /// Gets the price data for the "current date" within the simulation.
        /// </summary>
        private PriceData Today
        {
            get
            {
                return this.priceHistory[this.currentIndex];
            }
        }

        /// <summary>
        /// Gets the price data for "yesterday" within the simulation.
        /// </summary>
        private PriceData Yesterday
        {
            get
            {
                return this.priceHistory[this.currentIndex - 1];
            }
        }

        /// <summary>
        /// Runs the back test.
        /// </summary>
        /// <returns>
        /// The results of the backtest.
        /// </returns>
        public BackTestResults RunBackTest()
        {
            var results = new BackTestResults();

            // For each day after the first.
            for (this.currentIndex = 1; this.currentIndex < this.priceHistory.Count; this.currentIndex++)
            {
                // Initialize a new worksheet entry.
                this.currentWorksheetEntry = new WorksheetEntry();

                // Process all pending orders based on the current day's metrics.
                this.broker.ProcessOrdersForToday(this.Today);

                // Determine the price direction that occurred for "today" in the simulation.
                this.Today.Direction = this.CalculateDailyDirection(this.Today, this.currentIndex - 1);

                // Update the 52-week values based on today's price.
                this.current52WeekHigh = this.Find52WeekHighPrice();
                this.current52WeekLow = this.Find52WeekLowPrice();
                this.current52WeekPercentage = (this.Today.Close - this.current52WeekLow)
                                               / (this.current52WeekHigh - this.current52WeekLow);

                //// Look for swing points.  Note: There must be at least 3 days worth of data to confirm a swing point.
                
                if (this.currentIndex >= 3 && this.Today.Direction == PriceDirections.Down)
                {
                    // If today is a down day, then it might confirm a High swing point.
                    this.LookForShortTermHigh();
                }

                if (this.currentIndex >= 3 && this.Today.Direction == PriceDirections.Up)
                {
                    // If today is an up day, then it might confirm a Low swing point.
                    this.LookForShortTermLow();
                }

                //// Place new orders with the broker based on today's performance.
                
                // We must have one year of data before we can begin placing orders
                if (this.currentIndex > 252)
                {
                    if (this.broker.CurrentPositionSize == 0)
                    {
                        // There are no open positions, so look for a new entrance.
                        this.LookForEntrance();
                    }
                    else
                    {
                        //// There is an open position, so look for maintanence orders.
                        
                        // First, look to see if we should build-up the position with a pyramid increase.
                        var isIncreaseOrderPlaced = this.LookForPyramidIncrease();

                        // If we didn't place an increase order, then just verify the current stop order.
                        if (!isIncreaseOrderPlaced)
                        {
                            this.AdjustStopOrder();
                        }
                    }
                }

                //// Update the markers for the last intermediate high and last intermediate low.

                if (this.Today.ConfirmsIntermediateLowSwingPoint)
                {
                    this.lastIntermediateLowSwingDate = this.Today.IntervalOpenTime;
                }

                if (this.Today.ConfirmsIntermediateHighSwingPoint)
                {
                    this.lastIntermediHighSwingDate = this.Today.IntervalOpenTime;
                }

                // Log the worksheet entry.
                this.UpdateWorksheetEntry();
                results.Worksheet.Add(this.currentWorksheetEntry);
            }

            // Populate the final results of the back test
            results.EndingBalance = this.broker.CurrentBalance;
            results.HighestBalance = this.broker.HighestBalance;
            results.InitialBalance = this.broker.InitialBalance;
            results.LowestBalance = this.broker.LowestBalance;
            results.NetProfitsFromLongPositions = this.broker.NetProfitsFromLongPositions;
            results.NetProfitsFromShortPositions = this.broker.NetProfitsFromShortPositions;
            results.NumLongPositions = this.broker.NumLongPositions;
            results.NumShortPositions = this.broker.NumShortPositions;
            results.NumTrades = this.broker.NumTrades;
            results.OpenPositionSize = this.broker.CurrentPositionSize;
            results.OpenPositionValue = results.OpenPositionSize * this.priceHistory[this.priceHistory.Count - 1].Close;
            results.TotalBrokerageFees = results.NumTrades * Broker.BrokerageFee;

            return results;
        }

        /// <summary>
        /// Determines whether the target price is a low swing point.
        /// </summary>
        /// <param name="price">The target price.</param>
        /// <param name="priorPrice">The prior price.</param>
        /// <param name="nextPrice">The next price.</param>
        /// <returns>
        /// <c>true</c> if the price is a low swing point; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// A low swing point is a price that is lower than it's prior and next prices.
        /// </remarks>
        private static bool IsLowSwingPoint(float price, float priorPrice, float nextPrice)
        {
            return (price <= priorPrice) && (price < nextPrice);
        }

        /// <summary>
        /// Determines whether the target price is a high swing point.
        /// </summary>
        /// <param name="price">The target price.</param>
        /// <param name="priorPrice">The prior price.</param>
        /// <param name="nextPrice">The next price.</param>
        /// <returns>
        /// <c>true</c> if the price is a high swing point; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// A high swing point is a price that is higher than it's prior and next prices.
        /// </remarks>
        private static bool IsHighSwingPoint(float price, float priorPrice, float nextPrice)
        {
            return (price >= priorPrice) && (price > nextPrice);
        }

        /// <summary>
        /// Calculates the daily direction of the stock price.
        /// </summary>
        /// <param name="current">The current day's price data.</param>
        /// <param name="previousDayIndex">Index of the previous day.  This allows the method to be used recursively.</param>
        /// <returns>
        /// The daily price direction.
        /// </returns>
        private PriceDirections CalculateDailyDirection(PriceData current, int previousDayIndex)
        {
            var previous = this.priceHistory[previousDayIndex];

            if (previous.Direction == PriceDirections.Inside)
            {
                // Inside days cannot be used for this calculation...skip to the next prior day using a recursive call.
                return this.CalculateDailyDirection(current, previousDayIndex - 1);
            }

            // UP days are days with a higher high and a higher low.
            if (current.High > previous.High && current.Low > previous.Low)
            {
                return PriceDirections.Up;
            }

            // DOWN days are days with a lower high and a lower low.
            if (current.High < previous.High && current.Low < previous.Low)
            {
                return PriceDirections.Down;
            }

            // INSIDE days are inconclusive days with lower highs and higher lows.
            // These days will be omitted from most price comparisons.
            if (current.High <= previous.High && current.Low >= previous.Low)
            {
                return PriceDirections.Inside;
            }

            // OUTSIDE days are inconclusive days with higher highs and lower lows.
            // Unlike INSIDE days, these days are not entirely inconslusive...
            // ...they just require an additional day of data to determine their true direction.
            if (current.High >= previous.High && current.Low <= previous.Low)
            {
                return PriceDirections.Outside;
            }

            // Exception case
            return PriceDirections.Uncalculated;
        }

        /// <summary>
        /// Finds the 52 week high price.
        /// </summary>
        /// <returns>
        /// The 52 week high price.
        /// </returns>
        private float Find52WeekHighPrice()
        {
            var currentDate = this.Today.IntervalOpenTime;
            var priorYearOfData = this.priceHistory.Where(data => (data.IntervalOpenTime <= currentDate) && currentDate.Subtract(data.IntervalOpenTime) < TimeSpan.FromDays(365));
            return priorYearOfData.Max(data => data.High);
        }

        /// <summary>
        /// Finds the 52 week low price.
        /// </summary>
        /// <returns>
        /// The 52 week low price.
        /// </returns>
        private float Find52WeekLowPrice()
        {
            var currentDate = this.Today.IntervalOpenTime;
            var priorYearOfData = this.priceHistory.Where(data => (data.IntervalOpenTime <= currentDate) && currentDate.Subtract(data.IntervalOpenTime) < TimeSpan.FromDays(365));
            return priorYearOfData.Min(data => data.Low);
        }

        /// <summary>
        /// Looks for short term low.
        /// </summary>
        private void LookForShortTermLow()
        {
            var middleDayIndex = this.GetFirstPriorNonInsideDayIndex(this.currentIndex);
            var priorDayIndex = this.GetFirstPriorNonInsideDayIndex(middleDayIndex);

            if (IsLowSwingPoint(this.priceHistory[middleDayIndex].Low, this.priceHistory[priorDayIndex].Low, this.Today.Low))
            {
                this.Today.ConfirmsShortTermLowSwingPoint = true;
                this.shortTermLows.Add(this.priceHistory[middleDayIndex].Low);
                this.currentWorksheetEntry.ShortTermLow = this.priceHistory[middleDayIndex].Low;

                this.LookForIntermediateLow(); 
            }
        }

        /// <summary>
        /// Looks for intermediate low.
        /// </summary>
        private void LookForIntermediateLow()
        {
            if (this.shortTermLows.Count >= 3
                && IsLowSwingPoint(
                    this.shortTermLows[this.shortTermLows.Count - 2], 
                    this.shortTermLows[this.shortTermLows.Count - 3], 
                    this.shortTermLows[this.shortTermLows.Count - 1]))
            {
                this.Today.ConfirmsIntermediateLowSwingPoint = true;
                this.intermediateLows.Add(this.shortTermLows[this.shortTermLows.Count - 2]);
                this.currentWorksheetEntry.IntermediateLow = this.shortTermLows[this.shortTermLows.Count - 2];

                this.LookForLongTermLow();
            }
        }

        /// <summary>
        /// Looks for long term low.
        /// </summary>
        private void LookForLongTermLow()
        {
            if (this.intermediateLows.Count >= 3
                && IsLowSwingPoint(
                    this.intermediateLows[this.intermediateLows.Count - 2],
                    this.intermediateLows[this.intermediateLows.Count - 3],
                    this.intermediateLows[this.intermediateLows.Count - 1]))
            {
                this.longTermLows.Add(this.intermediateLows[this.intermediateLows.Count - 2]);
                this.currentWorksheetEntry.LongTermLow = this.intermediateLows[this.intermediateLows.Count - 2];
            }
        }

        /// <summary>
        /// Looks for short term high.
        /// </summary>
        private void LookForShortTermHigh()
        {
            if (this.currentIndex >= 3
                && IsHighSwingPoint(this.Yesterday.High, this.priceHistory[this.currentIndex - 2].High, this.Today.High)
                && this.Today.Direction == PriceDirections.Down)
            {
                this.Today.ConfirmsShortTermHighSwingPoint = true;
                this.shortTermHighs.Add(this.Yesterday.High);
                this.currentWorksheetEntry.ShortTermHigh = this.Yesterday.High;

                this.LookForIntermediateHigh();
            }
        }

        /// <summary>
        /// Looks for intermediate high.
        /// </summary>
        private void LookForIntermediateHigh()
        {
            if (this.shortTermHighs.Count >= 3
                && IsHighSwingPoint(
                    this.shortTermHighs[this.shortTermHighs.Count - 2],
                    this.shortTermHighs[this.shortTermHighs.Count - 3],
                    this.shortTermHighs[this.shortTermHighs.Count - 1]))
            {
                this.Today.ConfirmsIntermediateHighSwingPoint = true;
                this.intermediateHighs.Add(this.shortTermHighs[this.shortTermHighs.Count - 2]);
                this.currentWorksheetEntry.IntermediateHigh = this.shortTermHighs[this.shortTermHighs.Count - 2];

                this.LookForLongTermHigh();
            }
        }

        /// <summary>
        /// Looks for long term high.
        /// </summary>
        private void LookForLongTermHigh()
        {
            if (this.intermediateHighs.Count >= 3
                && IsHighSwingPoint(
                    this.intermediateHighs[this.intermediateHighs.Count - 2],
                    this.intermediateHighs[this.intermediateHighs.Count - 3],
                    this.intermediateHighs[this.intermediateHighs.Count - 1]))
            {
                this.longTermHighs.Add(this.intermediateHighs[this.intermediateHighs.Count - 2]);
                this.currentWorksheetEntry.LongTermHigh = this.intermediateHighs[this.intermediateHighs.Count - 2];
            }
        }

        /// <summary>
        /// Updates the current worksheet entry.
        /// </summary>
        private void UpdateWorksheetEntry()
        {
            this.currentWorksheetEntry.Adx = this.Today.Adx;
            this.currentWorksheetEntry.AdxDirection = Enum.GetName(typeof(PriceDirections), this.Today.AdxDirection);
            this.currentWorksheetEntry.ClosePrice = this.Today.Close;
            this.currentWorksheetEntry.Date = this.Today.IntervalOpenTime.ToShortDateString();
            this.currentWorksheetEntry.FiftyTwoWeekHigh = this.current52WeekHigh;
            this.currentWorksheetEntry.FiftyTwoWeekLow = this.current52WeekLow;
            this.currentWorksheetEntry.FiftyTwoWeekPercentage =
                this.current52WeekPercentage;
            this.currentWorksheetEntry.HighPrice = this.Today.High;
            this.currentWorksheetEntry.LowPrice = this.Today.Low;
            this.currentWorksheetEntry.OpenPrice = this.Today.Open;
            this.currentWorksheetEntry.PriceDirection = Enum.GetName(typeof(PriceDirections), this.Today.Direction);
            this.currentWorksheetEntry.VolatilityRange = this.Today.VolatilityRange;
            this.currentWorksheetEntry.Volume = this.Today.Volume;
            this.currentWorksheetEntry.CurrentBalance = this.broker.CurrentBalance;
            this.currentWorksheetEntry.CurrentPositionSize =
                this.broker.CurrentPositionSize;
            this.currentWorksheetEntry.OnBalanceVolume = this.Today.OnBalanceVolume;
            this.currentWorksheetEntry.OnBalanceVolumeStrength =
                this.Today.ObvStrength;
            this.currentWorksheetEntry.Ema5 = this.Today.Ema5;
            this.currentWorksheetEntry.Ema20 = this.Today.Ema20;

            if (this.broker.CurrentPositionSize != 0)
            {
                this.currentWorksheetEntry.CurrentStop = this.currentStopPrice;
            }
        }

        /// <summary>
        /// Looks for an entrance into a new position.
        /// </summary>
        private void LookForEntrance()
        {
            // Look for an entrance into a long position.
            if (this.IsTodayALongSetup())
            {
                if (this.currentIndex == this.priceHistory.Count - 1)
                {
                    Console.WriteLine(this.ticker);
                }

                this.PlaceOrderForLongPosition((int)(1000 / (this.Today.High + 0.05f)), false);
            }

            // TODO:  Look for entrances into short positions.
        }

        /// <summary>
        /// Determines whether today is a setup for a long position.
        /// </summary>
        /// <returns>
        /// <c>true</c> if is today a long setup; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Modify this method to modify the trading strategy for long setups.
        /// </remarks>
        private bool IsTodayALongSetup()
        {
            // For this strategy, a long setup is a day that confirms a second-consecutive
            // intermediate low swing point. 
            return this.IsTodayPotentialHigherLow()
                              && this.lastIntermediHighSwingDate < this.lastIntermediateLowSwingDate
                              && IsLowSwingPoint(
                                  this.shortTermLows[this.shortTermLows.Count - 1],
                                  this.shortTermLows[this.shortTermLows.Count - 2],
                                  this.Today.Low);
        }

        /// <summary>
        /// Determines whether today is a potential higher short-term low.
        /// </summary>
        /// <returns>
        /// <c>true</c> if today is a potential higher short-term low; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Today is potentially a higher short-term low if
        /// 1) it is a down day
        /// 2) it will be a low swing day if tomorrow is an up day.
        /// 3) its low is higher than the previous short-term low.
        /// 4) It would confirm an intermediate low.
        /// </remarks>
        private bool IsTodayPotentialHigherLow()
        {
            var priorDayIndex = this.GetFirstPriorNonInsideDayIndex(this.currentIndex);
            return this.Today.Direction == PriceDirections.Down
                   && IsLowSwingPoint(this.Today.Low, this.priceHistory[priorDayIndex].Low, this.Today.Low + 1.0f)
                   && this.Today.Low > this.shortTermLows[this.shortTermLows.Count - 1];
        }

        /// <summary>
        /// Looks for pyramid increase to the current position size.
        /// </summary>
        /// <returns>
        /// True, when a new order was placed to increase the position size.
        /// </returns>
        private bool LookForPyramidIncrease()
        {
            //var nextPyramidLevel = this.GetNextPyramidLevel();

            //if (this.Today.ConfirmsIntermediateHighSwingPoint && this.broker.CurrentPositionSize < 0 && nextPyramidLevel > 0)
            //{
            //    this.PlaceOrderForShortPosition(nextPyramidLevel, true);
            //    return true;
            //}

            //if (this.Today.ConfirmsShortTermLowSwingPoint 
            //    && this.broker.CurrentPositionSize > 0
            //    && this.shortTermLows[this.shortTermLows.Count - 1] > this.shortTermLows[this.shortTermLows.Count - 2]
            //    && nextPyramidLevel > 0)
            //{
            //    this.PlaceOrderForLongPosition(nextPyramidLevel, true);
            //    return true;
            //}

            return false;
        }

        /// <summary>
        /// Places an order for a long position.
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        /// <param name="replaceCurrentStop">if set to <c>true</c> replace the current stop order.</param>
        private void PlaceOrderForLongPosition(int quantity, bool replaceCurrentStop)
        {
            var stopOrder = new Order
            {
                Id = this.broker.GetNextOrderId(),
                Action = Order.Actions.Sell,
                ActivationPrice = this.CalculateStopPriceForLongPosition(),
                GoodUntil = DateTime.Today + TimeSpan.FromDays(365),
                Position = Order.Positions.Long,
                Quantity = quantity + Math.Abs(this.broker.CurrentPositionSize),
                Type = Order.Types.StopMarket
            };

            var order = new Order
            {
                Id = this.broker.GetNextOrderId(),
                Action = Order.Actions.Buy,
                ActivationPrice = this.Today.High * 1.025f,
                GoodUntil = this.Today.IntervalOpenTime + TimeSpan.FromDays(1),
                Position = Order.Positions.Long,
                Quantity = quantity,
                Type = Order.Types.StopMarket,
                ConditionalOrder = stopOrder
            };

            if (replaceCurrentStop)
            {
                stopOrder.IsReplacementOrder = true;
                stopOrder.ReplacementId = this.GetCurrentStopOrderId();
            }

            this.currentStopPrice = stopOrder.ActivationPrice;

            this.broker.QueueOrder(order);
        }

        /// <summary>
        /// Places an order for a short position.
        /// </summary>
        /// <param name="quantity">The quantity.</param>
        /// <param name="replaceCurrentStop">if set to <c>true</c> replace the current stop order.</param>
        private void PlaceOrderForShortPosition(int quantity, bool replaceCurrentStop)
        {
            var stopOrder = new Order
            {
                Id = this.broker.GetNextOrderId(),
                Action = Order.Actions.Buy,
                ActivationPrice = this.CalculateStopPriceForShortPosition(),
                GoodUntil = DateTime.Today + TimeSpan.FromDays(365),
                Position = Order.Positions.Short,
                Quantity = quantity + Math.Abs(this.broker.CurrentPositionSize),
                Type = Order.Types.StopMarket
            };

            var order = new Order
            {
                Id = this.broker.GetNextOrderId(),
                Action = Order.Actions.Sell,
                GoodUntil = DateTime.Today + TimeSpan.FromDays(1),
                Position = Order.Positions.Short,
                Quantity = quantity,
                Type = Order.Types.Market,
                ConditionalOrder = stopOrder
            };

            if (replaceCurrentStop)
            {
                stopOrder.IsReplacementOrder = true;
                stopOrder.ReplacementId = this.GetCurrentStopOrderId();
            }

            this.currentStopPrice = stopOrder.ActivationPrice;

            this.broker.QueueOrder(order);
        }

        /// <summary>
        /// Adjusts the stop order, if necessary.
        /// </summary>
        private void AdjustStopOrder()
        {
            float adjustedStopPrice;

            if (this.broker.CurrentPositionSize > 0)
            {
                adjustedStopPrice = this.CalculateStopPriceForLongPosition();
                
                if (adjustedStopPrice > this.currentStopPrice)
                {
                    this.UpdateLongStopOrder(adjustedStopPrice);
                }
            }

            if (this.broker.CurrentPositionSize < 0)
            {
                adjustedStopPrice = this.CalculateStopPriceForShortPosition();

                if (adjustedStopPrice < this.currentStopPrice)
                {
                    this.UpdateShortStopOrder(adjustedStopPrice);
                }
            }
        }

        /// <summary>
        /// Calculates the stop price for a long position.
        /// </summary>
        /// <returns>
        /// The stop price for a long position.
        /// </returns>
        private float CalculateStopPriceForLongPosition()
        {
            return this.Today.Low - 0.01f;
        }

        /// <summary>
        /// Configures the stop price for a short position.
        /// </summary>
        /// <returns>
        /// The stop price for a short position.
        /// </returns>
        private float CalculateStopPriceForShortPosition()
        {
            var bestResistance = this.FindBestResistanceLevel();
            return bestResistance + (0.1f * this.Today.VolatilityRange);
        }

        /// <summary>
        /// Finds the best support level.
        /// </summary>
        /// <returns>
        /// The best support level.
        /// </returns>
        private float FindBestSupportLevel()
        {
            return this.shortTermLows[this.shortTermLows.Count - 1];
        }

        /// <summary>
        /// Finds the best resistance level.
        /// </summary>
        /// <returns>
        /// The best resistance level.
        /// </returns>
        private float FindBestResistanceLevel()
        {
            return this.shortTermHighs[this.shortTermHighs.Count - 1];
        }

        /// <summary>
        /// Updates the long stop order.
        /// </summary>
        /// <param name="stopPrice">The stop price.</param>
        private void UpdateLongStopOrder(float stopPrice)
        {
            var order = new Order
                {
                    Id = this.broker.GetNextOrderId(),
                    Action = Order.Actions.Sell,
                    ActivationPrice = stopPrice,
                    GoodUntil = DateTime.Today + TimeSpan.FromDays(365),
                    Position = Order.Positions.Long,
                    Quantity = Math.Abs(this.broker.CurrentPositionSize),
                    Type = Order.Types.StopMarket,
                    IsReplacementOrder = true,
                    ReplacementId = this.GetCurrentStopOrderId()
                };

            this.broker.QueueOrder(order);
            
            this.currentStopPrice = stopPrice;
        }

        /// <summary>
        /// Updates the short stop order.
        /// </summary>
        /// <param name="stopPrice">The stop price.</param>
        private void UpdateShortStopOrder(float stopPrice)
        {
            var order = new Order
                {
                    Id = this.broker.GetNextOrderId(),
                    Action = Order.Actions.Buy,
                    ActivationPrice = stopPrice,
                    GoodUntil = DateTime.Today + TimeSpan.FromDays(365),
                    Position = Order.Positions.Short,
                    Quantity = Math.Abs(this.broker.CurrentPositionSize),
                    Type = Order.Types.StopMarket,
                    IsReplacementOrder = true,
                    ReplacementId = this.GetCurrentStopOrderId()
                };

            this.broker.QueueOrder(order);

            this.currentStopPrice = stopPrice;
        }

        /// <summary>
        /// Gets the current stop order id.
        /// </summary>
        /// <returns>
        /// The ID of the current stop order.
        /// </returns>
        /// <remarks>
        /// This method assumes that only one stop order will be pending at once.
        /// </remarks>
        private int GetCurrentStopOrderId()
        {
            return this.broker.QueuedOrders.First(order => order.Type == Order.Types.StopMarket).Id;
        }

        /// <summary>
        /// Gets the size of the next pyramid level.
        /// </summary>
        /// <returns>
        /// The size of the next pyramid order.
        /// </returns>
        private int GetNextPyramidLevel()
        {
            if (this.broker.CurrentPositionSize == this.pyramidLevels[0])
            {
                return this.pyramidLevels[1];
            }

            return 0;
        }

        /// <summary>
        /// Gets the first index of the prior non-inside day.
        /// </summary>
        /// <param name="baseIndex">Base index for the comparison.</param>
        /// <returns>
        /// The first index prior to the base index that represents a non-inside day.
        /// </returns>
        private int GetFirstPriorNonInsideDayIndex(int baseIndex)
        {
            for (var index = baseIndex - 1; index >= 0; index--)
            {
                if (this.priceHistory[index].Direction != PriceDirections.Uncalculated
                    && this.priceHistory[index].Direction != PriceDirections.Inside)
                {
                    return index;
                }
            }

            return 0;  // Return the first day.
        }
    }
}
