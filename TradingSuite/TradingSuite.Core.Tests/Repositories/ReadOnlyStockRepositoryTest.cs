// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStockRepositoryTest.cs" company="The Logans Ferry Software Co.">
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
    using System.Linq.Expressions;

    using LogansFerry.TradingSuite.Repositories;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// A collection of unit tests for the ReadOnlyStockRepository class.
    /// </summary>
    [TestFixture]
    public class ReadOnlyStockRepositoryTest
    {
        /// <summary>
        /// Tests for the GetByTicker method.
        /// </summary>
        [TestFixture]
        public class GetByTickerMethod
        {
            /// <summary>
            /// The generic repository is queried for the stock using the ticker.
            /// </summary>
            [Test]
            public void QueriesTheRepoForTheTicker()
            {
                //// SETUP
                
                // Test Data
                var expected = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };
                
                // Create a mock generic repository.
                var mockGenericRepository = new Mock<IReadOnlyRepository<Stock>>();
                mockGenericRepository.Setup(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>())).Returns(expected);

                // Setup target
                var target = new ReadOnlyStockRepository(mockGenericRepository.Object);

                // EXECUTE
                var actual = target.GetByTicker(expected.Ticker);

                // VERIFY
                Assert.AreSame(expected, actual);
                mockGenericRepository.Verify(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>()), Times.Once());
            }

            /// <summary>
            /// Returns NULL when the generic repository throws an exception.
            /// </summary>
            [Test]
            public void ReturnsNullOnRepoException()
            {
                //// SETUP
                
                // Create a mock generic repository
                var mockGenericRepository = new Mock<IReadOnlyRepository<Stock>>();
                mockGenericRepository.Setup(
                    mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>())).Throws(new Exception());

                // Setup target
                var target = new ReadOnlyStockRepository(mockGenericRepository.Object);

                // EXECUTE
                var actual = target.GetByTicker(string.Empty);

                // VERIFY
                Assert.Null(actual);
                mockGenericRepository.Verify(mock => mock.FindBy(It.IsAny<Expression<Func<Stock, bool>>>()), Times.Once());
            }
        }
    }
}
