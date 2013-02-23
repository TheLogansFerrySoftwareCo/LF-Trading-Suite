// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceData.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>04/28/2012 9:15 PM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System;

    /// <summary>
    /// Information about the price activity of a stock during a time interval.
    /// </summary>
    public class PriceData
    {
        /// <summary>
        /// The daily stock data.
        /// </summary>
        private StockDaily stockDaily;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriceData"/> class.
        /// </summary>
        /// <param name="stockDaily">The stock daily.</param>
        public PriceData(StockDaily stockDaily)
        {
            this.stockDaily = stockDaily;
        }

        /// <summary>
        /// Gets the opening timestamp of the interval.
        /// </summary>
        public DateTime IntervalOpenTime
        {
            get
            {
                return this.stockDaily.Date;
            }
        }

        /// <summary>
        /// Gets the opening price during the interval.
        /// </summary>
        public float Open
        {
            get
            {
                return this.stockDaily.OpenPrice;
            }
        }

        /// <summary>
        /// Gets the closing price during the interval.
        /// </summary>
        public float High
        {
            get
            {
                return this.stockDaily.HighPrice;
            }
        }

        /// <summary>
        /// Gets the low price during the interval.
        /// </summary>
        public float Low
        {
            get
            {
                return this.stockDaily.LowPrice;
            }
        }

        /// <summary>
        /// Gets the closing price during the interval. 
        /// </summary>
        public float Close
        {
            get
            {
                return this.stockDaily.ClosePrice;
            }
        }

        /// <summary>
        /// Gets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public int Volume
        {
            get
            {
                return this.stockDaily.Volume;
            }
        }

        /// <summary>
        /// Gets or sets the current direction of the stock's price.
        /// </summary>
        public PriceDirections Direction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a confirmation day for a prior short term low swing point.
        /// </summary>
        /// <value>
        /// <c>true</c> if this confirms a short-term low swing point; otherwise, <c>false</c>.
        /// </value>
        public bool ConfirmsShortTermLowSwingPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a confirmation day for a prior short term high swing point.
        /// </summary>
        /// <value>
        /// <c>true</c> if this confirms a short-term high swing point; otherwise, <c>false</c>.
        /// </value>
        public bool ConfirmsShortTermHighSwingPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a confirmation day for a prior intermediate low swing point.
        /// </summary>
        /// <value>
        /// <c>true</c> if this confirms an intermediate low swing point; otherwise, <c>false</c>.
        /// </value>
        public bool ConfirmsIntermediateLowSwingPoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a confirmation day for a prior intermediate high swing point.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is high swing point; otherwise, <c>false</c>.
        /// </value>
        public bool ConfirmsIntermediateHighSwingPoint { get; set; }

        /// <summary>
        /// Gets or sets the Positive Directional Movement (+DM).  Used to calculate the Average Directional Movement Index (ADX).
        /// </summary>
        /// <value>
        /// The Positive Directional Dovement (+DM).
        /// </value>
        public float PlusDm { get; set; }

        /// <summary>
        /// Gets or sets the Negative Directional Movement (-DM).  Used to calculate the Average Directional Movement Index (ADX).
        /// </summary>
        /// <value>
        /// The Negative Directional Movement (-DM).
        /// </value>
        public float MinusDm { get; set; }

        /// <summary>
        /// Gets or sets the 14-day exponential moving average of the Positive Directional Movement (+DM14).  Used to calculate the Average Directional Movement Index (ADX).
        /// </summary>
        /// <value>
        /// The 14-day exponential moving average of the Positive Directional Dovement (+DM14).
        /// </value>
        public float PlusDm14 { get; set; }

        /// <summary>
        /// Gets or sets the 14-day exponential moving average of the Negative Directional Movement (-DM14).  Used to calculate the Average Directional Movement Index (ADX).
        /// </summary>
        /// <value>
        /// The 14-day exponential moving average of the Negative Directional Movement (-DM14).
        /// </value>
        public float MinusDm14 { get; set; }

        /// <summary>
        /// Gets or sets the true range of price movement.
        /// </summary>
        /// <value>
        /// The true range.
        /// </value>
        public float TrueRange { get; set; }

        /// <summary>
        /// Gets or sets the 14-day exponential moving average of the true range.
        /// </summary>
        /// <value>
        /// The 14-day exponential moving average of the true range.
        /// </value>
        public float TrueRange14 { get; set; }

        /// <summary>
        /// Gets or sets the 14-day exponential moving average of the Positive Directional Movement (+DI14).  Used to calculate the Average Directional Movement Index (ADX).
        /// </summary>
        /// <value>
        /// The 14-day exponential moving average of the Positive Directional Indicator (+DI14).
        /// </value>
        public float PlusDi14 { get; set; }

        /// <summary>
        /// Gets or sets the 14-day exponential moving average of the Negative Directional Movement (-DI14).  Used to calculate the Average Directional Movement Index (ADX).
        /// </summary>
        /// <value>
        /// The Negative 14-day exponential moving average of the Directional Indicator (-DI14).
        /// </value>
        public float MinusDi14 { get; set; }

        /// <summary>
        /// Gets or sets the Directional Movement Index (DX).  Used to calculate the Average Directional Movement Index (ADX).
        /// </summary>
        /// <value>
        /// The Directional Indicator (DX).
        /// </value>
        public float Dx { get; set; }

        /// <summary>
        /// Gets or sets the Average Directional Movement Index. (ADX).
        /// </summary>
        /// <value>
        /// The ADX.
        /// </value>
        public float Adx { get; set; }

        /// <summary>
        /// Gets the ADX direction.
        /// </summary>
        public PriceDirections AdxDirection
        {
            get
            {
                if (this.PlusDi14 > this.MinusDi14)
                {
                    return PriceDirections.Up;
                }

                if (this.PlusDi14 < this.MinusDi14)
                {
                    return PriceDirections.Down;
                }

                // Unlikely exception case.
                return PriceDirections.Uncalculated;
            }
        }

        /// <summary>
        /// Gets the volatility range.
        /// </summary>
        /// <value>
        /// The volatility range.
        /// </value>
        public float VolatilityRange 
        {
            get
            {
                return Convert.ToSingle(this.TrueRange14) * 1.5f;
            }
        }

        /// <summary>
        /// Gets or sets the on balance volume.
        /// </summary>
        /// <value>
        /// The on balance volume.
        /// </value>
        public long OnBalanceVolume { get; set; }

        /// <summary>
        /// Gets or sets the OBV direction.
        /// </summary>
        /// <value>
        /// The OBV direction.
        /// </value>
        public PriceDirections ObvDirection { get; set; }

        /// <summary>
        /// Gets or sets the OBV strength.
        /// </summary>
        /// <value>
        /// The OBV strength.
        /// </value>
        public float ObvStrength { get; set; }

        /// <summary>
        /// Gets or sets the 5-Day Exponential Moving Average.
        /// </summary>
        /// <value>
        /// The EMA 5.
        /// </value>
        public float Ema5 { get; set; }

        /// <summary>
        /// Gets or sets the 20-Day Exponential Moving Average.
        /// </summary>
        /// <value>
        /// The EMA 20.
        /// </value>
        public float Ema20 { get; set; }

        /// <summary>
        /// Gets or sets the EMA cross-over direction.
        /// </summary>
        /// <value>
        /// The EMA cross-over direction.
        /// </value>
        public PriceDirections LastEmaCrossOverDirection { get; set; }
    }
}
