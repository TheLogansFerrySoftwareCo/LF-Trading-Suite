// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyPriceHistoryRepostioryTests.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.Core.Tests.
//   
//   TradingSuite.Core.Tests is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.Core.Tests is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.Core.Tests. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.Core.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using LogansFerry.TradingSuite.Repositories;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the ReadOnlyPriceHistoryRepository class.
    /// </summary>
    [TestFixture]
    public class ReadOnlyPriceHistoryRepostioryTests
    {
        /// <summary>
        /// Unit tests for the GetMostRecentDateForTicker method.
        /// </summary>
        [TestFixture]
        public class GetMostRecentDateForTickerMethod
        {
            /// <summary>
            /// Returns the date of the most current price history entry.
            /// </summary>
            [Test]
            public void ReturnsDateOfMostRecentEntry()
            {
                //// SETUP

                // Test Data
                var stock = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };
                var oldest = new HistoricPrice { Stock = stock, Date = DateTime.Parse("1/1/2000") };
                var middle = new HistoricPrice { Stock = stock, Date = DateTime.Parse("1/2/2000") };
                var newest = new HistoricPrice { Stock = stock, Date = DateTime.Parse("1/3/2000") };
                var testData = new List<HistoricPrice> { oldest, newest, middle };

                // Create a mock generic repository.
                var mockGenericRepository = new Mock<IReadOnlyRepository<HistoricPrice>>();
                mockGenericRepository.Setup(mock => mock.FilterBy(It.IsAny<Expression<Func<HistoricPrice, bool>>>())).Returns(testData.AsQueryable());

                // Setup target
                var target = new ReadOnlyPriceHistoryRepository(mockGenericRepository.Object);

                // EXECUTE
                var actual = target.GetMostRecentDateForTicker(stock.Ticker);

                // VERIFY
                Assert.AreEqual(newest.Date, actual);
                mockGenericRepository.Verify(mock => mock.FilterBy(It.IsAny<Expression<Func<HistoricPrice, bool>>>()), Times.Once());
            }

            /// <summary>
            /// Returns the default DateTime value (DateTime.MinValue) when there are no existing price records.
            /// </summary>
            [Test]
            public void ReturnsDefaultDateWhenNoRecordsExist()
            {
                //// SETUP

                // Test Data
                var stock = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };
                
                // Create a mock generic repository.
                var mockGenericRepository = new Mock<IReadOnlyRepository<HistoricPrice>>();
                mockGenericRepository.Setup(mock => mock.FilterBy(It.IsAny<Expression<Func<HistoricPrice, bool>>>())).Returns(stock.PriceHistory.AsQueryable());

                // Setup target
                var target = new ReadOnlyPriceHistoryRepository(mockGenericRepository.Object);

                // EXECUTE
                var actual = target.GetMostRecentDateForTicker(stock.Ticker);

                // VERIFY
                Assert.AreEqual(DateTime.MinValue, actual);
                mockGenericRepository.Verify(mock => mock.FilterBy(It.IsAny<Expression<Func<HistoricPrice, bool>>>()), Times.Once());
            }
        }
    }
}
