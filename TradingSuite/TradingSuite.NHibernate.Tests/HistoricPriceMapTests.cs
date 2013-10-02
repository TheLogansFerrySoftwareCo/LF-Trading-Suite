// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoricPriceMapTests.cs" company="The Logans Ferry Software Co.">
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
    /// Verifies the NHibernate Mappings in the HistoricPriceMap class.
    /// </summary>
    [TestFixture]
    public class HistoricPriceMapTests
    {
        /// <summary>
        /// The HistoricPriceMap class properly configures the database.
        /// </summary>
        [Test]
        public void ValidateHistoricPriceMappings()
        {
            var autoValues = new Fixture();
            var testValue = autoValues.Create<HistoricPrice>();
            new PersistenceSpecification<HistoricPrice>(GetInMemoryDatabaseSession())
                .CheckProperty(x => x.Id, 1L)
                .CheckProperty(x => x.Date, testValue.Date)
                .CheckProperty(x => x.Open, testValue.Open)
                .CheckProperty(x => x.High, testValue.High)
                .CheckProperty(x => x.Low, testValue.Low)
                .CheckProperty(x => x.Close, testValue.Close)
                .CheckProperty(x => x.Volume, testValue.Volume)
                .CheckReference(
                    x => x.Stock, 
                    autoValues.Create<Stock>(),
                    (historicPrice, stock) => stock.AddHistoricPrice(historicPrice))
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
                    m => m.FluentMappings.AddFromAssemblyOf<HistoricPriceMap>())
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
