// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateReadOnlyRepositoryTests.cs" company="The Logans Ferry Software Co.">
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
    using System.Collections.Generic;
    using System.Linq;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Mapping;

    using LogansFerry.TradingSuite.NHibernate;

    using Moq;

    using global::NHibernate;
    using global::NHibernate.Cfg;
    using global::NHibernate.Linq;
    using global::NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the NHibernate stock repository class.
    /// </summary>
    [TestFixture]
    public class NHibernateReadOnlyRepositoryTests : NHibernateTest
    {
        /// <summary>
        /// Tests for the All method.
        /// </summary>
        [TestFixture]
        public class AllMethod
        {
            /// <summary>
            /// The method will query for all elements without using a filter.
            /// </summary>
            [Test]
            public void QueriesForAllElementsOfTypeT()
            {
                //// SETUP

                var testEntities = new List<TestEntity>
                    {
                        new TestEntity { Name = "One" },
                        new TestEntity { Name = "Two" },
                        new TestEntity { Name = "Three" }
                    };

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entities.
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var entity in testEntities)
                        {
                            session.Save(entity);
                        }

                        transaction.Commit();
                    }

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateReadOnlyRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var query = target.All();
                    
                    //// VERIFY
                    
                    var actual = query.ToList();
                    
                    Assert.AreEqual(testEntities.Count, actual.Count);
                    foreach (var entity in testEntities)
                    {
                        Assert.True(actual.Contains(entity));
                    }
                }
            }
        }

        /// <summary>
        /// Tests for the FindBy method.
        /// </summary>
        [TestFixture]
        public class FindByMethod
        {
            /// <summary>
            /// The method will query for the only element that matches a filter.
            /// </summary>
            [Test]
            public void QueriesForOnlyMatchingElement()
            {
                //// SETUP

                var elementOne = new TestEntity { Name = "One" };
                var testEntities = new List<TestEntity>
                    {
                        elementOne,
                        new TestEntity { Name = "Two" },
                        new TestEntity { Name = "Three" }
                    };

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entities.
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var entity in testEntities)
                        {
                            session.Save(entity);
                        }

                        transaction.Commit();
                    }

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateReadOnlyRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var actual = target.FindBy(x => x.Name.StartsWith("O"));

                    // VERIFY
                    Assert.AreSame(elementOne, actual);
                }
            }
            
            /// <summary>
            /// Returns null when there are no matching elements.
            /// </summary>
            [Test]
            public void ReturnsNullForNoMatches()
            {
                //// SETUP

                var testEntities = new List<TestEntity>
                    {
                        new TestEntity { Name = "One" },
                        new TestEntity { Name = "Two" },
                        new TestEntity { Name = "Three" }
                    };

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entities.
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var entity in testEntities)
                        {
                            session.Save(entity);
                        }

                        transaction.Commit();
                    }

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateReadOnlyRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var actual = target.FindBy(x => x.Name.StartsWith("X"));

                    // VERIFY
                    Assert.Null(actual);
                }
            }

            /// <summary>
            /// Returns null when there are multiple matching elements.
            /// </summary>
            [Test]
            public void ReturnsNullForMultipleMatches()
            {
                //// SETUP

                var testEntities = new List<TestEntity>
                    {
                        new TestEntity { Name = "One" },
                        new TestEntity { Name = "Two" },
                        new TestEntity { Name = "Three" }
                    };

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entities.
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var entity in testEntities)
                        {
                            session.Save(entity);
                        }

                        transaction.Commit();
                    }

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateReadOnlyRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var actual = target.FindBy(x => x.Name.StartsWith("T"));

                    // VERIFY
                    Assert.Null(actual);
                }
            }
        }

        /// <summary>
        /// Tests for the FilterBy method.
        /// </summary>
        [TestFixture]
        public class FilterByMethod
        {
            /// <summary>
            /// The method will query for all elements that match the filter.
            /// </summary>
            [Test]
            public void QueriesForAllMatchingElements()
            {
                //// SETUP

                var elementTwo = new TestEntity { Name = "Two" };
                var elementThree = new TestEntity { Name = "Three" };
                var testEntities = new List<TestEntity>
                    {
                        new TestEntity { Name = "One" },
                        elementTwo,
                        elementThree
                    };

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entities.
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var entity in testEntities)
                        {
                            session.Save(entity);
                        }

                        transaction.Commit();
                    }

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateReadOnlyRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var query = target.FilterBy(x => x.Name.StartsWith("T"));

                    //// VERIFY

                    var actual = query.ToList();

                    Assert.AreEqual(2, actual.Count);
                    Assert.True(actual.Contains(elementTwo));
                    Assert.True(actual.Contains(elementThree));
                }
            }
        }
    }
}
