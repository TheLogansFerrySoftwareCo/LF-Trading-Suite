// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateRepository.cs" company="The Logans Ferry Software Co.">
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
// <attribution>
//  This code is based a tutorial published under the Creative Commons Attribution 3.0 United States License by Bob Cravens on June 1, 2010.
//  The tutorial can be viewed at:  http://blog.bobcravens.com/2010/06/the-repository-pattern-with-linq-to-fluent-nhibernate-and-mysql/
// </attribution>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.NHibernate
{
    using System;
    using System.Collections.Generic;

    using LogansFerry.TradingSuite.Repositories;

    using global::NHibernate;

    /// <summary>
    /// A data repository that uses NHibernate to access the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity exposed by this repository.</typeparam>
    public class NHibernateRepository<TEntity> : NHibernateReadOnlyRepository<TEntity>, IRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="sessionFactory">The session factory.</param>
        public NHibernateRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        /// <summary>
        /// Adds the provided entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        /// True, when the addition is successful.
        /// </returns>
        public bool Add(TEntity entity)
        {
            try
            {
                using (var transaction = this.Session.BeginTransaction())
                {
                    this.Session.Save(entity);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds the provided items to the repository.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>
        /// True, when the items are successfully added.
        /// </returns>
        public bool Add(IEnumerable<TEntity> items)
        {
            try
            {
                using (var transaction = this.Session.BeginTransaction())
                {
                    foreach (var item in items)
                    {
                        this.Session.Save(item);
                    }

                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the specified entity within the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// True, when the entity is successfully updated.
        /// </returns>
        public bool Update(TEntity entity)
        {
            try
            {
                using (var transaction = this.Session.BeginTransaction())
                {
                    this.Session.Update(entity);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes the specified entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// True, when the entity is deleted.
        /// </returns>
        public bool Delete(TEntity entity)
        {
            try
            {
                using (var transaction = this.Session.BeginTransaction())
                {
                    this.Session.Delete(entity);
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Deletes the specified entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>
        /// True, when the entities are deleted.
        /// </returns>
        public bool Delete(IEnumerable<TEntity> entities)
        {
            try
            {
                using (var transaction = this.Session.BeginTransaction())
                {
                    foreach (var entity in entities)
                    {
                        this.Session.Delete(entity);
                    }

                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
