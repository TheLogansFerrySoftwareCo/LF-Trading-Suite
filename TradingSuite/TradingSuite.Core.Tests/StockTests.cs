// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockTests.cs" company="The Logans Ferry Software Co.">
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

namespace LogansFerry.TradingSuite.Core.Tests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Unit tests for the Stock class.
    /// </summary>
    [TestFixture]
    public class StockTests
    {
        /// <summary>
        /// The company name property will not exceed the maximum characters.
        /// </summary>
        [Test]
        public void CompanyNameMaxLengthIsEnforced()
        {
            var shortName = new string('a', Stock.CompanyNameMaxLength - 1);
            var maxLengthName = new string('a', Stock.CompanyNameMaxLength);
            var tooLongName = new string('a', Stock.CompanyNameMaxLength + 1);

            var target = new Stock();

            // Verify that names under max length are not trimmed.
            target.CompanyName = shortName;
            Assert.AreEqual(shortName, target.CompanyName);

            // Verify that names over max length are trimmed.
            target.CompanyName = tooLongName;
            Assert.AreEqual(maxLengthName, target.CompanyName);
        }

        /// <summary>
        /// The ticker property will not exceed maximum characters.
        /// </summary>
        [Test]
        public void TickerMaxLengthIsEnforced()
        {
            var shortTicker = new string('a', Stock.TickerMaxLength - 1);
            var maxLengthTicker = new string('a', Stock.TickerMaxLength);
            var tooLongTicker = new string('a', Stock.TickerMaxLength + 1);

            var target = new Stock();

            // Verify that tickers under max length are not trimmed.
            target.Ticker = shortTicker;
            Assert.AreEqual(shortTicker, target.Ticker);

            // Verify that tickers over max length are trimmed.
            target.Ticker = tooLongTicker;
            Assert.AreEqual(maxLengthTicker, target.Ticker);
        }

        /// <summary>
        /// Tests for the Add Historic Price method.
        /// </summary>
        [TestFixture]
        public class AddHistoricPriceMethod
        {
            /// <summary>
            /// Adds a new historic price to the stock's price history when it is not pre-existing.
            /// </summary>
            [Test]
            public void AddsHistoricPriceWhenPriceIsNew()
            {
                //// SETUP

                // Test Data
                var price1 = new HistoricPrice { Date = DateTime.Parse("1/1/2000") };
                var price2 = new HistoricPrice { Date = DateTime.Parse("1/2/2000") };
                var price3 = new HistoricPrice { Date = DateTime.Parse("1/3/2000") };
                
                // Setup target
                var target = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };
                
                // EXECUTE (3 times)
                var actual1 = target.AddHistoricPrice(price1);
                var actual2 = target.AddHistoricPrice(price2);
                var actual3 = target.AddHistoricPrice(price3);

                //// VERIFY
                
                Assert.AreEqual(3, target.PriceHistory.Count);

                // Price 1
                Assert.True(actual1);
                Assert.True(target.PriceHistory.Contains(price1));
                Assert.AreSame(target, price1.Stock);

                // Price 2
                Assert.True(actual2);
                Assert.True(target.PriceHistory.Contains(price2));
                Assert.AreSame(target, price2.Stock);

                // Price 3
                Assert.True(actual3);
                Assert.True(target.PriceHistory.Contains(price3));
                Assert.AreSame(target, price2.Stock);
            }

            /// <summary>
            /// An historic price will not be added when it is pre-existing in the stock's price history.
            /// </summary>
            [Test]
            public void DoesNotAddHistoricPriceWhenPriceIsPreexisting()
            {
                //// SETUP

                // Test Data
                var price = new HistoricPrice { Date = DateTime.Parse("1/1/2000") };
                var duplicatePrice = new HistoricPrice { Date = DateTime.Parse("1/1/2000") };

                // Setup target
                var target = new Stock { Ticker = "FLWS", CompanyName = "1-800 FLOWERS.COM" };

                // EXECUTE (twice)
                var initialActual = target.AddHistoricPrice(price);
                var duplicateActual = target.AddHistoricPrice(duplicatePrice);

                //// VERIFY

                Assert.AreEqual(1, target.PriceHistory.Count);

                // Initial (Should have succeeded)
                Assert.True(initialActual);
                Assert.True(target.PriceHistory.Contains(price));
                Assert.AreSame(target, price.Stock);

                // Duplicate (Should have failed)
                Assert.False(duplicateActual);
                Assert.Null(duplicatePrice.Stock);
            }
        }
    }
}
