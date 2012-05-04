// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnBalanceVolumeCalculator.cs" company="The Logans Ferry Software Co.">
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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A technical analysis utility that specializes in calculating a stock's On Balance Volume indicator.
    /// </summary>
    public class OnBalanceVolumeCalculator
    {
        /// <summary>
        /// The price history that will be used to calculate the OBV.
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
        /// Populates the historical OBV values into the provided list of price history.
        /// </summary>
        /// <param name="priceHistory">The price history that should be populated with OBV values.</param>
        public void PopulateHistoricalObvValues(List<PriceData> priceHistory)
        {
            this.priceHistory = priceHistory;
            
            // Initialize the OBV for the first day.
            this.priceHistory[0].OnBalanceVolume = this.priceHistory[0].Volume;
            this.priceHistory[0].ObvStrength = 0.0f;
            this.priceHistory[0].ObvDirection = PriceDirections.Uncalculated;

            for (this.currentIndex = 1; this.currentIndex < this.priceHistory.Count; this.currentIndex++)
            {
                // Perform daily technical calculations.
                this.Today.OnBalanceVolume = this.GetOnBalanceVolumeForToday();
                this.Today.ObvStrength = this.GetOnBalanceVolumeStrengthForToday();
                this.Today.ObvDirection = this.GetOnBalanceVolumeDirectionForToday();
            }
        }

        /// <summary>
        /// Calculates the on-balance volume.
        /// </summary>
        /// <returns>
        /// The On-Balance Volume
        /// </returns>
        private long GetOnBalanceVolumeForToday()
        {
            if (this.Today.Close > this.Yesterday.Close)
            {
                return this.Yesterday.OnBalanceVolume + (this.Today.Volume * Convert.ToInt64(this.Today.Close / this.Today.High));
            }
            
            if (this.Today.Close < this.Yesterday.Close)
            {
                return this.Yesterday.OnBalanceVolume - (this.Today.Volume * Convert.ToInt64(this.Today.Close / this.Today.Low));
            }

            return this.Yesterday.OnBalanceVolume;
        }

        /// <summary>
        /// Calculates the on-balance volume strength.
        /// </summary>
        /// <returns>
        /// The OBV strength.
        /// </returns>
        private float GetOnBalanceVolumeStrengthForToday()
        {
            return (float)(this.Today.OnBalanceVolume - this.Yesterday.OnBalanceVolume) / Math.Abs(this.Yesterday.OnBalanceVolume);
        }

        /// <summary>
        /// Calculates the on balance volume direction.
        /// </summary>
        /// <returns>
        /// The OBV direction.
        /// </returns>
        private PriceDirections GetOnBalanceVolumeDirectionForToday()
        {
            if (this.Today.ObvStrength < 0)
            {
                return PriceDirections.Down;
            }

            if (this.Today.ObvStrength > 0)
            {
                return PriceDirections.Up;
            }

            return PriceDirections.Uncalculated;
        }
    }
}
