// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Entities.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.StockData.
//   
//   TradingSuite.StockData is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.StockData is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.StockData. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.NHibernate
{
    /// <summary>
    /// An organized listing of entity and field names defined in this namespace.
    /// </summary>
    /// <remarks>
    /// These values are intended to be used as table and column references in NHibernate mapping configurations.
    /// Doing so will ensure consistent mapping references by multiple models and view-models that represent the
    /// same actual data. 
    /// </remarks>
    public static class Entities
    {
        /// <summary>
        /// The Stock entity that represents one tradable financial product.
        /// </summary>
        public static class Stock
        {
            /// <summary>
            /// The entity name of the Stock entity.
            /// </summary>
            public const string Name = "Stock";

            /// <summary>
            /// A listing of field names associated with the Stock entity.
            /// </summary>
            public static class FieldNames
            {
                /// <summary>
                /// The name of the Stock entity's ID number field.
                /// </summary>
                public const string Id = "Id";

                /// <summary>
                /// The name of the Stock entity's Ticker Symbol field.
                /// </summary>
                public const string Ticker = "Ticker";

                /// <summary>
                /// The name of the Stock entity's Company Name field.
                /// </summary>
                public const string CompanyName = "CompanyName";

                /// <summary>
                /// The name of the Stock entity's Is Excluded field.
                /// </summary>
                public const string IsExcluded = "IsExcluded";
            }
        }

        /// <summary>
        /// The entity that represents an historic price for a stock.
        /// </summary>
        public static class HistoricPrice
        {
            /// <summary>
            /// The entity name of the Historic Price entity.
            /// </summary>
            public const string Name = "HistoricPrice";

            /// <summary>
            /// The name of the Index reference for this entity.
            /// </summary>
            public const string UniqueKey = "UniqueDateStockId";

            /// <summary>
            /// A listing of field names associated with the Historic Price entity.
            /// </summary>
            public static class FieldNames
            {
                /// <summary>
                /// The name of the Historic Price entity's ID number field.
                /// </summary>
                public const string Id = "Id";

                /// <summary>
                /// The name of the Historic Price entity's Stock field.
                /// </summary>
                public const string StockId = "StockId";

                /// <summary>
                /// The name of the Historic Price entity's Date field.
                /// </summary>
                public const string Date = "Date";

                /// <summary>
                /// The name of the Historic Price entity's Open field.
                /// </summary>
                public const string Open = "Open";

                /// <summary>
                /// The name of the Historic Price entity's High field.
                /// </summary>
                public const string High = "High";

                /// <summary>
                /// The name of the Historic Price entity's Low field.
                /// </summary>
                public const string Low = "Low";

                /// <summary>
                /// The name of the Historic Price entity's Close field.
                /// </summary>
                public const string Close = "Close";

                /// <summary>
                /// The name of the Historic Price entity's Volume field.
                /// </summary>
                public const string Volume = "Volume";
            }
        }
    }
}
