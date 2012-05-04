// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PriceDirections.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>04/29/2012 7:41 AM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    /// <summary>
    /// An enumeration of directional movements that a stock price can perform on a given day.
    /// </summary>
    public enum PriceDirections
    {
        /// <summary>
        /// The default value (prior to calculation).
        /// </summary>
        Uncalculated,

        /// <summary>
        /// The stock price achieved a higher high and a higher low.
        /// </summary>
        Up,

        /// <summary>
        /// The stock price achieved a lower high and a lower low.
        /// </summary>
        Down,

        /// <summary>
        /// The stock price achieved a lower high and a higher low.
        /// </summary>
        Inside,

        /// <summary>
        /// The stock price achieved a higher high and a lower low.
        /// </summary>
        Outside
    }
}
