// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Stock.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.StockData.
//   
//   TradingSuite.StockData is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.StockData is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.StockData. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite
{
    using System.Collections.Generic;

    /// <summary>
    /// A financial stock that can be traded.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// The maximum length of the Ticker field.
        /// </summary>
        public const int TickerMaxLength = 10;

        /// <summary>
        /// The maximum length of the Company Name field.
        /// </summary>
        public const int CompanyNameMaxLength = 50;

        /// <summary>
        /// The listing of this stock's historic prices.
        /// </summary>
        private readonly SortedSet<HistoricPrice> priceHistory;

        /// <summary>
        /// The stock's ticker symbol.
        /// </summary>
        private string ticker;

        /// <summary>
        /// The name of the company represented by the stock.
        /// </summary>
        private string companyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// </summary>
        public Stock()
        {
            this.priceHistory = new SortedSet<HistoricPrice>();
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the ticker symbol.
        /// </summary>
        /// <value>
        /// The ticker symbol.
        /// </value>
        public virtual string Ticker
        {
            get
            {
                return this.ticker;
            }

            set
            {
                this.ticker = 
                    value.Length <= TickerMaxLength 
                        ? value 
                        : value.Substring(0, TickerMaxLength);
            }
        }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public virtual string CompanyName
        {
            get
            {
                return this.companyName;
            }

            set
            {
                this.companyName = 
                    value.Length <= CompanyNameMaxLength 
                        ? value 
                        : value.Substring(0, CompanyNameMaxLength);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this stock is excluded from update operations.
        /// </summary>
        /// <value>
        /// <c>true</c> if this stock is excluded; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsExcluded { get; set; }

        /// <summary>
        /// Gets the stock's the price history.
        /// </summary>
        /// <value>
        /// The price history.
        /// </value>
        public virtual ICollection<HistoricPrice> PriceHistory
        {
            get
            {
                return this.priceHistory;
            }
        }

        /// <summary>
        /// Adds an historic price to the stock's price history.
        /// </summary>
        /// <param name="price">The price to add to the history.</param>
        /// <returns>True, when the price was successfully added.</returns>
        public virtual bool AddHistoricPrice(HistoricPrice price)
        {
            // Store the price's current Stock reference so the operation can be reverted on failure.
            var priorStockReference = price.Stock;

            // Try to add the price to this stock's history.
            price.Stock = this;
            var success = this.priceHistory.Add(price);

            // Revert on failure.
            if (!success)
            {
                price.Stock = priorStockReference;
            }

            return success;
        }

        /// <summary>
        /// Determines whether the specified Stock is equal to this instance.
        /// </summary>
        /// <param name="other">The other stock.</param>
        /// <returns>
        /// True, when the two are considered to be equal.
        /// </returns>
        public virtual bool Equals(Stock other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return object.Equals(other.Ticker, this.Ticker);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
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

            var other = obj as Stock;
            
            return this.Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Ticker != null ? this.Ticker.GetHashCode() : 0;
        }
    }
}