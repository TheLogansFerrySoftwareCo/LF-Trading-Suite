// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockDataAreaRegistration.cs" company="The Logans Ferry Software Co.">
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

namespace LogansFerry.TradingSuite.WebApp.Areas.StockData
{
    using System.Web.Mvc;

    /// <summary>
    /// Registers the Stock Data area with the application.
    /// </summary>
    /// <remarks>
    /// This area is also the Default area for the application.
    /// </remarks>
    public class StockDataAreaRegistration : AreaRegistration
    {
        /// <summary>
        /// Gets the name of the area to register.
        /// </summary>
        /// <returns>The name of the area to register.</returns>
        public override string AreaName
        {
            get
            {
                return "StockData";
            }
        }

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Register the area's default route.
            context.MapRoute(
                "StockData_default",
                "StockData/{controller}/{action}/{id}",
                new { controller = MVC.StockData.UpdatePrices.Name, action = MVC.StockData.UpdatePrices.ActionNames.Index, id = UrlParameter.Optional });

            // Register the application's default route.
            context.MapRoute(
                "default",
                "{controller}/{action}/{id}",
                new { controller = MVC.StockData.UpdatePrices.Name, action = MVC.StockData.UpdatePrices.ActionNames.Index, id = UrlParameter.Optional });
        }
    }
}
