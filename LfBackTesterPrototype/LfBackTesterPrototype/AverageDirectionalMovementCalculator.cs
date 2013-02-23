// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AverageDirectionalMovementCalculator.cs" company="The Logans Ferry Software Co.">
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
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A technical analysis utility that specializes in calculating a stock's ADX indicator.
    /// </summary>
    public class AverageDirectionalMovementCalculator
    {
        /// <summary>
        /// The price history that will be used to calculate the ADX.
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
        /// Populates the historical ADX values into the provided list of price history.
        /// </summary>
        /// <param name="priceHistory">The price history that should be populated with ADX values.</param>
        public void PopulateHistoricalAdxValues(List<PriceData> priceHistory)
        {
            this.priceHistory = priceHistory;

            for (this.currentIndex = 1; this.currentIndex < this.priceHistory.Count; this.currentIndex++)
            {
                // Perform daily technical calculations.
                this.Today.PlusDm = this.GetPlusDmForToday();
                this.Today.MinusDm = this.GetMinusDmForToday();
                this.Today.TrueRange = this.GetTrueRangeForToday();
                this.Today.PlusDm14 = this.GetPlusDm14ForToday();
                this.Today.MinusDm14 = this.GetMinusDm14ForToday();
                this.Today.TrueRange14 = this.GetTrueRange14ForToday();
                this.Today.PlusDi14 = this.GetPlusDi14ForToday();
                this.Today.MinusDi14 = this.GetMinusDi14ForToday();
                this.Today.Dx = this.GetDirectionalMovementIndexForToday();
                this.Today.Adx = this.GetAdxForToday();
            }
        }

        /// <summary>
        /// Calculates the Positive Directional Movement (+DM).
        /// </summary>
        /// <returns>
        /// The +DM.
        /// </returns>
        /// <remarks>
        /// From:  http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:average_directional_index_adx:
        /// Directional movement is positive (plus) when the current high minus the prior high is greater than the prior low minus the current low. 
        /// This so-called Plus Directional Movement (+DM) then equals the current high minus the prior high, provided it is positive. 
        /// A negative value would simply be entered as zero.
        /// </remarks>
        private float GetPlusDmForToday()
        {
            var upwardMovement = this.Today.High - this.Yesterday.High;
            var downwardMovement = this.Yesterday.Low - this.Today.Low;

            // The positive Directional Movement (+DM) and negative Directional Movement (-DM) values.
            if (upwardMovement > 0 && upwardMovement > downwardMovement)
            {
                return upwardMovement;
            }

            return 0.0f;
        }

        /// <summary>
        /// Calculates the Negative Directional Movement (-DM).
        /// </summary>
        /// <returns>
        /// The -DM.
        /// </returns>
        /// <remarks>
        /// From:  http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:average_directional_index_adx
        /// Directional movement is negative (minus) when the prior low minus the current low is greater than the current high minus the prior high. 
        /// This so-called Minus Directional Movement (-DM) equals the prior low minus the current low, provided it is positive. 
        /// A negative value would simply be entered as zero.
        /// </remarks>
        private float GetMinusDmForToday()
        {
            var upwardMovement = this.Today.High - this.Yesterday.High;
            var downwardMovement = this.Yesterday.Low - this.Today.Low;

            // The positive Directional Movement (+DM) and negative Directional Movement (-DM) values.
            if (downwardMovement > 0 && downwardMovement > upwardMovement)
            {
                return downwardMovement;
            }

            return 0.0f;
        }

        /// <summary>
        /// Calculates the current True Range value.
        /// </summary>
        /// <returns>
        /// The True Range value
        /// </returns>
        /// <remarks>
        /// From http://en.wikipedia.org/wiki/Average_true_range:
        /// The true range is the largest of the:
        ///    - Most recent period's high minus the most recent period's low
        ///    - Absolute value of the most recent period's high minus the previous close
        ///    - Absolute value of the most recent period's low minus the previous close
        /// </remarks>
        private float GetTrueRangeForToday()
        {
            return Math.Max(
                    this.Today.High - this.Today.Low,
                    Math.Max(
                        Math.Abs(this.Today.High - this.Yesterday.Close),
                        Math.Abs(this.Today.Low - this.Yesterday.Close)));
        }

        /// <summary>
        /// Calculates the 14-day exponential moving average of the Positive Directional Movement (+DM).
        /// </summary>
        /// <returns>
        /// The +DM14 value.
        /// </returns>
        private float GetPlusDm14ForToday()
        {
            if (this.currentIndex < 14)
            {
                // Not enough data.
                return 0.0f;
            }

            if (this.currentIndex == 14)
            {
                var average = 0.0f;
                for (var index = 1; index <= 14; index++)
                {
                    average += this.priceHistory[index].PlusDm;
                }

                return average / 14;
            }

            return (this.Yesterday.PlusDm14 * 0.92857f) + (this.Today.PlusDm * 0.07143f);
        }

        /// <summary>
        /// Calculates the 14-day exponential moving average of the Negative Directional Movement (-DM).
        /// </summary>
        /// <returns>
        /// The -DM14 value.
        /// </returns>
        private float GetMinusDm14ForToday()
        {
            if (this.currentIndex < 14)
            {
                // Not enough data.
                return 0.0f;
            }

            if (this.currentIndex == 14)
            {
                var average = 0.0f;
                for (var index = 1; index <= 14; index++)
                {
                    average += this.priceHistory[index].MinusDm;
                }

                return average / 14;
            }

            return (this.Yesterday.MinusDm14 * 0.92857f) + (this.Today.MinusDm * 0.07143f);
        }

        /// <summary>
        /// Calculates the 14-day exponential moving average of the True Range.
        /// </summary>
        /// <returns>
        /// The TR14 value.
        /// </returns>
        private float GetTrueRange14ForToday()
        {
            if (this.currentIndex < 14)
            {
                // Not enough data.
                return 0.0f;
            }

            if (this.currentIndex == 14)
            {
                var average = 0.0f;
                for (var index = 1; index <= 14; index++)
                {
                    average += this.priceHistory[index].TrueRange;
                }

                return average / 14;
            }

            return (this.Yesterday.TrueRange14 * 0.92857f) + (this.Today.TrueRange * 0.07143f);
        }

        /// <summary>
        /// Calculates the +DI14, which is the +DM14 / TR14.
        /// </summary>
        /// <returns>
        /// The +DI14.
        /// </returns>
        private float GetPlusDi14ForToday()
        {
            if (this.currentIndex < 14)
            {
                return 0.0f;
            }

            return this.Today.PlusDm14 / this.Today.TrueRange14 * 100;
        }

        /// <summary>
        /// Calculates the -DI14, which is the -DM14 / TR14.
        /// </summary>
        /// <returns>
        /// The -DI14.
        /// </returns>
        private float GetMinusDi14ForToday()
        {
            if (this.currentIndex < 14)
            {
                return 0.0f;
            }

            return this.Today.MinusDm14 / this.Today.TrueRange14 * 100;
        }

        /// <summary>
        /// Calculates the directional movement index (DX).
        /// </summary>
        /// <returns>
        /// The DX value.
        /// </returns>
        private float GetDirectionalMovementIndexForToday()
        {
            if (this.currentIndex < 14)
            {
                return 0.0f;
            }

            return Math.Abs(this.Today.PlusDi14 - this.Today.MinusDi14) / (this.Today.PlusDi14 + this.Today.MinusDi14) * 100;
        }

        /// <summary>
        /// Calculates the Average Directional Movement Index (ADX).
        /// </summary>
        /// <returns>
        /// The calculated ADX.
        /// </returns>
        private float GetAdxForToday()
        {
            if (this.currentIndex < 28)
            {
                return 0.0f;
            }

            if (this.currentIndex == 28)
            {
                var average = 0.0f;
                for (var index = 1; index <= 14; index++)
                {
                    average += this.priceHistory[index].Dx;
                }

                return average / 14;
            }

            return (this.Yesterday.Adx * 0.92857f) + (this.Today.Dx * 0.07143f);
        }
    }
}
