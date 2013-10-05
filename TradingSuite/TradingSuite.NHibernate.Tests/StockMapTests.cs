// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockMapTests.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.NHibernate.Tests.
//   
//   TradingSuite.NHibernate.Tests is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.NHibernate.Tests is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.NHibernate.Tests. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.NHibernate.Tests
{
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Testing;

    using global::NHibernate;

    using global::NHibernate.Cfg;

    using global::NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

    /// <summary>
    /// Verifies the NHibernate Mappings in the StockMap class.
    /// </summary>
    [TestFixture]
    public class StockMapTests
    {
        /// <summary>
        /// The StockMapping class properly configures the database.
        /// </summary>
        [Test]
        public void ValidateStockMappings()
        {
            var autoValues = new Fixture();
            var testValue = autoValues.Create<Stock>();
            testValue.AddHistoricPrice(new HistoricPrice());
            new PersistenceSpecification<Stock>(GetInMemoryDatabaseSession())
                .CheckProperty(x => x.Id, 1)
                .CheckProperty(x => x.Ticker, testValue.Ticker)
                .CheckProperty(x => x.CompanyName, testValue.CompanyName)
                .CheckProperty(x => x.IsExcluded, testValue.IsExcluded)
                .CheckComponentList(
                    x => x.PriceHistory, 
                    testValue.PriceHistory,
                    (stock, historicPrice) => stock.AddHistoricPrice(historicPrice))
                .VerifyTheMappings();
        }

        /// <summary>
        /// Gets an NHibernate session for an in-memory database.
        /// </summary>
        /// <returns>
        /// A session for an in-memory database.
        /// </returns>
        private static ISession GetInMemoryDatabaseSession()
        {
            Configuration configuration = null;
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(
                    SQLiteConfiguration.Standard 
                        .InMemory()
                        .ShowSql())
                .Mappings(
                    m => m.FluentMappings.AddFromAssemblyOf<StockMap>())
                .ExposeConfiguration(
                    cfg => configuration = cfg)
                .BuildSessionFactory();
            var session = sessionFactory.OpenSession();

            var schemaExport = new SchemaExport(configuration);
            schemaExport.Execute(true, true, false, session.Connection, null);

            return session;
        }
    }
}
