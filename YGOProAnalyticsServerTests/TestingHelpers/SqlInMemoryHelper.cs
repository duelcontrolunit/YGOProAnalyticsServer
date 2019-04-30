using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace YGOProAnalyticsServerTests.TestingHelpers
{
    /// <summary>
    /// Provide support for testing in memory.
    /// </summary>
    static class SqlInMemoryHelper
    {
        /// <summary>
        /// SqlLite options.
        /// </summary>
        /// <typeparam name="TDbContext">DbContext</typeparam>
        /// <returns>Options</returns>
        public static DbContextOptions<TDbContext> SqlLiteOptions<TDbContext>()
            where TDbContext : DbContext
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return new DbContextOptionsBuilder<TDbContext>()
                    .UseSqlite(connection)
                    .ConfigureWarnings(x => x.Ignore(RelationalEventId.QueryClientEvaluationWarning))
                    .Options;
        }
    }
}
