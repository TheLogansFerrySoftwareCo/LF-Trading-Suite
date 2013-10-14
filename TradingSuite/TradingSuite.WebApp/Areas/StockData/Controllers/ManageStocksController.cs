// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageStocksController.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.WebApp.
//   
//   TradingSuite.WebApp is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.WebApp is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.WebApp. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.WebApp.Areas.StockData.Controllers
{
    using System.Web.Mvc;

    using LogansFerry.TradingSuite.DataFetchers.StockList;
    using LogansFerry.TradingSuite.Repositories;
    
    using MvcContrib.Pagination;

    /// <summary>
    /// Controller for the Manage Stock page within the Stock Data area.
    /// </summary>
    public partial class ManageStocksController : Controller
    {
        /// <summary>
        /// The repository that will retrieve information about stocks.
        /// </summary>
        private readonly IStockRepository stockRepository;

        /// <summary>
        /// The utility that will download a list of stocks from the web.
        /// </summary>
        private readonly IStockListFetcher stockListFetcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageStocksController"/> class.
        /// </summary>
        /// <param name="stockRepository">The stock repository.</param>
        /// <param name="stockListFetcher">The stock list fetcher.</param>
// ReSharper disable UnusedMember.Global -- (This constructor is used by StructureMap.)
        public ManageStocksController(IStockRepository stockRepository, IStockListFetcher stockListFetcher)
// ReSharper restore UnusedMember.Global
        {
            this.stockRepository = stockRepository;
            this.stockListFetcher = stockListFetcher;
        }

        /// <summary>
        /// Action for the Index page.  Displays a paginated list of stocks from the DB.
        /// </summary>
        /// <param name="page">The page number of paginated stock data to display.</param>
        /// <returns>
        /// The Index view.
        /// </returns>
        public virtual ActionResult Index(int? page)
        {
            var stocks = this.stockRepository.All().AsPagination(page ?? 1, 100);
            return this.View(stocks);
        }

        /// <summary>
        /// An AJAX action that will update the database with a current list of stocks.
        /// </summary>
        /// <returns>
        /// The number of stocks added during the update operation.
        /// </returns>
        [HttpPost]
        public virtual JsonResult UpdateStocks()
        {
            var stocksList = this.stockListFetcher.FetchStockList();
            var numUpdated = this.stockRepository.TryAddNewStocks(stocksList);
            return this.Json(numUpdated, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// An action that will flag selected stocks as excluded.
        /// </summary>
        /// <param name="ticker">The ticker of the stock to update.</param>
        /// <param name="isExcluded">The new "Is Excluded" value.</param>
        /// <returns>
        /// The status of the update operation.
        /// </returns>
        [HttpPost]
        public virtual JsonResult UpdateIsExcluded(string ticker, bool isExcluded)
        {
            if (string.IsNullOrEmpty(ticker))
            {
                return this.Json(null);
            }

            return this.Json(this.stockRepository.UpdateIsExcludedFlag(ticker, isExcluded), JsonRequestBehavior.AllowGet);
        }
    }
}
