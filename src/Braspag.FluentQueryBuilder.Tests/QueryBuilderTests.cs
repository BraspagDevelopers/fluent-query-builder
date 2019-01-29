using FluentAssertions;
using Xunit;

namespace Braspag.FluentQueryBuilder.Tests
{
    public class QueryBuilderTests
    {
        [Fact]
        public void SelectFromQuery_UsingStringSelect_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1");
        }

        [Fact]
        public void SelectFromQuery_UsingStringArraySelect_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select(new[] { "Field1", "Field2", "Field3" })
                .From("Table1")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1");
        }

        [Fact]
        public void InnerJoin_WithOn_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1 T1")
                .InnerJoin("Table2 T2", "T1.Field1 = T2.Field2")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 T1 INNER JOIN Table2 T2 ON T1.Field1 = T2.Field2");
        }

        [Fact]
        public void LeftJoin_WithOn_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1 T1")
                .LeftJoin("Table2 T2", "T1.Field1 = T2.Field2")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 T1 LEFT JOIN Table2 T2 ON T1.Field1 = T2.Field2");
        }

        [Fact]
        public void RightJoin_WithOn_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1 T1")
                .RightJoin("Table2 T2", "T1.Field1 = T2.Field2")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 T1 RIGHT JOIN Table2 T2 ON T1.Field1 = T2.Field2");
        }

        [Fact]
        public void FullOuterJoin_WithOn_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1 T1")
                .FullOuterJoin("Table2 T2", "T1.Field1 = T2.Field2")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 T1 FULL OUTER JOIN Table2 T2 ON T1.Field1 = T2.Field2");
        }

        [Fact]
        public void Select_WithWhere_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value'");
        }

        [Fact]
        public void Select_WithWhereAnd_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .And("Field2 = 'This value is not so cool as the first'")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value' AND Field2 = 'This value is not so cool as the first'");
        }

        [Theory]
        [InlineData(true, " AND Field2 = 'This value is not so cool as the first'")]
        [InlineData(false, "")]
        public void Select_WithWhereAndIf_ShouldReturnExpectedResult(bool condition, string complement)
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .AndIf("Field2 = 'This value is not so cool as the first'", condition)
                .Build();

            sql.Should().Be($"SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value'{complement}");
        }

        [Fact]
        public void Select_WithWhereAndGroup_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .And("Field2 = 'This value is not so cool as the first'", x => x.Or("Field2 = 'Who cares?'"))
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value' AND (Field2 = 'This value is not so cool as the first' OR Field2 = 'Who cares?')");
        }

        [Theory]
        [InlineData(true, " AND (Field2 = 'This value is not so cool as the first' OR Field2 = 'Who cares?')")]
        [InlineData(false, "")]
        public void Select_WithWhereAndIfGroup_ShouldReturnExpectedResult(bool condition, string complement)
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .AndIf("Field2 = 'This value is not so cool as the first'", x => x.Or("Field2 = 'Who cares?'"), condition)
                .Build();

            sql.Should().Be($"SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value'{complement}");
        }

        [Fact]
        public void Select_WithWhereOr_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .Or("Field2 = 'This value is not so cool as the first'")
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value' OR Field2 = 'This value is not so cool as the first'");
        }

        [Theory]
        [InlineData(true, " OR Field2 = 'This value is not so cool as the first'")]
        [InlineData(false, "")]
        public void Select_WithWhereOrIf_ShouldReturnExpectedResult(bool condition, string complement)
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .OrIf("Field2 = 'This value is not so cool as the first'", condition)
                .Build();

            sql.Should().Be($"SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value'{complement}");
        }

        [Fact]
        public void Select_WithWhereOrGroup_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .Or("Field2 = 'This value is not so cool as the first'", x => x.And("Field3 = 'Who cares?'"))
                .Build();

            sql.Should().Be("SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value' OR (Field2 = 'This value is not so cool as the first' AND Field3 = 'Who cares?')");
        }

        [Theory]
        [InlineData(true, " OR (Field2 = 'This value is not so cool as the first' AND Field2 = 'Who cares?')")]
        [InlineData(false, "")]
        public void Select_WithWhereOrIfGroup_ShouldReturnExpectedResult(bool condition, string complement)
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,Field3")
                .From("Table1")
                .Where("Field1 = 'A very cool value'")
                .OrIf("Field2 = 'This value is not so cool as the first'", x => x.And("Field2 = 'Who cares?'"), condition)
                .Build();

            sql.Should().Be($"SELECT Field1,Field2,Field3 FROM Table1 WHERE Field1 = 'A very cool value'{complement}");
        }

        [Fact]
        public void Select_WithStringGroupBy_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,COUNT(*)")
                .From("Table1")
                .GroupBy("Field1")
                .Build();

            sql.Should().Be("SELECT Field1,COUNT(*) FROM Table1 GROUP BY Field1");
        }

        [Fact]
        public void Select_WithStringGroupBy_AndWhereClause_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,COUNT(*)")
                .From("Table1")
                .Where("Table1.Id = 3")
                .GroupBy("Field1")
                .Build();

            sql.Should().Be("SELECT Field1,COUNT(*) FROM Table1 WHERE Table1.Id = 3 GROUP BY Field1");
        }

        [Fact]
        public void Select_WithStringArrayGroupBy_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .GroupBy(new[] { "Field1", "Field2" })
                .Build();

            sql.Should().Be("SELECT Field1,Field2,COUNT(*) FROM Table1 GROUP BY Field1,Field2");
        }

        [Fact]
        public void Select_WithStringArrayGroupBy_AndWhereClause_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .Where("Table1.Id = 3")
                .GroupBy(new[] { "Field1", "Field2" })
                .Build();

            sql.Should().Be("SELECT Field1,Field2,COUNT(*) FROM Table1 WHERE Table1.Id = 3 GROUP BY Field1,Field2");
        }

        [Fact]
        public void Select_WithStringOrderBy_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,COUNT(*)")
                .From("Table1")
                .OrderBy("Field1")
                .Build();

            sql.Should().Be("SELECT Field1,COUNT(*) FROM Table1 ORDER BY Field1");
        }

        [Fact]
        public void Select_WithStringOrderBy_AndWhereClause_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,COUNT(*)")
                .From("Table1")
                .Where("Table1.Id = 3")
                .OrderBy("Field1")
                .Build();

            sql.Should().Be("SELECT Field1,COUNT(*) FROM Table1 WHERE Table1.Id = 3 ORDER BY Field1");
        }

        [Fact]
        public void Select_WithStringArrayOrderBy_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .OrderBy(new[] { "Field1", "Field2" })
                .Build();

            sql.Should().Be("SELECT Field1,Field2,COUNT(*) FROM Table1 ORDER BY Field1,Field2");
        }

        [Fact]
        public void Select_WithStringArrayOrderBy_AndWhereClause_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .Where("Table1.Id = 3")
                .OrderBy(new[] { "Field1", "Field2" })
                .Build();

            sql.Should().Be("SELECT Field1,Field2,COUNT(*) FROM Table1 WHERE Table1.Id = 3 ORDER BY Field1,Field2");
        }

        [Fact]
        public void Select_Paginated_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .Paginated(50, 3)
                .Build();

            sql.Should().Be("SELECT Field1,Field2,COUNT(*) FROM Table1 OFFSET 100 ROWS FETCH NEXT 50 ROWS ONLY");
        }

        [Fact]
        public void Select_Paginated_WithWhereClause_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .Where("Table1.Field = 3")
                .Paginated(50, 3)
                .Build();

            sql.Should().Be("SELECT Field1,Field2,COUNT(*) FROM Table1 WHERE Table1.Field = 3 OFFSET 100 ROWS FETCH NEXT 50 ROWS ONLY");
        }

        [Fact]
        public void Select_One_TableHint_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2")
                .From("Table1")
                .With("TABLOCK")
                .Build();

            sql.Should().Be("SELECT Field1,Field2 FROM Table1 WITH(TABLOCK)");
        }

        [Fact]
        public void Select_Multiple_TableHint_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2")
                .From("Table1")
                .With("TABLOCK", "INDEX(myindex)", "KEEPIDENTITY")
                .Build();

            sql.Should().Be("SELECT Field1,Field2 FROM Table1 WITH(TABLOCK,INDEX(myindex),KEEPIDENTITY)");
        }

        [Fact]
        public void Select_QueryHint_WithWhereAndJoinClause_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("T1.Field1,T1.Field2")
                .From("Table1 T1").With("TABLOCK", "INDEX(myindex)", "KEEPIDENTITY")
                .InnerJoin("Table2 T2", "T1.Id = T2.Id")
                .Where("T1.Id = 3")
                .Option("RECOMPILE", "INDEX(myindex)", "KEEPIDENTITY")
                .Build();

            sql.Should().Be("SELECT T1.Field1,T1.Field2 FROM Table1 T1 WITH(TABLOCK,INDEX(myindex),KEEPIDENTITY) INNER JOIN Table2 T2 ON T1.Id = T2.Id WHERE T1.Id = 3 OPTION(RECOMPILE,INDEX(myindex),KEEPIDENTITY)");
        }

        [Fact]
        public void Select_One_QueryHint_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2")
                .From("Table1")
                .Option("RECOMPILE")
                .Build();

            sql.Should().Be("SELECT Field1,Field2 FROM Table1 OPTION(RECOMPILE)");
        }

        [Fact]
        public void Select_Multiple_QueryHint_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2")
                .From("Table1")
                .Option("RECOMPILE", "INDEX(myindex)", "KEEPIDENTITY")
                .Build();

            sql.Should().Be("SELECT Field1,Field2 FROM Table1 OPTION(RECOMPILE,INDEX(myindex),KEEPIDENTITY)");
        }

        [Fact]
        public void SelectCountFromQuery_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .SelectCount("Id")
                .From("Table1")
                .Build();

            sql.Should().Be("SELECT COUNT(Id) FROM Table1");
        }

        [Fact]
        public void SelectCountAllFromQuery_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .SelectCountAll()
                .From("Table1")
                .Build();

            sql.Should().Be("SELECT COUNT(*) FROM Table1");
        }

        [Fact]
        public void PrependSelectFromQuery_UsingStringSelect_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2")
                .From("Table1")
                .PrependSelect("Field3,Field4")
                .Build();

            sql.Should().Be("SELECT Field3,Field4,Field1,Field2 FROM Table1");
        }

        [Fact]
        public void PrependSelectFromQuery_UsingStringArraySelect_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select(new[] { "Field1", "Field2" })
                .From("Table1")
                .PrependSelect(new[] { "Field3", "Field4" })
                .Build();

            sql.Should().Be("SELECT Field3,Field4,Field1,Field2 FROM Table1");
        }

        [Fact]
        public void PrependSelectAfterWhereFromQuery_UsingStringSelect_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2")
                .From("Table1")
                .Where("Field5 = Field6")
                .PrependSelect("Field3,Field4")
                .Build();

            sql.Should().Be("SELECT Field3,Field4,Field1,Field2 FROM Table1 WHERE Field5 = Field6");
        }

        [Fact]
        public void PrependSelectAfterWhereFromQuery_UsingStringArraySelect_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select(new[] { "Field1", "Field2" })
                .From("Table1")
                .Where("Field5 = Field6")
                .PrependSelect(new[] { "Field3", "Field4" })
                .Build();

            sql.Should().Be("SELECT Field3,Field4,Field1,Field2 FROM Table1 WHERE Field5 = Field6");
        }

        [Fact]
        public void Select_PaginatedByDenseRank_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .PaginatedByDenseRank(50, 3, "Field1")
                .Build();

            sql.Should().Be("SELECT * FROM (SELECT DENSE_RANK() OVER (ORDER BY Field1) AS RowPosition,Field1,Field2,COUNT(*) FROM Table1) DerivedTable WHERE RowPosition BETWEEN 101 AND 150");
        }

        [Fact]
        public void Select_PaginatedByDenseRank_AfterWhere_ShouldReturnExpectedResult()
        {
            var sql = new SelectBuilder()
                .Select("Field1,Field2,COUNT(*)")
                .From("Table1")
                .Where("Field1 IS NULL")
                .PaginatedByDenseRank(50, 3, "Field1")
                .Build();

            sql.Should().Be("SELECT * FROM (SELECT DENSE_RANK() OVER (ORDER BY Field1) AS RowPosition,Field1,Field2,COUNT(*) FROM Table1 WHERE Field1 IS NULL) DerivedTable WHERE RowPosition BETWEEN 101 AND 150");
        }
    }
}