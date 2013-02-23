// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorksheetEntry.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>04/29/2012 8:32 AM</last_updated>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    /// <summary>
    /// One entry (line) for the backtesting worksheet that will be outputed at the end of the backtest.
    /// </summary>
    public class WorksheetEntry
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the open price.
        /// </summary>
        /// <value>
        /// The open price.
        /// </value>
        public float OpenPrice { get; set; }

        /// <summary>
        /// Gets or sets the high price.
        /// </summary>
        /// <value>
        /// The high price.
        /// </value>
        public float HighPrice { get; set; }

        /// <summary>
        /// Gets or sets the low price.
        /// </summary>
        /// <value>
        /// The low price.
        /// </value>
        public float LowPrice { get; set; }

        /// <summary>
        /// Gets or sets the close price.
        /// </summary>
        /// <value>
        /// The close price.
        /// </value>
        public float ClosePrice { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public int Volume { get; set; }

        /// <summary>
        /// Gets or sets the price direction.
        /// </summary>
        /// <value>
        /// The price direction.
        /// </value>
        public string PriceDirection { get; set; }

        /// <summary>
        /// Gets or sets the adx.
        /// </summary>
        /// <value>
        /// The adx.
        /// </value>
        public float Adx { get; set; }

        /// <summary>
        /// Gets or sets the adx direction.
        /// </summary>
        /// <value>
        /// The adx direction.
        /// </value>
        public string AdxDirection { get; set; }

        /// <summary>
        /// Gets or sets the fifty two week percentage.
        /// </summary>
        /// <value>
        /// The fifty two week percentage.
        /// </value>
        public float FiftyTwoWeekPercentage { get; set; }

        /// <summary>
        /// Gets or sets the fifty two week high.
        /// </summary>
        /// <value>
        /// The fifty two week high.
        /// </value>
        public float FiftyTwoWeekHigh { get; set; }

        /// <summary>
        /// Gets or sets the fifty two week low.
        /// </summary>
        /// <value>
        /// The fifty two week low.
        /// </value>
        public float FiftyTwoWeekLow { get; set; }

        /// <summary>
        /// Gets or sets the short term low.
        /// </summary>
        /// <value>
        /// The short term low.
        /// </value>
        public float ShortTermLow { get; set; }

        /// <summary>
        /// Gets or sets the short term high.
        /// </summary>
        /// <value>
        /// The short term high.
        /// </value>
        public float ShortTermHigh { get; set; }

        /// <summary>
        /// Gets or sets the intermediate low.
        /// </summary>
        /// <value>
        /// The intermediate low.
        /// </value>
        public float IntermediateLow { get; set; }

        /// <summary>
        /// Gets or sets the intermediate high.
        /// </summary>
        /// <value>
        /// The intermediate high.
        /// </value>
        public float IntermediateHigh { get; set; }

        /// <summary>
        /// Gets or sets the long term low.
        /// </summary>
        /// <value>
        /// The long term low.
        /// </value>
        public float LongTermLow { get; set; }

        /// <summary>
        /// Gets or sets the long term high.
        /// </summary>
        /// <value>
        /// The long term high.
        /// </value>
        public float LongTermHigh { get; set; }

        /// <summary>
        /// Gets or sets the volatility range.
        /// </summary>
        /// <value>
        /// The volatility range.
        /// </value>
        public float VolatilityRange { get; set; }

        /// <summary>
        /// Gets or sets the size of the current position.
        /// </summary>
        /// <value>
        /// The size of the current position.
        /// </value>
        public int CurrentPositionSize { get; set; }

        /// <summary>
        /// Gets or sets the current balance.
        /// </summary>
        /// <value>
        /// The current balance.
        /// </value>
        public float CurrentBalance { get; set; }

        /// <summary>
        /// Gets or sets the current stop.
        /// </summary>
        /// <value>
        /// The current stop.
        /// </value>
        public float CurrentStop { get; set; }

        /// <summary>
        /// Gets or sets the on balance volume.
        /// </summary>
        /// <value>
        /// The on balance volume.
        /// </value>
        public long OnBalanceVolume { get; set; }

        /// <summary>
        /// Gets or sets the on balance volume strength.
        /// </summary>
        /// <value>
        /// The on balance volume strength.
        /// </value>
        public float OnBalanceVolumeStrength { get; set; }

        /// <summary>
        /// Gets or sets the 5-Day EMA.
        /// </summary>
        /// <value>
        /// The EMA 5.
        /// </value>
        public float Ema5 { get; set; }

        /// <summary>
        /// Gets or sets the 20-Day EMA.
        /// </summary>
        /// <value>
        /// The EMA 20.
        /// </value>
        public float Ema20 { get; set; }
    }
}
