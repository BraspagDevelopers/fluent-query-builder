using System.Text;

namespace Braspag.FluentQueryBuilder
{
    public class SelectBuilder
    {
        private readonly WhereBuilder _whereBuilder;

        private readonly StringBuilder _builder;

        public SelectBuilder()
        {
            _builder = new StringBuilder();
            _whereBuilder = new WhereBuilder(this, _builder);
        }

        /// <summary>
        /// Adds the SELECT statement to Query.
        /// </summary>
        /// <param name="fields">The fields, comma separated, that the query will return</param>              
        public SelectBuilder Select(string fields)
        {
            _builder.Append($"SELECT {fields}");
            return this;
        }

        /// <summary>
        /// Adds the SELECT statement to Query.
        /// </summary>
        /// <param name="fields">Array with the fields that the query will return</param>
        public SelectBuilder Select(string[] fields)
        {
            string f = string.Join(",", fields);
            return Select(f);
        }

        /// <summary>
        /// Adds the FROM of the query.
        /// </summary>
        /// <param name="table">Name of the table</param>
        /// <returns></returns>
        public SelectBuilder From(string table)
        {
            _builder.Append($" FROM {table}");
            return this;
        }

        /// <summary>
        /// Adds an INNER JOIN with ON clause
        /// </summary>
        /// <param name="table">Name of the table</param>
        /// <param name="on">ON clause</param>
        /// <returns></returns>
        public SelectBuilder InnerJoin(string table, string on)
        {
            _builder.Append(Join("INNER", table, on));
            return this;
        }

        /// <summary>
        /// Adds an LEFT JOIN with ON clause
        /// </summary>
        /// <param name="table">Name of the table</param>
        /// <param name="on">ON clause</param>
        /// <returns></returns>
        public SelectBuilder LeftJoin(string table, string on)
        {
            _builder.Append(Join("LEFT", table, on));
            return this;
        }

        /// <summary>
        /// Adds an FULL OUTER JOIN with ON clause
        /// </summary>
        /// <param name="table">Name of the table</param>
        /// <param name="on">ON clause</param>
        /// <returns></returns>
        public SelectBuilder FullOuterJoin(string table, string on)
        {
            _builder.Append(Join("FULL OUTER", table, on));
            return this;
        }

        /// <summary>
        /// Adds the where clause with the first condition
        /// </summary>
        /// <param name="condition">First condition of the WHERE condition</param>
        /// <returns></returns>
        public WhereBuilder Where(string condition)
        {
            return _whereBuilder.AddRootClause(condition);
        }

        /// <summary>
        /// Adds the GROUP BY statement to query
        /// </summary>
        /// <param name="group">The, comma separated, group members</param>
        public SelectBuilder GroupBy(string group)
        {
            _builder.Append($" GROUP BY {group}");
            return this;
        }

        /// <summary>
        /// Adds the GROUP BY statement to query
        /// </summary>
        /// <param name="group">Array with the group members</param>
        public SelectBuilder GroupBy(string[] group)
        {
            var g = string.Join(",", group);
            return GroupBy(g);
        }

        /// <summary>
        /// Adds the GROUP BY statement to query
        /// </summary>
        /// <param name="order">The, comma separated, group members</param>
        public SelectBuilder OrderBy(string order)
        {
            _builder.Append($" ORDER BY {order}");
            return this;
        }

        /// <summary>
        /// Adds the GROUP BY statement to query
        /// </summary>
        /// <param name="order">Array with the group members</param>
        public SelectBuilder OrderBy(string[] order)
        {
            var g = string.Join(",", order);
            return OrderBy(g);
        }

        /// <summary>
        /// Adds pagination using the OFFSET/FETCH statements
        /// </summary>
        /// <param name="pageSize">Number of rows per page</param>
        /// <param name="currentPage">Current page</param>
        public SelectBuilder Paginated(int pageSize, int currentPage)
        {
            _builder.Append($" OFFSET {pageSize * (currentPage - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            return this;
        }

        public string Build()
        {
            return _builder.ToString();
        }

        private string Join(string join, string table, string on)
        {
            return $" {join} JOIN {table} ON {on}";
        }
    }
}