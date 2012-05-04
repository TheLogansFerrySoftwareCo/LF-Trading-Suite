// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="The Logans Ferry Software Co.">
//   Copyright 2012, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of LfBackTesterPrototype.
//   
//   LfBackTesterPrototype is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   LfBackTesterPrototype is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   LfBackTesterPrototype. If not, see http://www.gnu.org/licenses/.
// </license>
// <author>Aaron Morris</author>
// <last_updated>04/28/2012 9:11 PM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System;

    /// <summary>
    /// The main program. 
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main method.
        /// </summary>
        public static void Main()
        {
            // IMPORTANT:  Update this constant value to the ticker symbol that you wish to run a back-test on.
            // TODO:  Make this a command-line argument when the application is no longer used from Visual Studio.
            const string Ticker = "Dvr";
            
            const string OutputFile = Ticker + "Worksheet.csv";
            const float InitialAccountSize = 5000f;

            var dataSource = new GoogleCsvDataSource();
            var history = dataSource.GetDataFromGoogle(Ticker);

            var broker = new Broker(InitialAccountSize);
            var backTester = new BackTester(history, broker, new AverageDirectionalMovementCalculator(), new OnBalanceVolumeCalculator(), new MovingAverageCalculator());
            var results = backTester.RunBackTest();

            var writer = new WorksheetWriter();
            writer.WriteWorksheetToCsv(OutputFile, results.Worksheet);

            System.Diagnostics.Process.Start(OutputFile);

            Console.WriteLine("Initial Balance:  $" + results.InitialBalance);
            Console.WriteLine("Ending Balance:  $" + results.EndingBalance);
            Console.WriteLine("Highest Balance:  $" + results.HighestBalance);
            Console.WriteLine("Lowest Balance:  $" + results.LowestBalance);
            Console.WriteLine();
            Console.WriteLine("Total Long Positions:   " + results.NumLongPositions);
            Console.WriteLine("Total Short Positions:  " + results.NumShortPositions);
            Console.WriteLine("Total Positions:  " + (results.NumLongPositions + results.NumShortPositions));
            Console.WriteLine();
            Console.WriteLine("Net from Longs:  $" + results.NetProfitsFromLongPositions);
            Console.WriteLine("Net from Shorts:  $" + results.NetProfitsFromShortPositions);
            Console.WriteLine("Total Brokerage Fees:  $" + results.TotalBrokerageFees);
            Console.WriteLine("Total Net:  $" + results.NetProfitsLosses);
            Console.WriteLine();
            Console.WriteLine("Open Position:  " + results.OpenPositionSize);
            Console.WriteLine("Open Position Value:  $" + results.OpenPositionValue);
            Console.WriteLine();
            Console.WriteLine("Total Gains (Realized & Unrealized):  $" + results.NetUnrealizedProfitLosses);
            Console.ReadLine();
        }
    }
}
