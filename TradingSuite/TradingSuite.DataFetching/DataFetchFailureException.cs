// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataFetchFailureException.cs" company="The Logans Ferry Software Co.">
//   Copyright 2013, The Logans Ferry Software Co. 
// </copyright>
// <license>  
//   This file is part of TradingSuite.Data.
//   
//   TradingSuite.Data is free software: you can redistribute it and/or modify it under the terms
//   of the GNU General Public License as published by the Free Software Foundation, either
//   version 3 of the License, or (at your option) any later version.
//   
//   TradingSuite.Data is distributed in the hope that it will be useful, but WITHOUT ANY
//   WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
//   A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//   
//   You should have received a copy of the GNU General Public License along with
//   TradingSuite.Data. If not, see http://www.gnu.org/licenses/.
// </license>
// --------------------------------------------------------------------------------------------------------------------

namespace LogansFerry.TradingSuite.DataFetchers
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception that is raised when fetched data is not in an expected format.
    /// </summary>
    public class DataFetchFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataFetchFailureException"/> class.
        /// </summary>
        public DataFetchFailureException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFetchFailureException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DataFetchFailureException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFetchFailureException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        public DataFetchFailureException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFetchFailureException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public DataFetchFailureException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
