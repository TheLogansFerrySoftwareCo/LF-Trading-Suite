// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Order.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>04/29/2012 11:39 AM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    /// <summary>
    /// A transaction order that will be placed with the Broker.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// A listing of positions that can be assumed.
        /// </summary>
        public enum Positions
        {
            /// <summary>
            /// The order is related to a long position.
            /// </summary>
            Long,

            /// <summary>
            /// The order is related to a short position.
            /// </summary>
            Short
        }

        /// <summary>
        /// A listing of actions that can be achieved through orders.
        /// </summary>
        public enum Actions
        {
            /// <summary>
            /// Buy stock shares to open a long position or close a short position.
            /// </summary>
            Buy,

            /// <summary>
            /// Sell stock shares to close a long position or open a short position.
            /// </summary>
            Sell
        }

        /// <summary>
        /// Types of orders that can be filled.
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// Execute the order at the current market price.
            /// </summary>
            Market,

            /// <summary>
            /// Execute the order as a market order after the activation price is hit.
            /// </summary>
            StopMarket
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public Actions Action { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Types Type { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Positions Position { get; set; }

        /// <summary>
        /// Gets or sets the Order ID.
        /// </summary>
        /// <value>
        /// The Order ID.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the quantity of shares to buy/sell.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the activation price for stop orders.
        /// </summary>
        /// <value>
        /// The activation price.
        /// </value>
        public float ActivationPrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order is pending.
        /// </summary>
        /// <value>
        /// <c>true</c> if the order is pending; otherwise, <c>false</c>.
        /// </value>
        public bool IsPending { get; set; }

        /// <summary>
        /// Gets or sets the conditional order that will be executed if this order is executed.
        /// </summary>
        /// <value>
        /// The conditional order.
        /// </value>
        public Order ConditionalOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this order is a replacement for another order.
        /// </summary>
        /// <value>
        /// <c>true</c> if this order is a replacement order; otherwise, <c>false</c>.
        /// </value>
        public bool IsReplacementOrder { get; set; }

        /// <summary>
        /// Gets or sets the ID of the order that will be replaced by this one, if this is a replacement order.
        /// </summary>
        /// <value>
        /// The replacement ID.
        /// </value>
        public int ReplacementId { get; set; }
    }
}
