// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HistoricPriceMap.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.NHibernate.
//   
//   TradingSuite.NHibernate is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.NHibernate is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.NHibernate. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.NHibernate
{
    using FluentNHibernate.Mapping;

    /// <summary>
    /// A Fluent NHibernate mapping for the Historic Price class.
    /// </summary>
    public class HistoricPriceMap : ClassMap<HistoricPrice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoricPriceMap"/> class.
        /// </summary>
        public HistoricPriceMap()
        {
            this.Table(Entities.HistoricPrice.Name);
            this.Id(x => x.Id).Column(Entities.HistoricPrice.FieldNames.Id);
            this.Map(x => x.Open).Column(Entities.HistoricPrice.FieldNames.Open);
            this.Map(x => x.High).Column(Entities.HistoricPrice.FieldNames.High);
            this.Map(x => x.Low).Column(Entities.HistoricPrice.FieldNames.Low);
            this.Map(x => x.Close).Column(Entities.HistoricPrice.FieldNames.Close);
            this.Map(x => x.Volume).Column(Entities.HistoricPrice.FieldNames.Volume);
            this.Map(x => x.Date).Column(Entities.HistoricPrice.FieldNames.Date).UniqueKey(Entities.HistoricPrice.UniqueKey);
            this.References(x => x.Stock).Column(Entities.HistoricPrice.FieldNames.StockId).Cascade.All().UniqueKey(Entities.HistoricPrice.UniqueKey);
        }
    }
}
