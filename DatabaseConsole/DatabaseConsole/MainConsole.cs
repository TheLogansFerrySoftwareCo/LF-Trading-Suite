// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainConsole.cs" company="The Logans Ferry Software Co.">
//   Copyright 2012, The Logans Ferry Software Co.
// </copyright>
// <license>  
//   This file is part of DatabaseConsole.
//   
//   DatabaseConsole is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   DatabaseConsole is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   DatabaseConsole. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.DatabaseConsole
{
    using System;
    
    /// <summary>
    /// The primary console of the application (Main Menu.)
    /// </summary>
    public class MainConsole
    {
        /// <summary>
        /// A sub-menu for managing watch lists.
        /// </summary>
        private readonly WatchListConsole watchListConsole;

        /// <summary>
        /// A sub-menu for managing stock tickers.
        /// </summary>
        private readonly StockTickerConsole stockTickerConsole;

        /// <summary>
        /// A flag that indicates whether the user has selected to exit.
        /// </summary>
        private bool isExiting;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainConsole"/> class.
        /// </summary>
        public MainConsole()
        {
            this.watchListConsole = new WatchListConsole();
            this.stockTickerConsole = new StockTickerConsole();
        }

        /// <summary>
        /// Runs the program.
        /// </summary>
        public void Run()
        {
            while (!this.isExiting)
            {
                DisplayMenu();
                this.ReadSelection();
            }
        }

        /// <summary>
        /// Displays the main program menu.
        /// </summary>
        private static void DisplayMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Main Menu");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1)  Manage Watch Lists");
            Console.WriteLine("2)  Manage Stock Tickers");
            Console.WriteLine("3) Exit");
            Console.WriteLine();
            Console.Write("-->  ");
        }

        /// <summary>
        /// Reads the user's menu selection selection.
        /// </summary>
        private void ReadSelection()
        {
            var input = Console.ReadLine();

            int menuItem;
            if (!int.TryParse(input, out menuItem))
            {
                menuItem = 0;
            }

            switch (menuItem)
            {
                case 1:
                    this.watchListConsole.Run();
                    break;

                case 2:
                    this.stockTickerConsole.Run();
                    break;

                case 3:
                    this.isExiting = true;
                    break;

                default:
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine();
                    break;
            }
        }
    }
}
