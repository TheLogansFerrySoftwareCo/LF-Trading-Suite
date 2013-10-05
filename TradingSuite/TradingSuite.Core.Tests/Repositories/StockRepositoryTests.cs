// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockRepositoryTests.cs" company="The Logans Ferry Software Co.">
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

namespace LogansFerry.TradingSuite.Core.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using LogansFerry.TradingSuite;
    using LogansFerry.TradingSuite.Repositories;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the NHibernate stock repository class.
    /// </summary>
    [TestFixture]
    public class StockRepositoryTests
    {
        /// <summary>
        /// Tests for the TryAddNewStocks method.
        /// </summary>
        [TestFixture]
        public class TryAddNewStocksMethod
        {
            /// <summary>
            /// The repository will add all symbols when all are new.
            /// </summary>
            [Test]
            public void AddsAllSymbolsWhenNoneExist()
            {
                //// SETUP
                
                // Test Data
                var stock1 = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };
                var stock2 = new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares" };
                var stock3 = new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" };
                var expectedResults = new List<Stock> { stock1, stock2, stock3 }; 
                
                // Create a mock generic repository.
                var mockGenericRepository = new Mock<IRepository<Stock>>();
                mockGenericRepository.Setup(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>())).Returns((Stock)null);

                // Setup target
                var target = new StockRepository(mockGenericRepository.Object);

                // EXECUTE
                var actualCountAdded = target.TryAddNewStocks(expectedResults);

                // VERIFY
                Assert.AreEqual(expectedResults.Count, actualCountAdded);
                mockGenericRepository.Verify(mock => mock.Add(expectedResults), Times.Once());
                mockGenericRepository.Verify(mock => mock.Add(It.IsAny<Stock>()), Times.Never());
            }

            /// <summary>
            /// The repository will only add new symbols when some are pre-existing.
            /// </summary>
            [Test]
            public void AddsOnlyNewSymbolsWhenSomeExist()
            {
                //// SETUP
                
                //// Test Data
                
                var stock1 = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };
                var stock2 = new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares" };
                var stock3 = new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" };
                var stock4 = new Stock { Ticker = "SRCE", CompanyName = "1st Source Corporation" };

                var preexistingRecords = new List<Stock> { stock1, stock2 };
                var newRecords = new List<Stock> { stock3, stock4 };

                var testData = new List<Stock>();
                testData.AddRange(preexistingRecords);
                testData.AddRange(newRecords);

                // Create a mock generic repository.
                var mockGenericRepository = new Mock<IRepository<Stock>>();

                var numFindByCalls = 0;
                mockGenericRepository.Setup(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>())).Returns(() =>
                    {
                        numFindByCalls++;

                        switch (numFindByCalls)
                        {
                            case 1:
                                // Stock 1 is preexisting
                                 return stock1;

                            case 2:
                                // Stock 2 is preexisting
                                return stock2;

                            case 3:
                                // Stock 3 does not exist
                                return (Stock)null;

                            case 4:
                                // Stock 4 does not exist
                                return (Stock)null;

                            default:
                                throw new InvalidOperationException("Too many calls to mock.FindBy.");
                        }
                    });

                // Setup target
                var target = new StockRepository(mockGenericRepository.Object);

                // EXECUTE
                var actualCountAdded = target.TryAddNewStocks(testData);

                // VERIFY
                Assert.AreEqual(newRecords.Count, actualCountAdded);
                mockGenericRepository.Verify(mock => mock.Add(newRecords));
                mockGenericRepository.Verify(mock => mock.Add(It.IsAny<Stock>()), Times.Never());
                mockGenericRepository.Verify(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>()), Times.Exactly(4));
            }

            /// <summary>
            /// The repository will not add any symbols when all are pre-existing.
            /// </summary>
            [Test]
            public void AddsNoSymbolsWhenAllExist()
            {
                //// SETUP
                
                // Create two record sets that are identical
                var stock1 = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };
                var stock2 = new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares" };
                var stock3 = new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" };
                var preexistingRecords = new List<Stock> { stock1, stock2, stock3 };

                // Create a mock generic repository.
                var mockGenericRepository = new Mock<IRepository<Stock>>();

                var numFindByCalls = 0;
                mockGenericRepository.Setup(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>())).Returns(() =>
                {
                    numFindByCalls++;

                    switch (numFindByCalls)
                    {
                        case 1:
                            // Stock 1 is preexisting
                            return stock1;

                        case 2:
                            // Stock 2 is preexisting
                            return stock2;

                        case 3:
                            // Stock 3 does not exist
                            return stock3;

                        default:
                            throw new InvalidOperationException("Too many calls to mock.FindBy.");
                    }
                });

                // Setup target
                var target = new StockRepository(mockGenericRepository.Object);

                // EXECUTE
                var actualCountAdded = target.TryAddNewStocks(preexistingRecords);

                // VERIFY
                Assert.AreEqual(0, actualCountAdded);
                mockGenericRepository.Verify(mock => mock.Add(It.IsAny<Stock>()), Times.Never());
                mockGenericRepository.Verify(mock => mock.Add(It.IsAny<IEnumerable<Stock>>()), Times.Never());
                mockGenericRepository.Verify(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>()), Times.Exactly(3));
            }
        }
    }
}
