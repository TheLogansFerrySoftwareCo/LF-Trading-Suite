// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NHibernateReadOnlyRepository.cs" company="The Logans Ferry Software Co.">
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
    using System.Linq;
    using System.Linq.Expressions;

    using LogansFerry.TradingSuite.Repositories;

    using global::NHibernate;
    using global::NHibernate.Linq;

    /// <summary>
    /// A read-only data repository that uses NHibernate to access the data store.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity exposed by the repository.</typeparam>
    public class NHibernateReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// The utility that is used to create NHibernate data sessions.
        /// </summary>
        private readonly ISessionFactory sessionFactory;

        /// <summary>
        /// The NHibernate data session.
        /// </summary>
        private ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateReadOnlyRepository&lt;TEntity&gt;"/> class.
        /// </summary>
        /// <param name="sessionFactory">The session factory.</param>
        public NHibernateReadOnlyRepository(ISessionFactory sessionFactory)
        {
            if (sessionFactory == null)
            {
                throw new ArgumentNullException("sessionFactory");
            }

            this.sessionFactory = sessionFactory;
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        protected ISession Session
        {
            get
            {
                return this.session ?? (this.session = this.sessionFactory.OpenSession());
            }
        }

        /// <summary>
        /// Returns queryable collection of all items from the repository.
        /// </summary>
        /// <returns>
        /// A queryable collection of all items from the repository.
        /// </returns>
        public IQueryable<TEntity> All()
        {
            return this.Session.Query<TEntity>();
        }

        /// <summary>
        /// Finds a single item in the repository using the provided expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>
        /// A single item that satisfies the expression.
        /// </returns>
        public TEntity FindBy(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return this.FilterBy(expression).Single();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Finds all items in the repository that satisfy the provided expression.
        /// </summary>
        /// <param name="expression">The expression to filter with.</param>
        /// <returns>
        /// A queryable collection of all items that satisfy the expression.
        /// </returns>
        public IQueryable<TEntity> FilterBy(Expression<Func<TEntity, bool>> expression)
        {
            return this.Session.Query<TEntity>().Where(expression);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.session != null)
                {
                    this.session.Dispose();
                }
            }
        }
    }
}
