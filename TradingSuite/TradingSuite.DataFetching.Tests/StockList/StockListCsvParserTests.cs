// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockListCsvParserTests.cs" company="The Logans Ferry Software Co.">
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

    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the StockListCsvParser.
    /// </summary>
    [TestFixture]
    public class StockListCsvParserTests
    {
        /// <summary>
        /// Unit tests for the ParseCsvData method.
        /// </summary>
        [TestFixture]
        public class ParseCsvDataMethod
        {
            /// <summary>
            /// The parser will parse CSV data that uses lower case header fields.
            /// </summary>
            [Test]
            public void ParsesCsvDataWithLowerCaseHeader()
            {
                // Setup Test Data
                var dataWithLowerCaseHeaders = 
                    "symbol,name,lastsale,marketcap,adr tso,ipoyear,sector,industry,summary quote" + Environment.NewLine
                    + "FLWS,\"1-800 FLOWERS.COM, Inc.\",4.64,300378353,n/a,1999,Consumer Services,Other Specialty Stores,http://www.nasdaq.com/symbol/flws" + Environment.NewLine
                    + "FCTY,\"1st Century Bancshares, Inc.\",5.3,48325272.8,n/a,n/a,Finance,Major Banks,http://www.nasdaq.com/symbol/fcty" + Environment.NewLine
                    + "FCCY,1st Constitution Bancorp (NJ),8.9899,53297727.91,n/a,n/a,Finance,Savings Institutions,http://www.nasdaq.com/symbol/fccy" + Environment.NewLine;
                    
                // Setup expected results
                var expected = new List<Stock>
                    {
                        new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM Inc." },
                        new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares Inc." },
                        new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" }
                    };

                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute
                var actual = target.ParseCsvData(dataWithLowerCaseHeaders);

                // Verify: Actual results exactly match expected results.
                Assert.AreEqual(expected.Count, actual.Count);                        // Verify # of elements
                for (var index = 0; index < actual.Count; index++)
                {
                    var expectedElement = expected[index];
                    var actualElement = actual[index];

                    Assert.AreEqual(expectedElement.Ticker, actualElement.Ticker);              // Verify ticker symbol
                    Assert.AreEqual(expectedElement.CompanyName, actualElement.CompanyName);    // Verify company name.
                    Assert.False(actualElement.IsExcluded);                                     // Verify excluded flag is not initialized.
                    Assert.AreEqual(0, actualElement.Id);                                       // Verify ID # is not initialized.
                }
            }

            /// <summary>
            /// The parser will parse CSV data that uses upper case header fields.
            /// </summary>
            [Test]
            public void ParsesCsvDataWithUpperCaseHeader()
            {
                // Setup Test Data
                var dataWithUpperCaseHeaders =
                    "SYMBOL,NAME,LASTSALE,MARKETCAP,ADR TSO,IPOYEAR,SECTOR,INDUSTRY,SUMMARY QUOTE" + Environment.NewLine
                    + "FLWS,\"1-800 FLOWERS.COM, Inc.\",4.64,300378353,n/a,1999,Consumer Services,Other Specialty Stores,http://www.nasdaq.com/symbol/flws" + Environment.NewLine
                    + "FCTY,\"1st Century Bancshares, Inc.\",5.3,48325272.8,n/a,n/a,Finance,Major Banks,http://www.nasdaq.com/symbol/fcty" + Environment.NewLine
                    + "FCCY,1st Constitution Bancorp (NJ),8.9899,53297727.91,n/a,n/a,Finance,Savings Institutions,http://www.nasdaq.com/symbol/fccy" + Environment.NewLine;

                // Setup expected results
                var expected = new List<Stock>
                    {
                        new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM Inc." },
                        new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares Inc." },
                        new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" }
                    };

                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute
                var actual = target.ParseCsvData(dataWithUpperCaseHeaders);

                // Verify: Actual results exactly match expected results.
                Assert.AreEqual(expected.Count, actual.Count);                        // Verify # of elements
                for (var index = 0; index < actual.Count; index++)
                {
                    var expectedElement = expected[index];
                    var actualElement = actual[index];

                    Assert.AreEqual(expectedElement.Ticker, actualElement.Ticker);              // Verify ticker symbol
                    Assert.AreEqual(expectedElement.CompanyName, actualElement.CompanyName);    // Verify company name.
                    Assert.False(actualElement.IsExcluded);                                     // Verify excluded flag is not initialized.
                    Assert.AreEqual(0, actualElement.Id);                                       // Verify ID # is not initialized.
                }
            }

            /// <summary>
            /// The parser will parse CSV data that uses camel case header fields.
            /// </summary>
            [Test]
            public void ParsesCsvDataWithCamelCaseHeader()
            {
                // Setup Test Data
                var dataWithCamelCaseHeaders =
                    "Symbol,Name,LastSale,MarketCap,ADR TSO,IPOyear,Sector,Industry,Summary Quote" + Environment.NewLine
                    + "FLWS,\"1-800 FLOWERS.COM, Inc.\",4.64,300378353,n/a,1999,Consumer Services,Other Specialty Stores,http://www.nasdaq.com/symbol/flws" + Environment.NewLine
                    + "FCTY,\"1st Century Bancshares, Inc.\",5.3,48325272.8,n/a,n/a,Finance,Major Banks,http://www.nasdaq.com/symbol/fcty" + Environment.NewLine
                    + "FCCY,1st Constitution Bancorp (NJ),8.9899,53297727.91,n/a,n/a,Finance,Savings Institutions,http://www.nasdaq.com/symbol/fccy" + Environment.NewLine;

                // Setup expected results
                var expected = new List<Stock>
                    {
                        new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM Inc." },
                        new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares Inc." },
                        new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" }
                    };

                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute
                var actual = target.ParseCsvData(dataWithCamelCaseHeaders);

                // Verify: Actual results exactly match expected results.
                Assert.AreEqual(expected.Count, actual.Count);                        // Verify # of elements
                for (var index = 0; index < actual.Count; index++)
                {
                    var expectedElement = expected[index];
                    var actualElement = actual[index];

                    Assert.AreEqual(expectedElement.Ticker, actualElement.Ticker);              // Verify ticker symbol
                    Assert.AreEqual(expectedElement.CompanyName, actualElement.CompanyName);    // Verify company name.
                    Assert.False(actualElement.IsExcluded);                                     // Verify excluded flag is not initialized.
                    Assert.AreEqual(0, actualElement.Id);                                       // Verify ID # is not initialized.
                }
            }

            /// <summary>
            /// The parser will parse CSV data with columns in any order.
            /// </summary>
            [Test]
            public void ParsesCsvDataWithReorderedColumns()
            {
                // Setup Test Data
                var dataWithReorderedColumns =
                    "LastSale,MarketCap,ADR TSO,IPOyear,Sector,industry,Company,Ticker,Summary Quote" + Environment.NewLine
                    + "4.64,300378353,n/a,1999,Consumer Services,Other Specialty Stores,\"1-800 FLOWERS.COM, Inc.\",FLWS,http://www.nasdaq.com/symbol/flws" + Environment.NewLine
                    + "5.3,48325272.8,n/a,n/a,Finance,Major Banks,\"1st Century Bancshares, Inc.\",FCTY,http://www.nasdaq.com/symbol/fcty" + Environment.NewLine
                    + "8.9899,53297727.91,n/a,n/a,Finance,Savings Institutions,1st Constitution Bancorp (NJ),FCCY,http://www.nasdaq.com/symbol/fccy" + Environment.NewLine;

                // Setup expected results
                var expected = new List<Stock>
                    {
                        new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM Inc." },
                        new Stock { Ticker = "FCTY", CompanyName = "1st Century Bancshares Inc." },
                        new Stock { Ticker = "FCCY", CompanyName = "1st Constitution Bancorp (NJ)" }
                    };

                // Setup target
                var target = new StockListCsvParser("Ticker", "Company");

                // Execute
                var actual = target.ParseCsvData(dataWithReorderedColumns);

                // Verify: Actual results exactly match expected results.
                Assert.AreEqual(expected.Count, actual.Count);                        // Verify # of elements
                for (var index = 0; index < actual.Count; index++)
                {
                    var expectedElement = expected[index];
                    var actualElement = actual[index];

                    Assert.AreEqual(expectedElement.Ticker, actualElement.Ticker);              // Verify ticker symbol
                    Assert.AreEqual(expectedElement.CompanyName, actualElement.CompanyName);    // Verify company name.
                    Assert.False(actualElement.IsExcluded);                                     // Verify excluded flag is not initialized.
                    Assert.AreEqual(0, actualElement.Id);                                       // Verify ID # is not initialized.
                }
            }

            /// <summary>
            /// Throws an exception when the NAME column is missing.
            /// </summary>
            [Test]
            [ExpectedException(typeof(DataFetchFailureException))]
            public void ThrowsDataFetchExceptionForMissingNameColumn()
            {
                // Setup Test Data
                var dataWithMissingSymbols =
                    "LastSale,MarketCap,ADR TSO,IPOyear,Sector,industry,Symbol,Summary Quote" + Environment.NewLine
                    + "4.64,300378353,n/a,1999,Consumer Services,Other Specialty Stores,FLWS,http://www.nasdaq.com/symbol/flws" + Environment.NewLine
                    + "5.3,48325272.8,n/a,n/a,Finance,Major Banks,FCTY,http://www.nasdaq.com/symbol/fcty" + Environment.NewLine
                    + "8.9899,53297727.91,n/a,n/a,FCCY,http://www.nasdaq.com/symbol/fccy" + Environment.NewLine;

                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute -- should throw exception.
                target.ParseCsvData(dataWithMissingSymbols);
            }

            /// <summary>
            /// Throws an exception when the SYMBOL column is missing.
            /// </summary>
            [Test]
            [ExpectedException(typeof(DataFetchFailureException))]
            public void ThrowsDataFetchExceptionForMissingSymbolColumn()
            {
                // Setup Test Data
                var dataWithMissingSymbols =
                    "LastSale,MarketCap,ADR TSO,IPOyear,Sector,industry,Name,Summary Quote" + Environment.NewLine
                    + "4.64,300378353,n/a,1999,Consumer Services,Other Specialty Stores,\"1-800 FLOWERS.COM, Inc.\",http://www.nasdaq.com/symbol/flws" + Environment.NewLine
                    + "5.3,48325272.8,n/a,n/a,Finance,Major Banks,\"1st Century Bancshares, Inc.\",http://www.nasdaq.com/symbol/fcty" + Environment.NewLine
                    + "8.9899,53297727.91,n/a,n/a,Finance,Savings Institutions,1st Constitution Bancorp (NJ),http://www.nasdaq.com/symbol/fccy" + Environment.NewLine;

                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute -- should throw exception.
                target.ParseCsvData(dataWithMissingSymbols);
            }

            /// <summary>
            /// Throws an exception when no data rows are provided.
            /// </summary>
            [Test]
            [ExpectedException(typeof(DataFetchFailureException))]
            public void ThrowsDataFetchExceptionForNoDataRows()
            {
                // Setup Test Data
                var dataWithMissingSymbols =
                    "Symbol,Name,LastSale,MarketCap,ADR TSO,IPOyear,Sector,Industry,Summary Quote" + Environment.NewLine;

                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute -- should throw exception.
                target.ParseCsvData(dataWithMissingSymbols);
            }

            /// <summary>
            /// Throws an exception when non-CSV data is provided.
            /// </summary>
            [Test]
            [ExpectedException(typeof(DataFetchFailureException))]
            public void ThrowsDataFetchExceptionForNonCsvData()
            {
                // Setup Test Data
                var nonCsvData =
                    "<!doctype html>" + Environment.NewLine
                    + "<html lang=\"en-us\">" + Environment.NewLine
                    + "<head>" + Environment.NewLine
                    + "<script type=\"text/javascript\"></script" + Environment.NewLine
                    + "</head>"
                    + "<body>"
                    + "<div></div>"
                    + "</body>"
                    + "</html>";

                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute -- should throw exception.
                target.ParseCsvData(nonCsvData);
            }

            /// <summary>
            /// Throws an exception when an empty string is provided.
            /// </summary>
            [Test]
            [ExpectedException(typeof(DataFetchFailureException))]
            public void ThrowsDataFetchExceptionForEmptyString()
            {
                // Setup target
                var target = new StockListCsvParser("Symbol", "Name");

                // Execute -- should throw exception.
                target.ParseCsvData(string.Empty);
            }
        }
    }
}
