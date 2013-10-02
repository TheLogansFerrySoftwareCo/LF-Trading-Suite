// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateTest.cs" company="The Logans Ferry Software Co.">
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
    using FluentNHibernate.Mapping;

    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NHibernate.Tool.hbm2ddl;

    /// <summary>
    /// A base class that provides support functionality for the NHibernate tests.
    /// </summary>
    public abstract class NHibernateTest
    {
        /// <summary>
        /// Gets an NHibernate session for an in-memory database.
        /// </summary>
        /// <returns>
        /// A session for an in-memory database.
        /// </returns>
        protected static ISession GetInMemoryDatabaseSession()
        {
            Configuration configuration = null;
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(
                    SQLiteConfiguration.Standard
                        .InMemory()
                        .ShowSql())
                .Mappings(
                    m => m.FluentMappings.Add<TestEntityMap>())
                .ExposeConfiguration(
                    cfg => configuration = cfg)
                .BuildSessionFactory();
            var session = sessionFactory.OpenSession();

            var schemaExport = new SchemaExport(configuration);
            schemaExport.Execute(true, true, false, session.Connection, null);

            return session;
        }
    }

    /// <summary>
    /// An entity that will be used to unit test the repository API.
    /// </summary>
    public class TestEntity
    {
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }
    }

    /// <summary>
    /// A Fluent NHibernate mapping for the Test Entity.
    /// </summary>
    public class TestEntityMap : ClassMap<TestEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestEntityMap"/> class.
        /// </summary>
        public TestEntityMap()
        {
            this.Id(x => x.Id);
            this.Map(x => x.Name);
        }
    }
}
