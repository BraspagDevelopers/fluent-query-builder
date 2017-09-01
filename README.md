# Introduction 

The purpose of this project is to make easier the construction of dynamic queries that are commonly used in the implementation of reports with several optional filters.

### What is not the purpose of this project?

This project does not want to be like the awesome [Dapper](https://github.com/StackExchange/Dapper) that is an Object Mapper. We only want to build the query as easy as possible and then we use this query with [Dapper](https://github.com/StackExchange/Dapper).

This project also does not validade order of the method calls, so beware!

Below is an example of converting queries with IFs to **SelectBuilder**.

``` csharp
var sql = string.Concat("SELECT C.Id, C.Name, C.Identity, C.Phone, E.State ",
                        "FROM Cliente C ",
                        "INNER JOIN Endereco E ON C.Id = E.ClientId ",
                        "WHERE C.Active = 1 AND C.RegisterDate > '2017-01-01'";
                        
if(!string.IsNullOrEmpty(filter.Name))
{
  sql = string.Concat("AND C.Name LIKE '%' + @clientName + '%'");
}
if(!string.IsNullOrEmpty(filter.State))
{
  sql = string.Concat("AND E.Staste = @state");
}
```
This type of code become confuse and hard to read very quickly. The approach using **SelectBuilder** is:
``` csharp
var sql = new SelectBuilder()
  .Select("C.Id, C.Name, C.Identity, C.Phone, E.State")
  .From("Cliente C")
  .Where("C.Active = 1")
  .And("C.Active = 1 AND C.RegisterDate > '2017-01-01'")
  .AndIf("C.Name LIKE '%' + @clientName + '%'", !string.IsNullOrEmpty(filter.Name))
  .AndIf("E.Staste = @state", !string.IsNullOrEmpty(filter.State));
```
As you can see, the **SelectBuilder** approach is much cleaner than using IFs. The API will be detailed in the next sections.

# Getting Started
You only need to install the **Braspag.FluentQueryBuilder** package from Nuget.org and you a ready to build your queries.
```
PM> install-package Braspag.FluentQueryBuilder
```

# SelectBuilder Fluent Api
The **SelectBuilder** is designed to have an API as fluent as possible, so it will not be hard to you understand. Below are the list of all the methods:

- **Select(** *string fields* **)**   
  &rarr; Adds the SELECT statement to Query

- **Select(** *string[] fields* **)**   
  &rarr; Adds the SELECT statement to Query
  
- **From(** *string table* **)**   
  &rarr; Adds the FROM of the query

- **InnerJoin(** *string table, string on* **)**    
  &rarr; Adds an INNER JOIN with ON clause

- **LeftJoin(** *string table, string on* **)**   
  &rarr; Adds an LEFT JOIN with ON clause

- **FullOuterJoin(** *string table, string on* **)**    
  &rarr; Adds an FULL OUTER JOIN with ON clause

- **Where(** *string condition* **)**    
  &rarr; Adds the where clause with the first condition

- **And(** *string predicate* **)**   
  &rarr;  Adds an AND condition to WHERE statement

- **And(** *string predicate, Func<WhereBuilder, WhereBuilder> predicates* **)**    
  &rarr; Adds an AND group condition to WHERE statement

- **AndIf(** *string predicate, bool condition* **)**    
  &rarr; Conditionally adds an AND condition to WHERE statement

- **AndIf(** *string predicate, Func<WhereBuilder, WhereBuilder> predicates, bool condition* **)**   
  &rarr; Conditionally adds an AND group condition to WHERE statement

- **Or(** *string predicate* **)**    
  &rarr; Adds an OR condition to WHERE statement

- **Or(** *string predicate, Func<WhereBuilder, WhereBuilder> predicates* **)**    
  &rarr; Adds an OR group condition to WHERE statement

- **OrIf(** *string predicate, bool condition* **)**    
  &rarr; Conditionally adds an OR condition to WHERE statement

- **OrIf(** *string predicate, Func<WhereBuilder, WhereBuilder> predicates, bool condition* **)** 
  &rarr; Conditionally adds an OR group condition to WHERE statement

- **GroupBy(** *string group* **)**   
  &rarr; Adds the GROUP BY statement to query

- **GroupBy(** *string[] group* **)**   
  &rarr; Adds the GROUP BY statement to query

- **OrderBy(** *string order* **)**   
  &rarr; Adds the ORDER BY statement to query

- **OrderBy(** *string[] order* **)**   
  &rarr; Adds the ORDER BY statement to query

# Sample

Selecting filds "*Field1, Field2, Field3*" from table "*Table1*" where "*Field1 = 'A very cool value'*".

``` csharp
var sql = new SelectBuilder()
  .Select("Field1,Field2,Field3")
  .From("Table1")
  .Where("Field1 = 'A very cool value'")
  .Build();

sql.Should().Be("SELECT Campo1,Campo2,Campo3 FROM Tabela1 WHERE Campo1 = 'A very cool value'");
```

Want more samples? Look a the [tests](https://github.com/BraspagDevelopers/fluent-query-builder/blob/master/src/Braspag.FluentQueryBuilder.Tests/QueryBuilderTests.cs)! Every method has a test.

# Build and Test
To build this project you will need Visual Studio 2017.3. If you already have it, clone this repo and have fun!