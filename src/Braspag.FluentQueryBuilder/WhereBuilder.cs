using System;
using System.Text;

namespace Braspag.FluentQueryBuilder
{
    public class WhereBuilder
    {
        private readonly SelectBuilder _selectBuilder;
        private readonly StringBuilder _builder;

        public WhereBuilder(SelectBuilder selectBuilder, StringBuilder builder)
        {
            _selectBuilder = selectBuilder;
            _builder = builder;
        }

        internal WhereBuilder AddRootClause(string condition)
        {
            _builder.Append($" WHERE {condition}");
            return this;
        }

        /// <summary>
        /// Adds an AND condition to WHERE statement
        /// </summary>
        public WhereBuilder And(string predicate)
        {
            return AppendClause("AND", predicate);
        }

        /// <summary>
        /// Adds an AND group condition to WHERE statement
        /// </summary>
        public WhereBuilder And(string predicate, Func<WhereBuilder, WhereBuilder> predicates)
        {
            return AppendGroupClause("AND", predicate, predicates);
        }

        /// <summary>
        /// Conditionally adds an AND condition to WHERE statement
        /// </summary>
        /// <param name="condition">The condition that must be satisfied to add the AND</param>
        public WhereBuilder AndIf(string predicate, bool condition)
        {
            return condition ? And(predicate) : this;
        }

        /// <summary>
        /// Conditionally adds an AND group condition to WHERE statement
        /// </summary>
        public WhereBuilder AndIf(string predicate, Func<WhereBuilder, WhereBuilder> predicates, bool condition)
        {
            return condition ? And(predicate, predicates) : this;
        }

        /// <summary>
        /// Adds an OR condition to WHERE statement
        /// </summary>
        public WhereBuilder Or(string predicate)
        {
            return AppendClause("OR", predicate);
        }

        /// <summary>
        /// Adds an OR group condition to WHERE statement
        /// </summary>
        public WhereBuilder Or(string predicate, Func<WhereBuilder, WhereBuilder> conditions)
        {
            return AppendGroupClause("OR", predicate, conditions);
        }

        /// <summary>
        /// Conditionally adds an OR condition to WHERE statement
        /// </summary>
        /// <param name="condition">The condition that must be satisfied to add the OR</param>
        /// <returns></returns>
        public WhereBuilder OrIf(string predicate, bool condition)
        {
            return condition ? Or(predicate) : this;
        }

        /// <summary>
        /// Conditionally adds an OR group condition to WHERE statement
        /// </summary>
        public WhereBuilder OrIf(string predicate, Func<WhereBuilder, WhereBuilder> conditions, bool condition)
        {
            return condition ? Or(predicate, conditions) : this;
        }

        /// <summary>
        /// Adds the GROUP BY statement to query
        /// </summary>
        /// <param name="group">The, comma separated, group members</param>
        public SelectBuilder GroupBy(string group)
        {
            return _selectBuilder.GroupBy(group);
        }

        /// <summary>
        /// Adds the GROUP BY statement to query
        /// </summary>
        /// <param name="group">Array with the group members</param>
        public SelectBuilder GroupBy(string[] group)
        {
            return _selectBuilder.GroupBy(group);
        }

        /// <summary>
        /// Adds the ORDER BY statement to query
        /// </summary>
        /// <param name="order">The, comma separated, group members</param>
        public SelectBuilder OrderBy(string order)
        {
            return _selectBuilder.OrderBy(order);
        }

        /// <summary>
        /// Adds the ORDER BY statement to query
        /// </summary>
        /// <param name="order">Array with the group members</param>
        public SelectBuilder OrderBy(string[] order)
        {
            return _selectBuilder.OrderBy(order);
        }

        /// <summary>
        /// Adds pagination using the OFFSET/FETCH statements
        /// </summary>
        /// <param name="pageSize">Number of rows per page</param>
        /// <param name="currentPage">Current page</param>
        public SelectBuilder Paginated(int pageSize, int currentPage)
        {
            return _selectBuilder.Paginated(pageSize, currentPage);
        }

        /// <summary>
        /// Adds pagination using DENSE_RANK
        /// </summary>
        /// <param name="pageSize">Number of rows per page</param>
        /// <param name="currentPage">Current page</param>
        /// <param name="rankField">Field used for ranking</param>
        public WhereBuilder PaginatedByDenseRank(int pageSize, int currentPage, string rankField)
        {
            return _selectBuilder.PaginatedByDenseRank(pageSize, currentPage, rankField);
        }

        /// <summary>
        /// Adds query hint option statement
        /// </summary>
        /// <param name="hints">The option clause to be used</param>
        public SelectBuilder Option(params string[] hints)
        {
            return _selectBuilder.Option(hints);
        }

        /// <summary>
        /// Adds more fields to the beginning of the SELECT statement.
        /// </summary>
        /// <param name="fields">The comma separated fields to be added</param>  
        public SelectBuilder PrependSelect(string fields)
        {
            return _selectBuilder.PrependSelect(fields);
        }

        /// <summary>
        /// Adds more fields to the beginning of the SELECT statement.
        /// </summary>
        /// <param name="fields">Array with the fields that the query will return</param>
        public SelectBuilder PrependSelect(string[] fields)
        {
            return _selectBuilder.PrependSelect(fields);
        }

        public string Build()
        {
            return _selectBuilder.Build();
        }

        private WhereBuilder AppendClause(string op, string condition)
        {
            _builder.Append($" {op} {condition}");
            return this;
        }

        private WhereBuilder AppendGroupClause(string op, string clause, Func<WhereBuilder, WhereBuilder> conditions)
        {
            _builder.Append($" {op} ({clause}");
            conditions.Invoke(this);
            _builder.Append(")");

            return this;
        }
    }
}