// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WatchListConsole.cs" company="The Logans Ferry Software Co.">
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
    using System.Linq;
    
    /// <summary>
    /// A console (menu) that provides options for managing watch lists.
    /// </summary>
    public class WatchListConsole
    {
        /// <summary>
        /// A flag that indicates whether the user has selected to exit.
        /// </summary>
        private bool isExiting;

        /// <summary>
        /// Runs the program.
        /// </summary>
        public void Run()
        {
            while (!this.isExiting)
            {
                this.DisplayMenu();
                this.ReadSelection();
            }
        }

        /// <summary>
        /// Displays the main program menu.
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Watch List Menu");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();
            Console.WriteLine("1) Update Price History for Watch List");
            Console.WriteLine("2) List Watch Lists");
            Console.WriteLine("3) Create Watch List");
            Console.WriteLine("4) Delete Watch List");
            Console.WriteLine("5) Print Watch List");
            Console.WriteLine("6) Exit");
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
                    this.UpdateTickersInWatchList();
                    break;

                case 2:
                    this.ListWatchLists();
                    break;

                case 4:
                    this.CreateWatchList();
                    break;

                case 5:
                    this.DeleteWatchList();
                    break;

                case 6:
                    this.PrintWatchList();
                    break;

                case 7:
                    this.isExiting = true;
                    break;

                default:
                    Console.WriteLine("Invalid Input");
                    Console.WriteLine();
                    break;
            }
        }

        /// <summary>
        /// Displays a listing of the current watch lists.
        /// </summary>
        private void DisplayWatchLists()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Current Watch Lists");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();

            using (var context = new StockScreenerEntities())
            {
                // Create a list of watch lists from the database.
                var watchLists = context.Watchlists.ToList();

                if (watchLists.Count <= 0)
                {
                    // The list is empty...no watch lists.
                    Console.WriteLine("No Current Watch Lists");
                }

                // Print each item in the list (if any).
                foreach (var list in watchLists)
                {
                    Console.WriteLine(list.ID + "\t" + list.Name);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Updates the price history for tickers in a watch list.
        /// </summary>
        private void UpdateTickersInWatchList()
        {
            // First, print a list of watch lists for the user to choose from.
            this.DisplayWatchLists();

            // Prompt the user to select the watch list that will be updated with new data.
            Console.Write("Select ID of watch list to update. ([Blank] to cancel.) -->  ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                // The user opted to cancel without entering an ID.
                Console.WriteLine();
                return;
            }

            int id;
            if (!int.TryParse(input, out id))
            {
                // Invalid Input -- non-numeric data!!!
                Console.WriteLine("Invalid input...no watch list was updated.");
                Console.WriteLine();
                return;
            }

            using (var context = new StockScreenerEntities())
            {
                var watchLists = context.Watchlists.ToList();

                if (watchLists.Count(list => list.ID == id) <= 0)
                {
                    // Invalid Input -- invalid Watch List ID!!!
                    Console.WriteLine("Invalid input...no watch list was upated.");
                    Console.WriteLine();
                }
                else
                {
                    // Valid Input -- Update the selected watch list.
                    var dataUpdater = new DataUpdater();
                    dataUpdater.UpdateTickerHistories(watchLists.First(list => list.ID == id).Stocks);
                }
            }
        }

        /// <summary>
        /// Lists the current watch lists from the database.
        /// </summary>
        private void ListWatchLists()
        {
            // Display the current watch lists.
            this.DisplayWatchLists();

            // Provide a prompt to continue back to the menu.
            Console.Write("Press 'Enter' to continue...");
            Console.ReadLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Creates a new watch list.
        /// </summary>
        private void CreateWatchList()
        {
            // First, display a list of existing watch lists.
            this.DisplayWatchLists();

            // Prompt the user to enter a name for the new watch list.
            Console.Write("Enter new watch list name ([Blank] to cancel) -->  ");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                // The user opted to cancel without entering a name.
                Console.WriteLine();
                return;
            }

            // Prompt the user to confirm the inputted name.
            Console.Write("Create watch list named '" + name + "'?  (Y/N) -->  ");
            var isConfirmed = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(isConfirmed) || !isConfirmed.ToUpper().Equals("Y"))
            {
                // The user did not confirm the name.
                Console.WriteLine("Watch list not created.");
                Console.WriteLine();
                return;
            }

            // Create a new entry in the database using the inputted name.
            using (var context = new StockScreenerEntities())
            {
                try
                {
                    context.Watchlists.AddObject(new Watchlist { Name = name });
                    context.SaveChanges();
                    Console.WriteLine("Watch list was created.");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Deletes a watch list.
        /// </summary>
        private void DeleteWatchList()
        {
            // First, display a list of available watch lists.
            this.DisplayWatchLists();

            // Prompt the user to enter the watch list that will be deleted.
            Console.Write("Select ID of watch list to delete. ([Blank] to cancel.) -->  ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                // The user opted to cancel without entering an ID.
                Console.WriteLine();
                return;
            }

            int id;
            if (!int.TryParse(input, out id))
            {
                // Invalid Input -- non-numeric data!!!
                Console.WriteLine("Invalid input...no watch list was deleted.");
                Console.WriteLine();
                return;
            }

            using (var context = new StockScreenerEntities())
            {
                try
                {
                    var watchLists = context.Watchlists.ToList();

                    if (watchLists.Count(list => list.ID == id) <= 0)
                    {
                        // Invalid Input -- Invalid watch list ID!!!
                        Console.WriteLine("Invalid input...no watch list was deleted.");
                        Console.WriteLine();
                        return;
                    }

                    // Delete the selected ID from the database.
                    var listToDelete = watchLists.First(list => list.ID == id);
                    context.Watchlists.DeleteObject(listToDelete);
                    context.SaveChanges();
                    Console.WriteLine("Watch list was deleted.");
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Prints the contents of a watch list.
        /// </summary>
        private void PrintWatchList()
        {
            // First, display a list of available watch lists.
            this.DisplayWatchLists();

            // Prompt the user to select the watch list that will be printed.
            Console.Write("Select ID of watch list to print. ([Blank] to cancel.) -->  ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                // The user opted to cancel without selecting a watch list.
                Console.WriteLine();
                return;
            }

            int id;
            if (!int.TryParse(input, out id))
            {
                // Invalid Input -- non-numeric data!!!
                Console.WriteLine("Invalid input...no watch list was printed.");
                Console.WriteLine();
                return;
            }

            using (var context = new StockScreenerEntities())
            {
                try
                {
                    var watchLists = context.Watchlists.ToList();

                    if (watchLists.Count(list => list.ID == id) <= 0)
                    {
                        // Invalid Input -- Invalid Watch List ID!!!
                        Console.WriteLine("Invalid input...no watch list was printed.");
                        Console.WriteLine();
                    }
                    else
                    {
                        // Print the selected watch list.
                        this.DisplayWatchListStocks(watchLists.First(list => list.ID == id));
                        Console.Write("Press 'Enter' to continue...");
                        Console.ReadLine();
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Displays a listing of stocks in a watch list.
        /// </summary>
        /// <param name="watchlist">The watchlist to display.</param>
        private void DisplayWatchListStocks(Watchlist watchlist)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Stocks in " + watchlist.Name);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine();

            if (watchlist.Stocks.Count <= 0)
            {
                // This watch list is empty.
                Console.WriteLine("No Stocks in Watch List");
            }

            // Print each entry in the list (if any).
            foreach (var stock in watchlist.Stocks)
            {
                Console.WriteLine(stock.Ticker);
            }

            Console.WriteLine();
        }
    }
}
