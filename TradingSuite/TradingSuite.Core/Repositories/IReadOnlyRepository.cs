// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyRepository.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.Core.
//   
//   TradingSuite.Core is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.Core is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.Core. If not, see http://www.gnu.org/licenses/.
// </license>
// <attribution>
//  This code is taken from a tutorial published under the Creative Commons Attribution 3.0 United States License by Bob Cravens on June 1, 2010.
//  The tutorial can be viewed at:  http://blog.bobcravens.com/2010/06/the-repository-pattern-with-linq-to-fluent-nhibernate-and-mysql/
// </attribution>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.Repositories
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// A data repository that provides read-only access to the underlying data collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity exposed by this repository.</typeparam>
    public interface IReadOnlyRepository<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// Returns queryable collection of all items from the repository.
        /// </summary>
        /// <returns>
        /// A queryable collection of all items from the repository.
        /// </returns>
        IQueryable<TEntity> All();

        /// <summary>
        /// Finds a single item in the repository using the provided expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// A single item that satisfies the expression.
        /// </returns>
        TEntity FindBy(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Finds all items in the repository that satisfy the provided expression.
        /// </summary>
        /// <param name="expression">The expression to filter with.</param>
        /// <returns>
        /// A queryable collection of all items that satisfy the expression.
        /// </returns>
        IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression);
    }
}
