// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MovingAverageCalculator.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>05/01/2012 6:28 PM</last_updated>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System.Collections.Generic;

    /// <summary>
    /// A technical analysis utility that specializes in calculating a stock's Exponential Moving Average indicator.
    /// </summary>
    public class MovingAverageCalculator
    {
        /// <summary>
        /// The price history that will be used to calculate the EMA.
        /// </summary>
        private List<PriceData> priceHistory;

        /// <summary>
        /// The current index in the iteration of historical days.
        /// </summary>
        private int currentIndex;

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
        /// Populates the historical EMA values into the provided list of price history.
        /// </summary>
        /// <param name="priceHistory">The price history that should be populated with EMA values.</param>
        public void PopulateHistoricalEmaValues(List<PriceData> priceHistory)
        {
            this.priceHistory = priceHistory;

            for (this.currentIndex = 1; this.currentIndex < this.priceHistory.Count; this.currentIndex++)
            {
                // Perform daily technical calculations.
                this.Today.Ema5 = this.GetEma5ForToday();
                this.Today.Ema20 = this.GetEma20ForToday();
                this.Today.LastEmaCrossOverDirection = this.GetLastEmaCrossOverDirection();
            }
        }

        /// <summary>
        /// Calculates the 5-day exponential moving average.
        /// </summary>
        /// <returns>
        /// The EMA 5 value.
        /// </returns>
        private float GetEma5ForToday()
        {
            if (this.currentIndex < 5)
            {
                // Not enough data.
                return 0.0f;
            }

            if (this.currentIndex == 5)
            {
                var average = 0.0f;
                for (var index = 1; index <= 5; index++)
                {
                    average += this.priceHistory[index].Close;
                }

                return average / 5;
            }

            return (this.Yesterday.Ema5 * 0.8f) + (this.Today.Close * 0.2f);
        }

        /// <summary>
        /// Calculates the 20-day exponential moving average.
        /// </summary>
        /// <returns>
        /// The EMA 20 value.
        /// </returns>
        private float GetEma20ForToday()
        {
            if (this.currentIndex < 20)
            {
                // Not enough data.
                return 0.0f;
            }

            if (this.currentIndex == 20)
            {
                var average = 0.0f;
                for (var index = 1; index <= 20; index++)
                {
                    average += this.priceHistory[index].Close;
                }

                return average / 20;
            }

            return (this.Yesterday.Ema20 * 0.95f) + (this.Today.Close * 0.05f);
        }

        /// <summary>
        /// Calculates the last EMA cross over direction.
        /// </summary>
        /// <returns>
        /// The last EMA cross-over direction.
        /// </returns>
        private PriceDirections GetLastEmaCrossOverDirection()
        {
            if (this.Today.Ema5 > this.Today.Ema20)
            {
                return PriceDirections.Up;
            }

            if (this.Today.Ema5 < this.Today.Ema20)
            {
                return PriceDirections.Down;
            }

            return PriceDirections.Uncalculated;
        }
    }
}
