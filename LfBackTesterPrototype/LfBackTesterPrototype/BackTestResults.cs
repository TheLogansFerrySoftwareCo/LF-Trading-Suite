// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackTestResults.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>04/28/2012 9:19 PM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System.Collections.Generic;

    /// <summary>
    /// The results of a backtesting operation.
    /// </summary>
    public class BackTestResults
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackTestResults"/> class.
        /// </summary>
        public BackTestResults()
        {
            this.Worksheet = new List<WorksheetEntry>();
        }

        /// <summary>
        /// Gets the back-testing worksheet.
        /// </summary>
        public List<WorksheetEntry> Worksheet { get; private set; }

        /// <summary>
        /// Gets or sets the ending account balance.
        /// </summary>
        /// <value>
        /// The current account balance.
        /// </value>
        public float EndingBalance { get; set; }

        /// <summary>
        /// Gets or sets the initial account balance.
        /// </summary>
        /// <value>
        /// The initial balance.
        /// </value>
        public float InitialBalance { get; set; }

        /// <summary>
        /// Gets or sets the highest account balance.
        /// </summary>
        /// <value>
        /// The highest account balance.
        /// </value>
        public float HighestBalance { get; set; }

        /// <summary>
        /// Gets or sets the lowest account balance.
        /// </summary>
        /// <value>
        /// The lowest account balance.
        /// </value>
        public float LowestBalance { get; set; }

        /// <summary>
        /// Gets or sets the size of the position that was open at the end of the simulation.
        /// </summary>
        /// <value>
        /// The size of the open position.
        /// </value>
        public int OpenPositionSize { get; set; }

        /// <summary>
        /// Gets or sets the open position value.
        /// </summary>
        public float OpenPositionValue { get; set; }

        /// <summary>
        /// Gets or sets the number of long positions.
        /// </summary>
        /// <value>
        /// The number of long positions.
        /// </value>
        public int NumLongPositions { get; set; }

        /// <summary>
        /// Gets or sets the number of short positions.
        /// </summary>
        /// <value>
        /// The number of short positions.
        /// </value>
        public int NumShortPositions { get; set; }

        /// <summary>
        /// Gets or sets the total number of trades.
        /// </summary>
        /// <value>
        /// The number of trades.
        /// </value>
        public int NumTrades { get; set; }

        /// <summary>
        /// Gets or sets the total brokerage fees.
        /// </summary>
        public float TotalBrokerageFees { get; set; }

        /// <summary>
        /// Gets or sets the net profits from short positions.
        /// </summary>
        /// <value>
        /// The net profits from short positions.
        /// </value>
        public float NetProfitsFromShortPositions { get; set; }

        /// <summary>
        /// Gets or sets the net profits from long positions.
        /// </summary>
        /// <value>
        /// The net profits from long positions.
        /// </value>
        public float NetProfitsFromLongPositions { get; set; }

        /// <summary>
        /// Gets the net profits losses.
        /// </summary>
        public float NetProfitsLosses
        {
            get
            {
                return this.EndingBalance - this.InitialBalance;
            }
        }

        /// <summary>
        /// Gets the net unrealized profit losses.
        /// </summary>
        public float NetUnrealizedProfitLosses
        {
            get
            {
                return this.NetProfitsLosses + this.OpenPositionValue;
            }
        }
    }
}
