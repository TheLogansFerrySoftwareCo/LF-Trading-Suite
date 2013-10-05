// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoricPrice.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.Core.
//   
//   TradingSuite.Core is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.Core is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.Core. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite
{
    using System;

    /// <summary>
    /// Information about a stock's historic price on a specific day.
    /// </summary>
    public class HistoricPrice : IComparable
    {
        /// <summary>
        /// The date associated with this historic price.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public virtual long Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated stock.
        /// </summary>
        /// <value>
        /// The stock ID.
        /// </value>
        public virtual Stock Stock { get; set; }

        /// <summary>
        /// Gets or sets the associated date for this historic price.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public virtual DateTime Date
        {
            get
            {
                // Only return the date value with no timestamp component.
                return this.date.Date;
            }

            set
            {
                this.date = value;
            }
        }

        /// <summary>
        /// Gets or sets the stock's opening price.
        /// </summary>
        /// <value>
        /// The opening price.
        /// </value>
        public virtual float Open { get; set; }

        /// <summary>
        /// Gets or sets the stock's high price.
        /// </summary>
        /// <value>
        /// The high price.
        /// </value>
        public virtual float High { get; set; }

        /// <summary>
        /// Gets or sets the stock's low price.
        /// </summary>
        /// <value>
        /// The low price.
        /// </value>
        public virtual float Low { get; set; }

        /// <summary>
        /// Gets or sets the stock's closing price.
        /// </summary>
        /// <value>
        /// The closing price.
        /// </value>
        public virtual float Close { get; set; }

        /// <summary>
        /// Gets or sets the stock's trading volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public virtual long Volume { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(HistoricPrice))
            {
                return false;
            }

            return this.Equals((HistoricPrice)obj);
        }

        /// <summary>
        /// Determines whether the specified Historic Price is equal the this Historic Price.
        /// </summary>
        /// <param name="other">The other price to compare.</param>
        /// <returns>
        /// True, when the other historic price refers to the same Stock ID and Date as this one.
        /// </returns>
        public virtual bool Equals(HistoricPrice other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return (GetStockIdForComparison(other.Stock) == GetStockIdForComparison(this.Stock)) && other.Date.Equals(this.Date);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (GetStockIdForComparison(this.Stock) * 397) ^ this.Date.GetHashCode();
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        public virtual int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            var otherPrice = obj as HistoricPrice;

            if (otherPrice == null)
            {
                throw new ArgumentException("The other object is not an HistoricPrice object.", "obj");
            }

            // First, compare prices based on their Stock ID.
            var thisStockId = GetStockIdForComparison(this.Stock);
            var otherStockId = GetStockIdForComparison(otherPrice.Stock);
            var stockIdComparison = thisStockId.CompareTo(otherStockId);
            
            if (stockIdComparison != 0)
            {
                return stockIdComparison;
            }

            // If the prices are for the same stock, then compare based on date.
            return this.Date.CompareTo(otherPrice.Date);
        }

        /// <summary>
        /// Safely returns a stock ID that can be used in comparison-related logic.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>
        /// Returns the stock's ID when the stock is not null.  Otherwise, returns zero.
        /// </returns>
        private static int GetStockIdForComparison(Stock stock)
        {
            return stock != null ? stock.Id : 0;
        }
    }
}
