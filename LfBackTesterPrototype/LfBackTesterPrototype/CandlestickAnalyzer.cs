// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CandlestickAnalyzer.cs" company="The Logans Ferry Software Co.">
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

    /// <summary>
    /// A utility that specializes in identifying candlesticks.
    /// </summary>
    public class CandlestickAnalyzer
    {
        /// <summary>
        /// The precision with which price values will be compared.
        /// </summary>
        private const float Epsilon = 0.01f;

        /// <summary>
        /// The average daily range for the ticker symbol.
        /// </summary>
        private readonly float averageDailyRangeForTicker;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandlestickAnalyzer"/> class.
        /// </summary>
        /// <param name="exchange">The exchange.</param>
        /// <param name="tickerSymbol">The ticker symbol.</param>
        /// <param name="open">The open price.</param>
        /// <param name="high">The high price.</param>
        /// <param name="low">The low price.</param>
        /// <param name="close">The close price.</param>
        /// <param name="averageDailyRangeForTicker">The average daily range for the ticker.</param>
        public CandlestickAnalyzer(string exchange, string tickerSymbol, float open, float high, float low, float close, float averageDailyRangeForTicker)
        {
            this.Exchange = exchange;
            this.TickerSymbol = tickerSymbol;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;

            this.averageDailyRangeForTicker = averageDailyRangeForTicker;
        }

        /// <summary>
        /// Gets the exchange.
        /// </summary>
        public string Exchange { get; private set; }

        /// <summary>
        /// Gets the ticker symbol.
        /// </summary>
        public string TickerSymbol { get; private set; }

        /// <summary>
        /// Gets the candlestick's open price.
        /// </summary>
        public float Open { get; private set; }

        /// <summary>
        /// Gets the candlestick's high price.
        /// </summary>
        public float High { get; private set; }

        /// <summary>
        /// Gets the candlestick's low price.
        /// </summary>
        public float Low { get; private set; }

        /// <summary>
        /// Gets the candlestick's close price.
        /// </summary>
        public float Close { get; private set; }

        /// <summary>
        /// Gets the length of the candle.
        /// </summary>
        /// <value>
        /// The length of the candle.
        /// </value>
        public float CandleLength
        {
            get
            {
                return Math.Abs(this.Close - this.Open);
            }
        }

        /// <summary>
        /// Gets the total length.
        /// </summary>
        public float TotalLength
        {
            get
            {
                return this.High - this.Low;
            }
        }

        /// <summary>
        /// Gets the length of the top wick.
        /// </summary>
        /// <value>
        /// The length of the top wick.
        /// </value>
        public float TopWickLength
        {
            get
            {
                if (this.IsWhiteCandle())
                {
                    return this.High - this.Close;
                }

                return this.High - this.Open;
            }
        }

        /// <summary>
        /// Gets the length of the bottom wick.
        /// </summary>
        /// <value>
        /// The length of the bottom wick.
        /// </value>
        public float BottomWickLength
        {
            get
            {
                if (this.IsWhiteCandle())
                {
                    return this.Open - this.Low;
                }

                return this.Close - this.Low;
            }
        }

        /// <summary>
        /// Determines whether the candlestick represents a white marubozu.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the candlestick represents a white marubozu.
        /// </returns>
        /// <remarks>
        /// A white marubozu is a candlestick that is long and white, opens on the low, and closes on the high.
        /// </remarks>
        public bool IsWhiteMarubozu()
        {
            return this.IsWhiteCandle() && this.IsLongCandle() && this.IsTopShaven() && this.IsBottomShaven();
        }

        /// <summary>
        /// Determines whether the candlestick represents a closing white marubozu.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the candlestick represents a closing white marubozu.
        /// </returns>
        /// <remarks>
        /// A closing white marubozu is a candlestick that is long and white, opens near the low, and closes on the high.
        /// </remarks>
        public bool IsClosingWhiteMarubozu()
        {
            return this.IsWhiteCandle() && this.IsLongCandle() && this.IsTopShaven() && !this.IsBottomShaven();
        }

        /// <summary>
        /// Determines whether the candlestick represents an opening white marubozu.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the candlestick represents an opening white marubozu.
        /// </returns>
        /// <remarks>
        /// An opening white marubozu is a candlestick that is long and white, opens on the low, and closes near the high.
        /// </remarks>
        public bool IsOpeningWhiteMarubozu()
        {
            return this.IsWhiteCandle() && this.IsLongCandle() && !this.IsTopShaven() && this.IsBottomShaven();
        }

        public bool IsDragonflyDoji()
        {
            return (Math.Abs(this.Open - this.High) < Epsilon)
                && (Math.Abs(this.Close - this.High) < Epsilon)
                && (this.TotalLength > this.averageDailyRangeForTicker);
        }

        /// <summary>
        /// Determines whether the candlestick is a white symbol (closed higher than it opened).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the candlestick is white; otherwise, <c>false</c>.
        /// </returns>
        public bool IsWhiteCandle()
        {
            return this.Close > this.Open;
        }

        /// <summary>
        /// Determines whether the candlestick is long (the candle represents at least 90% of the overall smybol).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the candlestick is long; otherwise, <c>false</c>.
        /// </returns>
        public bool IsLongCandle()
        {
            return this.CandleLength / this.TotalLength > 0.9f  && this.CandleLength > this.averageDailyRangeForTicker;
        }

        /// <summary>
        /// Determines whether the candlestick has no top wick.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the candlestick has no top wick; otherwise, <c>false</c>.
        /// </returns>
        public bool IsTopShaven()
        {
            return this.TopWickLength < Epsilon;
        }

        /// <summary>
        /// Determines whether the candlestick has no bottom wick.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the candlestick has no bottom wick; otherwise, <c>false</c>.
        /// </returns>
        public bool IsBottomShaven()
        {
            return this.BottomWickLength < Epsilon;
        }
    }
}
