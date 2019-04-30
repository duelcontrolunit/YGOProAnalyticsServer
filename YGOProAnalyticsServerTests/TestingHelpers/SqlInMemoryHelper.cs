using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace YGOProAnalyticsServerTests.TestingHelpers
{
    static class SqlInMemoryHelper
    {
        public static DbContextOptions<T> SqlLiteOptions<T>()
            where T : DbContext
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            return new DbContextOptionsBuilder<T>()
                    .UseSqlite(connection)
                    .ConfigureWarnings(x => x.Ignore(RelationalEventId.QueryClientEvaluationWarning))
                    .Options;
        }
    }
}
