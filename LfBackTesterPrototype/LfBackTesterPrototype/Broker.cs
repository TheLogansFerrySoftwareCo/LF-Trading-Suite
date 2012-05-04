// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Broker.cs" company="The Logans Ferry Software Co.">
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
// <last_updated>04/29/2012 12:22 PM</last_updated>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.BackTesterPrototype
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A class that functions as a stock broker for the purposes of the back-test.
    /// </summary>
    public class Broker
    {
        /// <summary>
        /// The standard brokerage fee applied to all processed orders.
        /// </summary>
        public const float BrokerageFee = 9.99f;

        /// <summary>
        /// A list of orders that have been queued for processing.
        /// </summary>
        private readonly List<Order> queuedOrders;

        /// <summary>
        /// The last ID issued for an order.
        /// </summary>
        private int lastOrderId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Broker"/> class.
        /// </summary>
        /// <param name="initialBalance">The initial balance.</param>
        public Broker(float initialBalance)
        {
            this.queuedOrders = new List<Order>();
            this.InitialBalance = initialBalance;
            this.CurrentBalance = this.InitialBalance;
            this.LowestBalance = this.InitialBalance;
            this.HighestBalance = this.InitialBalance;
        }

        /// <summary>
        /// Gets the current account balance.
        /// </summary>
        /// <value>
        /// The current account balance.
        /// </value>
        public float CurrentBalance { get; private set; }

        /// <summary>
        /// Gets the initial account balance.
        /// </summary>
        /// <value>
        /// The initial balance.
        /// </value>
        public float InitialBalance { get; private set; }

        /// <summary>
        /// Gets the highest account balance.
        /// </summary>
        /// <value>
        /// The highest account balance.
        /// </value>
        public float HighestBalance { get; private set; }

        /// <summary>
        /// Gets the lowest account balance.
        /// </summary>
        /// <value>
        /// The lowest account balance.
        /// </value>
        public float LowestBalance { get; private set; }

        /// <summary>
        /// Gets the size of the current position.
        /// </summary>
        /// <value>
        /// The size of the current position.
        /// </value>
        public int CurrentPositionSize { get; private set; }

        /// <summary>
        /// Gets the num long positions.
        /// </summary>
        /// <value>
        /// The num long positions.
        /// </value>
        public int NumLongPositions { get; private set; }

        /// <summary>
        /// Gets the num short positions.
        /// </summary>
        /// <value>
        /// The num short positions.
        /// </value>
        public int NumShortPositions { get; private set; }

        /// <summary>
        /// Gets the number of trades.
        /// </summary>
        /// <value>
        /// The number of trades.
        /// </value>
        public int NumTrades { get; private set; }

        /// <summary>
        /// Gets the net profits from short positions.
        /// </summary>
        /// <value>
        /// The net profits from short positions.
        /// </value>
        public float NetProfitsFromShortPositions { get; private set; }

        /// <summary>
        /// Gets the net profits from long positions.
        /// </summary>
        /// <value>
        /// The net profits from long positions.
        /// </value>
        public float NetProfitsFromLongPositions { get; private set; }

        /// <summary>
        /// Gets a list of the queued orders.
        /// </summary>
        public IEnumerable<Order> QueuedOrders
        {
            get
            {
                return this.queuedOrders;
            }
        }

        /// <summary>
        /// Queues an order for processing.
        /// </summary>
        /// <param name="order">
        /// The order to queue.
        /// </param>
        public void QueueOrder(Order order)
        {
            order.IsPending = true;
            this.queuedOrders.Add(order);

            if (order.IsReplacementOrder)
            {
                // Cancel the order that this order is intended to replace.
                this.CancelOrder(order.ReplacementId);
            }
        }

        /// <summary>
        /// Cancels the specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel</param>
        public void CancelOrder(int orderId)
        {
            this.queuedOrders.RemoveAll(order => order.Id == orderId);
        }

        /// <summary>
        /// Processes the orders for today.
        /// </summary>
        /// <param name="currentPriceData">The current price data.</param>
        public void ProcessOrdersForToday(PriceData currentPriceData)
        {
            // First, process the pending market orders at the open price for the day.
            var marketOrders = this.queuedOrders.Where(order => order.Type == Order.Types.Market).ToList();
            this.ProcessMarketOrders(marketOrders, currentPriceData.Open);
            
            // Next, procecess the stop orders based on the high/low for the day.
            var stopOrders = this.queuedOrders.Where(order => order.Type == Order.Types.StopMarket).ToList();
            this.ProcessStopOrders(stopOrders, currentPriceData.Low, currentPriceData.High);

            //// Update the Highest/Lowest account balance metrics.
            
            if (this.CurrentBalance > this.HighestBalance)
            {
                this.HighestBalance = this.CurrentBalance;
            }

            if (this.CurrentBalance < this.LowestBalance)
            {
                this.LowestBalance = this.CurrentBalance;
            }
            
            // Remove all orders that have been processes.
            this.queuedOrders.RemoveAll(order => !order.IsPending);
        }

        /// <summary>
        /// Gets the next order ID.
        /// </summary>
        /// <returns>
        /// The next Order ID.
        /// </returns>
        public int GetNextOrderId()
        {
            this.lastOrderId++;
            return this.lastOrderId;
        }

        /// <summary>
        /// Processes the pending market orders at the open price for the day.
        /// </summary>
        /// <param name="marketOrders">The market orders.</param>
        /// <param name="openPrice">The open price.</param>
        private void ProcessMarketOrders(IEnumerable<Order> marketOrders, float openPrice)
        {
            foreach (var order in marketOrders)
            {
                if (order.Type != Order.Types.Market)
                {
                    continue;
                }

                switch (order.Action)
                {
                    case Order.Actions.Sell:
                        this.ProcessSellOrder(order, openPrice);
                        break;

                    case Order.Actions.Buy:
                        this.ProcessBuyOrder(order, openPrice);
                        break;
                }

                order.IsPending = false;
            }
        }

        /// <summary>
        /// Processes the open stop orders by checking each one's activation price.
        /// </summary>
        /// <param name="stopOrders">The stop orders.</param>
        /// <param name="lowPrice">The low price.</param>
        /// <param name="highPrice">The high price.</param>
        private void ProcessStopOrders(IEnumerable<Order> stopOrders, float lowPrice, float highPrice)
        {
            foreach (var order in stopOrders)
            {
                if (order.Type != Order.Types.StopMarket)
                {
                    continue;
                }

                switch (order.Action)
                {
                    case Order.Actions.Sell:
                        if (lowPrice <= order.ActivationPrice)
                        {
                            this.ProcessSellOrder(order, order.ActivationPrice);
                            order.IsPending = false;
                        }

                        break;

                    case Order.Actions.Buy:
                        if (highPrice >= order.ActivationPrice)
                        {
                            this.ProcessBuyOrder(order, order.ActivationPrice);
                            order.IsPending = false;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Processes a buy order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="price">The price.</param>
        private void ProcessBuyOrder(Order order, float price)
        {
            var cost = order.Quantity * price;

            if (cost > this.CurrentBalance)
            {
                // Stop processing this order...it will be canceled.
                return;
            }

            this.CurrentBalance -= cost;
            this.CurrentBalance -= BrokerageFee;
            this.CurrentPositionSize += order.Quantity;
            this.NumTrades++;

            if (order.Position == Order.Positions.Long)
            {
                this.NumLongPositions++;
                this.NetProfitsFromLongPositions -= cost;
            }
            else
            {
                this.NetProfitsFromShortPositions -= cost;
            }

            if (order.ConditionalOrder != null)
            {
                // This order has been successfully processed.
                // So, queue the conditional order.
                this.QueueOrder(order.ConditionalOrder);
            }
        }

        /// <summary>
        /// Processes a sell order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="price">The price.</param>
        private void ProcessSellOrder(Order order, float price)
        {
            var proceeds = order.Quantity * price;
            this.CurrentBalance += proceeds;
            this.CurrentBalance -= BrokerageFee;
            this.CurrentPositionSize -= order.Quantity;
            this.NumTrades++;

            if (order.Position == Order.Positions.Short)
            {
                this.NumShortPositions++;
                this.NetProfitsFromShortPositions += proceeds;
            }
            else
            {
                this.NetProfitsFromLongPositions += proceeds;
            }

            if (order.ConditionalOrder != null)
            {
                // This order has been successfully processed.
                // So, queue the conditional order.
               this.QueueOrder(order.ConditionalOrder);
            }
        }
    }
}
