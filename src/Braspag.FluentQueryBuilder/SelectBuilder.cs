using System.Collections.Generic;
using System.Linq;
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
        /// Adds the SELECT COUNT() statement to Query.
        /// </summary>
        /// <param name="field">The field where the counting will be made on</param>              
        public SelectBuilder SelectCount(string field)
        {
            _builder.Append($"SELECT COUNT({field})");
            return this;
        }

        /// <summary>
        /// Adds the SELECT COUNT(*) statement to Query.
        /// </summary>            
        public SelectBuilder SelectCountAll()
        {
            return SelectCount("*");
        }

        /// <summary>
        /// Adds more fields to the beginning of the SELECT statement.
        /// </summary>
        /// <param name="fields">The comma separated fields to be added</param>  
        public SelectBuilder PrependSelect(string fields)
        {
            var select = "SELECT ";
            _builder.Remove(0, select.Length).Insert(0, $"{new SelectBuilder().Select(fields).Build()},");
            return this;
        }

        /// <summary>
        /// Adds more fields to the beginning of the SELECT statement.
        /// </summary>
        /// <param name="fields">Array with the fields that the query will return</param>
        public SelectBuilder PrependSelect(string[] fields)
        {
            string f = string.Join(",", fields);
            return PrependSelect(f);
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
        /// Adds an RIGHT JOIN with ON clause
        /// </summary>
        /// <param name="table">Name of the table</param>
        /// <param name="on">ON clause</param>
        /// <returns></returns>
        public SelectBuilder RightJoin(string table, string on)
        {
            _builder.Append(Join("RIGHT", table, on));
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

        /// <summary>
        /// Adds query hint option statement
        /// </summary>
        /// <param name="hints">The option clause to be used</param>
        public SelectBuilder Option(params string[] hints)
        {
            _builder.Append(Hint("OPTION", hints));
            return this;
        }

        /// <summary>
        /// Adds table hint with statement
        /// </summary>
        /// <param name="hints">The with clause to be used</param>
        /// <returns></returns>
        public SelectBuilder With(params string[] hints)
        {
            _builder.Append(Hint("WITH", hints));
            return this;
        }

        public string Build()
        {
            return _builder.ToString();
        }

        /// <summary>
        /// Mount the hints with their parentheses and commas
        /// </summary>
        /// <param name="hintClause">The hint clause(OPTION, WITH)</param>
        /// <param name="hints">The hints statements array</param>
        /// <returns>The hint with their parentheses and commas</returns>
        private string Hint(string hintClause, IReadOnlyCollection<string> hints)
        {
            return hints.Aggregate($" {hintClause}(",
                (current, hint) =>
                    hints.Count == 1 || hints.Last().Equals(hint) ? current + hint : current + hint + ",",
                resultHint => resultHint + ")");
        }

        private string Join(string join, string table, string on)
        {
            return $" {join} JOIN {table} ON {on}";
        }
    }
}