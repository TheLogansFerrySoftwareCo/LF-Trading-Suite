// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="The Logans Ferry Software Co.">
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

// ReSharper disable CheckNamespace
namespace LogansFerry.TradingSuite
// ReSharper restore CheckNamespace
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Defines application route configurations.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Conduct application route registration.
        /// </summary>
        /// <param name="routes">The route collection in which routes will be registered.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Note:  The default route is intentionally omitted.  It is defined in the Database Management Area Registration class.
        }
    }
}