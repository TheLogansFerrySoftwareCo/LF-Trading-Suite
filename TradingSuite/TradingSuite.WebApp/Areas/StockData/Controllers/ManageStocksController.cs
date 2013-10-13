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

    using LogansFerry.TradingSuite.Repositories;
    
    using MvcContrib.Pagination;

    public partial class ManageStocksController : Controller
    {
        /// <summary>
        /// The repository that will retrieve information about stocks.
        /// </summary>
        private readonly IReadOnlyStockRepository stockRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageStocksController"/> class.
        /// </summary>
        /// <param name="stockRepository">The stock repository.</param>
// ReSharper disable UnusedMember.Global -- (This constructor is used by StructureMap.)
        public ManageStocksController(IReadOnlyStockRepository stockRepository)
// ReSharper restore UnusedMember.Global
        {
            this.stockRepository = stockRepository;
        }
        //
        // GET: /StockData/ManageStocks/

        public virtual ActionResult Index(int? page)
        {
            var stocks = this.stockRepository.All().AsPagination(page ?? 1, 100);
            return View(stocks);
        }

    }
}
