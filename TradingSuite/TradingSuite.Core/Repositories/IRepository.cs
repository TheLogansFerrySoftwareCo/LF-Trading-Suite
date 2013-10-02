// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="The Logans Ferry Software Co.">
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
    using System.Collections.Generic;

    /// <summary>
    /// A data repository that provides read/write access to the underlying data collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity exposed by this repository.</typeparam>
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds the provided entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        /// True, when the addition is successful.
        /// </returns>
        bool Add(TEntity entity);

        /// <summary>
        /// Adds the provided items to the repository.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>
        /// True, when the items are successfully added.
        /// </returns>
        bool Add(IEnumerable<TEntity> items);

        /// <summary>
        /// Updates the specified entity within the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// True, when the entity is successfully updated.
        /// </returns>
        bool Update(TEntity entity);

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// True, when the entity is deleted.
        /// </returns>
        bool Delete(TEntity entity);

        /// <summary>
        /// Deletes the specified entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>
        /// True, when the entities are deleted.
        /// </returns>
        bool Delete(IEnumerable<TEntity> entities);
    }
}
