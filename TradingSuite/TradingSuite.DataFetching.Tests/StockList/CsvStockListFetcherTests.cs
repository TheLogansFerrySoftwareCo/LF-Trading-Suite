// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockListFetcherTests.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.DataFetchers.Tests.
//   
//   TradingSuite.DataFetchers.Tests is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.DataFetchers.Tests is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.DataFetchers.Tests. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.DataFetchers.Tests.StockList
{
    using System;
    using System.Collections.Generic;

    using LogansFerry.TradingSuite.DataFetchers.StockList;

    using Moq;

    using NUnit.Framework;

    /// <summary>
    /// A collection of unit tests for the CSV Stock List Fetcher class.
    /// </summary>
    [TestFixture]
    public class CsvStockListFetcherTests
    {
        /// <summary>
        /// Tests for the class constructor.
        /// </summary>
        public class Constructor
        {
            /// <summary>
            /// A ArgumentNullException is thrown for the CSV Downloader argument.
            /// </summary>
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionForCsvDownloader()
            {
                var mockParser = new Mock<IStockListCsvParser>();
                new CsvStockListFetcher(null, mockParser.Object);
            }

            /// <summary>
            /// A ArgumentNullException is thrown for the CSV Parser argument.
            /// </summary>
            [Test]
            [ExpectedException(typeof(ArgumentNullException))]
            public void ThrowsArgumentNullExceptionForCsvParser()
            {
                var mockDownloader = new Mock<IStockListCsvDownloader>();
                new CsvStockListFetcher(mockDownloader.Object, null);
            }
        }

        /// <summary>
        /// Tests for the FetchStockList method.
        /// </summary>
        public class FetchStockListMethod
        {
            /// <summary>
            /// CSV data is downloaded and parsed into a stock list.
            /// </summary>
            [Test]
            public void DownloadsAndParsesData()
            {
                // Setup test values.
                const string FakeCsvData = "FakeCsvData";
                var expectedResults = new List<Stock>
                    {
                        new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM Inc." },
                        new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares Inc." },
                        new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" }
                    };

                // Setup mocks
                var mockCsvDownloader = new Mock<IStockListCsvDownloader>();
                mockCsvDownloader.Setup(mock => mock.DowloadCsvData()).Returns(FakeCsvData);
                var mockCsvParser = new Mock<IStockListCsvParser>();
                mockCsvParser.Setup(mock => mock.ParseCsvData(FakeCsvData)).Returns(expectedResults);

                // Setup target
                var target = new CsvStockListFetcher(mockCsvDownloader.Object, mockCsvParser.Object);

                // Execute
                var actual = target.FetchStockList();

                // Verify
                Assert.AreSame(expectedResults, actual);
                mockCsvDownloader.Verify(mock => mock.DowloadCsvData(), Times.Once());
                mockCsvParser.Verify(mock => mock.ParseCsvData(FakeCsvData), Times.Once());
            }
        }
    }
}
