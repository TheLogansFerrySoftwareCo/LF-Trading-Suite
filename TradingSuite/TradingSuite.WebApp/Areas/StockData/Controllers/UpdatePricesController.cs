// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdatePricesController.cs" company="The Logans Ferry Software Co.">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;

    using LogansFerry.TradingSuite.Repositories;
    using LogansFerry.TradingSuite.WebApp.Areas.StockData.Models;

    /// <summary>
    /// Home controller class for the "Database Management" area of the application.
    /// </summary>
    public partial class UpdatePricesController : Controller
    {
        /// <summary>
        /// The repository that will retrieve information about stocks.
        /// </summary>
        private readonly IStockRepository stockRepository;

        /// <summary>
        /// A list of download errors, indexed by ticker symbol.
        /// </summary>
        private readonly IDictionary<string, string> downloadErrorList; 

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePricesController"/> class.
        /// </summary>
        /// <param name="stockRepository">The stock repository.</param>
// ReSharper disable UnusedMember.Global -- (This constructor is used by StructureMap.)
        public UpdatePricesController(IStockRepository stockRepository)
// ReSharper restore UnusedMember.Global
        {
            this.stockRepository = stockRepository;
            this.downloadErrorList = new Dictionary<string, string>();

            // TODO: Remove
            this.downloadErrorList.Add("ABCDEF", "Bad download.");
        }

        /// <summary>
        /// The INDEX action for the database controller.
        /// </summary>
        /// <returns>
        /// An action result to display the Index page of the Database Management section.
        /// </returns>
        public virtual ActionResult Index()
        {
            return this.View(this.GetDownloadErrorViewModels());
        }

        /// <summary>
        /// Updates all prices.
        /// </summary>
        /// <returns>
        /// A redirect to the INDEX view.
        /// </returns>
        public virtual ActionResult UpdateAllPrices()
        {
            return this.RedirectToAction(MVC.StockData.UpdatePrices.Index());
        }

        /// <summary>
        /// An action that will flag selected stocks as excluded.
        /// </summary>
        /// <param name="tickers">The stocks to update.</param>
        /// <returns>
        /// A list of stocks that were successfully updated.
        /// </returns>
        [HttpPost]
        public virtual JsonResult FlagAsExcluded(List<string> tickers)
        {
            if (tickers == null || tickers.Count == 0)
            {
                return this.Json(null);
            }

            var updatedTickers = (from ticker in tickers 
                                  let isUpdateSuccessful = this.stockRepository.UpdateIsExcludedFlag(ticker, true) 
                                  where isUpdateSuccessful 
                                  select ticker).ToList();

            return this.Json(updatedTickers, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets a list of download error view models for the current error listing.
        /// </summary>
        /// <returns>
        /// A list of download error view models.
        /// </returns>
        private IEnumerable<DownloadErrorViewModel> GetDownloadErrorViewModels()
        {
            var downloadErrorViewModels = new List<DownloadErrorViewModel>();
            
            Mapper.CreateMap<Stock, DownloadErrorViewModel>();

            foreach (var downloadError in this.downloadErrorList)
            {
                var stock = this.stockRepository.GetByTicker(downloadError.Key);

                if (stock.IsExcluded)
                {
                    continue;
                }

                var viewModel = Mapper.Map<Stock, DownloadErrorViewModel>(stock);
                viewModel.ErrorMessage = downloadError.Value;
                downloadErrorViewModels.Add(viewModel);
            }

            return downloadErrorViewModels;
        }
    }
}
