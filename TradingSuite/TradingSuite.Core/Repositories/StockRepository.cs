// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockRepository.cs" company="The Logans Ferry Software Co.">
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A read/write stock repository that uses NHibernate to persist data.
    /// </summary>
    public class StockRepository : ReadOnlyStockRepository, IStockRepository
    {
        /// <summary>
        /// The repository encapsulated by this repository.
        /// </summary>
        private readonly IRepository<Stock> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockRepository"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public StockRepository(IRepository<Stock> repository) : base(repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        /// <summary>
        /// Tries to add the provided stocks to the repository.  Only new stocks will be added, and existing stocks will be skipped.
        /// </summary>
        /// <param name="stocksToAdd">The list of stocks to add.</param>
        /// <returns>
        /// The number of stocks that were successfully added.
        /// </returns>
        public int TryAddNewStocks(IEnumerable<Stock> stocksToAdd)
        {
            var newStocks = (from stock in stocksToAdd
                              let existingStock = this.GetByTicker(stock.Ticker)
                              where existingStock == null
                              select stock).ToList();

            if (newStocks.Count > 0)
            {
                this.repository.Add(newStocks);
            }
            
            return newStocks.Count;
        }
    }
}
