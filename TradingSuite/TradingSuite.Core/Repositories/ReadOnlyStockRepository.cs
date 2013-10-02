// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStockRepository.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.NHibernate.
//   
//   TradingSuite.NHibernate is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.NHibernate is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.NHibernate. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.Repositories
{
    using System;

    /// <summary>
    /// A read-only stock repository that uses NHibernate to access the data store.
    /// </summary>
    public class ReadOnlyStockRepository : IReadOnlyStockRepository
    {
        /// <summary>
        /// The read-only repository encapsulated by this class.
        /// </summary>
        private readonly IReadOnlyRepository<Stock> repository;
 
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyStockRepository"/> class.
        /// </summary>
        /// <param name="readOnlyRepository">The read-only repository.</param>
        public ReadOnlyStockRepository(IReadOnlyRepository<Stock> readOnlyRepository)
        {
            if (readOnlyRepository == null)
            {
                throw new ArgumentNullException("readOnlyRepository");
            }

            this.repository = readOnlyRepository;
        }

        /// <summary>
        /// Gets a stock by the specified ticker.
        /// </summary>
        /// <param name="ticker">The ticker to use as a selection criteria.</param>
        /// <returns>
        /// The specified stock when it exists in the repository; otherwise, NULL.
        /// </returns>
        public Stock GetByTicker(string ticker)
        {
            try
            {
                return this.repository.FindBy(stock => stock.Ticker.Equals(ticker));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
