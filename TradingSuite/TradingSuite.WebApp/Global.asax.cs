// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="The Logans Ferry Software Co.">
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

namespace LogansFerry.TradingSuite.WebApp
{
    using System;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using log4net;

    using LogansFerry.TradingSuite.WebApp.Areas.StockData.Models.Repositories;

    using NHibernate;
    using global::NHibernate;
    using global::NHibernate.Tool.hbm2ddl;

    using Npgsql;

    using StructureMap;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// Root class of the application.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// The logging utility.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MvcApplication));

        /// <summary>
        /// The NHibernate session factory that will be used throughout the website.
        /// </summary>
        private ISessionFactory sessionFactory;

        /// <summary>
        /// Entry point for the application.
        /// </summary>
        protected void Application_Start()
        {
            this.ConfigureDatabase();
            this.ConfigureStructureMap();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Configure the application database.
        /// </summary>
        private void ConfigureDatabase()
        {
           var configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(c => c
                    .Host("localhost")
                    .Port(5432)
                    .Database("StockData")
                    .Username("TradingSuite")
                    .Password("password")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<StockMap>());

            try
            {
                configuration.ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true));
            }
            catch (NpgsqlException ex)
            {
                Logger.Error(ex);
                Logger.Warn("An error occurred updating the database.  Attempting to re-create instead.");

                configuration.ExposeConfiguration(cfg => new SchemaExport(cfg).Execute(false, true, false));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }

            this.sessionFactory = configuration.BuildSessionFactory();
        }

        /// <summary>   
        /// Configures the Structure Map object factory.
        /// </summary>
        private void ConfigureStructureMap()
        {
            ObjectFactory.Configure(cfg => cfg.For<ITickerRepository>().Use<DummyTickerRepository>());
            ObjectFactory.Configure(cfg => cfg.For<ISessionFactory>().Use(this.sessionFactory));
        }
    }
}