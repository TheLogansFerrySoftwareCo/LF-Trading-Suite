// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StockMap.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.WebApp.
//   
//   TradingSuite.WebApp is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.WebApp is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.WebApp. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.NHibernate
{
    using FluentNHibernate.Mapping;

    /// <summary>
    /// A Fluent NHibernate mapping for the Stock class.
    /// </summary>
    public class StockMap : ClassMap<Stock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockMap"/> class.
        /// </summary>
        public StockMap()
        {
            this.Table(Entities.Stock.Name);
            this.Id(x => x.Id).Column(Entities.Stock.FieldNames.Id);
            this.Map(x => x.Ticker).Column(Entities.Stock.FieldNames.Ticker).Length(Stock.TickerMaxLength);
            this.Map(x => x.CompanyName).Column(Entities.Stock.FieldNames.CompanyName).Length(Stock.CompanyNameMaxLength);
            this.Map(x => x.IsExcluded).Column(Entities.Stock.FieldNames.IsExcluded);
            this.HasMany(x => x.PriceHistory).KeyColumn(Entities.HistoricPrice.FieldNames.StockId).AsSet().LazyLoad().Inverse().Cascade.All();
        }
    }
}