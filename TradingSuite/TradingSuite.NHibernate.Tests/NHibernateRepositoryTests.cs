// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateRepositoryTests.cs" company="The Logans Ferry Software Co.">
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

    using Moq;

    using global::NHibernate;
    using global::NHibernate.Linq;
    
    using NUnit.Framework;

    /// <summary>
    /// Tests for the NHibernateRepository class.
    /// </summary>
    [TestFixture]
    public class NHibernateRepositoryTests : NHibernateTest
    {
        /// <summary>
        /// Tests for the Add method.
        /// </summary>
        [TestFixture]
        public class AddMethod
        {
            /// <summary>
            /// The method will add a single element to the repository.
            /// </summary>
            [Test]
            public void AddSingleElementToTheRepository()
            {
                //// SETUP

                var testEntity = new TestEntity { Name = "One" };

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var result = target.Add(testEntity);

                    // VERIFY
                    var query = session.Query<TestEntity>();
                    Assert.True(result);
                    Assert.True(query.Contains(testEntity));
                    Assert.AreEqual(1, query.Count());
                }
            }
            
            /// <summary>
            /// The method will add a range of elements to the repository.
            /// </summary>
            [Test]
            public void AddsArrayOfElementsToTheRepository()
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
                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var result = target.Add(testEntities);

                    //// VERIFY

                    var query = session.Query<TestEntity>();

                    Assert.True(result);
                    Assert.AreEqual(testEntities.Count, query.Count());
                    foreach (var entity in testEntities)
                    {
                        Assert.True(query.Contains(entity));
                    }
                }
            }

            /// <summary>
            /// Returns false when the insertion into the repository fails.
            /// </summary>
            [Test]
            public void ReturnsFalseOnFailure()
            {
                using (var session = GetInMemoryDatabaseSession())
                {
                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var resultSingle = target.Add((TestEntity)null);
                    var resultListing = target.Add(new List<TestEntity> { (TestEntity)null });

                    //// VERIFY

                    var query = session.Query<TestEntity>();

                    Assert.False(resultSingle);
                    Assert.False(resultListing);
                    Assert.AreEqual(0, query.Count());
                }
            }
        }

        /// <summary>
        /// Tests for the Update method.
        /// </summary>
        [TestFixture]
        public class UpdateMethod
        {
            /// <summary>
            /// The method will update changes made to an element.
            /// </summary>
            [Test]
            public void UpdatesAnElement()
            {
                //// SETUP

                var testEntity = new TestEntity { Name = "Name" };
                const string UpdatedName = "New Name";

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entity.
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(testEntity);
                        transaction.Commit();
                    }

                    // Change the entity name.
                    testEntity.Name = UpdatedName;

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var result = target.Update(testEntity);

                    // VERIFY
                    session.Evict(testEntity);
                    var updatedEntity = session.Get<TestEntity>(testEntity.Id);
                    Assert.True(result);
                    Assert.AreEqual(UpdatedName, updatedEntity.Name);
                }
            }
            
            /// <summary>
            /// The method will return false when it fails to update the element.
            /// </summary>
            [Test]
            public void ReturnsFalseOnFailure()
            {
                //// SETUP

                const string OriginalName = "Name";
                const string UpdatedName = "New Name";
                var testEntity = new TestEntity { Name = OriginalName };
                
                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entity.
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(testEntity);
                        transaction.Commit();
                    }

                    // Change the entity name and ID (to an invalid ID).
                    var validId = testEntity.Id;
                    testEntity.Id++;
                    testEntity.Name = UpdatedName;

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var result = target.Update(testEntity);

                    // VERIFY
                    session.Evict(testEntity);
                    var updatedEntity = session.Get<TestEntity>(validId);
                    Assert.False(result);
                    Assert.AreEqual(OriginalName, updatedEntity.Name);
                }
            }
        }

        /// <summary>
        /// Tests for the Delete method.
        /// </summary>
        [TestFixture]
        public class DeleteMethod
        {
            /// <summary>
            /// The method will delete a single element from the repository.
            /// </summary>
            [Test]
            public void DeletesSingleElementFromTheRepository()
            {
                //// SETUP

                var testEntity = new TestEntity { Name = "One" };

                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add the test entity.
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(testEntity);
                        transaction.Commit();
                    }

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var result = target.Delete(testEntity);

                    // VERIFY
                    var query = session.Query<TestEntity>();
                    Assert.True(result);
                    Assert.False(query.Contains(testEntity));
                    Assert.AreEqual(0, query.Count());
                }
            }

            /// <summary>
            /// The method will delete a range of elements from the repository.
            /// </summary>
            [Test]
            public void AddsArrayOfElementsToTheRepository()
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
                    // Add the test entity.
                    using (var transaction = session.BeginTransaction())
                    {
                        foreach (var testEntity in testEntities)
                        {
                            session.Save(testEntity);
                        }
                        
                        transaction.Commit();
                    }
                    
                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var result = target.Delete(testEntities);

                    //// VERIFY

                    var query = session.Query<TestEntity>();

                    Assert.True(result);
                    Assert.AreEqual(0, query.Count());
                    foreach (var entity in testEntities)
                    {
                        Assert.False(query.Contains(entity));
                    }
                }
            }

            /// <summary>
            /// Returns false when the deletion from the repository fails.
            /// </summary>
            [Test]
            public void ReturnsFalseOnFailure()
            {
                using (var session = GetInMemoryDatabaseSession())
                {
                    // Add a test entity.
                    using (var transaction = session.BeginTransaction())
                    {
                        session.Save(new TestEntity());
                        transaction.Commit();
                    }

                    // Mock session factory that will provide the in-memory session.
                    var mockSessionFactory = new Mock<ISessionFactory>();
                    mockSessionFactory.Setup(mock => mock.OpenSession()).Returns(session);

                    // Setup target
                    var target = new NHibernateRepository<TestEntity>(mockSessionFactory.Object);

                    // EXECUTE
                    var resultSingle = target.Delete((TestEntity)null);
                    var resultListing = target.Delete(new List<TestEntity> { (TestEntity)null });

                    //// VERIFY

                    var query = session.Query<TestEntity>();

                    Assert.False(resultSingle);
                    Assert.False(resultListing);
                    Assert.AreEqual(1, query.Count());
                }
            }
        }
    }
}
