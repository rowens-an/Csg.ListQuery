using Microsoft.VisualStudio.TestTools.UnitTesting;
using Csg.ListQuery.Abstractions;
using System.Collections.Generic;
using Csg.Data.Sql;
using System.Linq;
using Csg.Data;
using Csg.ListQuery.Sql;

namespace Csg.ListQuery.Tests
{
    [TestClass]
    public partial class ListQueryTests
    {
        static ListQueryTests()
        {
            Csg.Data.DbQueryBuilder.GenerateFormattedSql = false;
        }

        [TestMethod]
        public void Test_ListQuery_BuildThrowsExceptionWhenNoConfig()
        {
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("Person", new Mock.MockConnection());
            var request = new ListQueryDefinition();

            request.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.Equal, Value = "Bob" }
            });

            Assert.ThrowsException<System.Exception>(() =>
            {
                var stmt = ListQueryBuilder.Create(query, request)
                    .Apply();
            });            
        }

        [TestMethod]
        public void Test_ListQuery_BuildWithNoValidation()
        {
            var expectedSql = "SELECT [t0].[FirstName],[t0].[LastName] FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]=@p0);";
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());
            var queryDef = new ListQueryDefinition();

            queryDef.Selections = new string[] { "FirstName", "LastName" };
            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.Equal, Value = "Bob" }
            });

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .NoValidation()
                .UseStreamingResult()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListQuery_DefaultHandler()
        {
            var expectedSql = "SELECT [t0].[FirstName],[t0].[LastName] FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]=@p0);";
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());
            var queryDef = new ListQueryDefinition();

            queryDef.Selections = new string[] { "FirstName", "LastName" };
            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.Equal, Value = "Bob" }
            });

            var conn = new Mock.MockConnection();

            var stmt = conn
                .QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_Equals()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]=@p0);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.Equal, Value = "Bob" }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_NotEquals()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]<>@p0);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.NotEqual, Value = "Bob" }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_GreaterThan()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]>@p0);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.GreaterThan, Value = "Bob" }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_GreaterThanOrEqual()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]>=@p0);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.GreaterThanOrEqual, Value = "Bob" }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_LessThan()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]<@p0);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.LessThan, Value = "Bob" }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_LessThanOrEqual()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]<=@p0);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.LessThanOrEqual, Value = "Bob" }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_IsNullTrue()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName] IS NULL);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.IsNull, Value = true }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
        }

        [TestMethod]
        public void Test_ListFilterOperator_IsNullFalse()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName] IS NOT NULL);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.IsNull, Value = false }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
        }

        [TestMethod]
        public void Test_ListFilterOperator_Between()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE (([t0].[FirstName]>=@p0) AND ([t0].[FirstName]<=@p1));";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.Between, Value = new string[]{ "Bob", "Dole" } }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(2, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListFilterOperator_Like()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName] LIKE @p0);";
            var queryDef = new ListQueryDefinition()
            {
                Filters = new List<ListQueryFilter>() {
                    new ListQueryFilter() { Name = "FirstName", Operator = ListFilterOperator.Like, Value = "Bob" }
                }
            };

            var stmt = new Mock.MockConnection().QueryBuilder("dbo.Person")
                .ListQuery(queryDef)
                .ValidateWith<Mock.Person>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
            // default is a beginswith search
            Assert.AreEqual("Bob%", stmt.Parameters.First().Value.ToString());
        }


        [TestMethod]
        public void Test_ListQuery_InlineHandler()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID])));";
            //                 SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID])));
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());            
            var queryDef = new ListQueryDefinition();

            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "PhoneNumber", Operator = ListFilterOperator.Like, Value = "555" }
            });

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .ValidateWith<Mock.Person>()
                .AddFilterHandler("PhoneNumber", Mock.PersonFilters.PhoneNumber)
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListQuery_TypeDerivedHandler()
        {
            var expectedSql = "SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID])));";
            //                 SELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[PersonID] IN (SELECT [t1].[PersonID] FROM [dbo].[PersonPhoneNumber] AS [t1] WHERE ([t1].[PhoneNumber] LIKE @p0) AND ([t1].[PersonID]=[t0].[PersonID])));
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());
            var queryDef = new ListQueryDefinition();

            queryDef.Filters = new List<ListQueryFilter>(new ListQueryFilter[] {
                new ListQueryFilter() { Name = "PhoneNumber", Operator = ListFilterOperator.Like, Value = "555" }
            });

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .ValidateWith<Mock.Person>()
                .AddFilterHandlers<Mock.PersonFilters>()
                .Apply()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText);
            Assert.AreEqual(1, stmt.Parameters.Count);
        }

        [TestMethod]
        public void Test_ListQuery_Paging()
        {
            var expectedSql = "SELECT COUNT(1) FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]=@p0);\r\nSELECT * FROM [dbo].[Person] AS [t0] WHERE ([t0].[FirstName]=@p1) ORDER BY [FirstName] ASC OFFSET 50 ROWS FETCH NEXT 26 ROWS ONLY;";
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());

            var queryDef = new ListQueryDefinition();

            queryDef.Sort = new List<ListQuerySort>()
            {
               new ListQuerySort(){ Name = "FirstName" }
            };

            queryDef.Filters = new List<ListQueryFilter>()
            {
                new ListQueryFilter(){ Name = "FirstName", Operator = ListFilterOperator.Equal, Value = "Bob"}
            };

            queryDef.Limit = 25;
            queryDef.Offset = 50;

            var stmt = ListQueryBuilder.Create(query, queryDef)
                .NoValidation()
                .Render();

            Assert.AreEqual(expectedSql, stmt.CommandText.Trim(), true);
        }

        [TestMethod]
        public void Test_ListQuery_Buffered_ApplyAddsLimitOracle()
        {
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());

            var queryDef = new ListQueryDefinition();

            queryDef.Sort = new List<ListQuerySort>()
            {
               new ListQuerySort(){ Name = "FirstName" }
            };

            queryDef.Offset = 0;
            queryDef.Limit = 50;

            var qb = ListQueryBuilder.Create(query, queryDef)
                .NoValidation()
                .Apply();

            Assert.AreEqual(0, qb.PagingOptions.Value.Offset);
            Assert.AreEqual(51, qb.PagingOptions.Value.Limit);
        }

        [TestMethod]
        public void Test_ListQuery_Streamed_Apply()
        {
            IDbQueryBuilder query = new Csg.Data.DbQueryBuilder("dbo.Person", new Mock.MockConnection());

            var queryDef = new ListQueryDefinition();

            queryDef.Sort = new List<ListQuerySort>()
            {
               new ListQuerySort(){ Name = "FirstName" }
            };

            queryDef.Offset = 0;
            queryDef.Limit = 50;

            var qb = ListQueryBuilder.Create(query, queryDef)
                .NoValidation()
                .UseStreamingResult()
                .Apply();

            Assert.AreEqual(0, qb.PagingOptions.Value.Offset);
            Assert.AreEqual(50, qb.PagingOptions.Value.Limit);
        }
    }
}