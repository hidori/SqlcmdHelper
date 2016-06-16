using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.IO;

namespace Mjollnir.Testing.Helpers.Tests
{
    [TestFixture]
    public class SqlcmdHelperTest
    {
        static class Config
        {
            public static readonly string TestDirectoryPath = Path.GetDirectoryName(typeof(SqlcmdHelperTest).Assembly.Location);
            public static readonly string ProjectDirectoryPath = Path.GetFullPath(Path.Combine(TestDirectoryPath, @"..\.."));

            public const string ExecutablePath = @"SQLCMD.EXE";
            public const string Server = @".\SQL2014";
            public const string UserId = "sa";
            public const string Password = "Password12!";
        }

        [Test]
        public void ConstructorTest()
        {
            var path = Config.ExecutablePath;
            var server = Config.Server;
            var userId = Config.UserId;
            var password = Config.Password;
            var database = "master";

            {
                var target = new SqlcmdHelper(path, server, userId, password, database);

                target.ExecutablePath.Is(path);
                target.Server.Is(server);
                target.UserId.Is(userId);
                target.Password.Is(password);
                target.Database.Is(database);
            }

            {
                var target = new SqlcmdHelper(path, server, userId, password);

                target.ExecutablePath.Is(path);
                target.Server.Is(server);
                target.UserId.Is(userId);
                target.Password.Is(password);
                target.Database.IsNull();
            }

            {
                var target = new SqlcmdHelper(path, server, database);

                target.ExecutablePath.Is(path);
                target.Server.Is(server);
                target.UserId.IsNull();
                target.Password.IsNull();
                target.Database.Is(database);
            }

            {
                var target = new SqlcmdHelper(path, server);

                target.ExecutablePath.Is(path);
                target.Server.Is(server);
                target.UserId.IsNull();
                target.Password.IsNull();
                target.Database.IsNull();
            }
        }

        [Test]
        public void ExecuteQueryStringTest()
        {
            var path = Config.ExecutablePath;
            var server = Config.Server;
            var userId = Config.UserId;
            var password = Config.Password;

            {
                var database = $"SqlcmdHelperTest_{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}_ExecuteQueryString";

                var target = new SqlcmdHelper(path, server, userId, password);

                target.ExecuteQueryString($"CREATE DATABASE {database}");
                target.ExecuteQueryString($"CREATE TABLE [dbo].[People]([Id] [bigint] NOT NULL, [Name] [nvarchar](50) NOT NULL)", database);
                target.ExecuteQueryString($"INSERT INTO [dbo].[People] ([Id], [Name]) VALUES (1, 'ABC')", database);
                target.ExecuteQueryString($"INSERT INTO [dbo].[People] ([Id], [Name]) VALUES (2, '\"ABC\"\"')", database);

                var connectionString = $"Data Source={server};Initial Catalog={database};User Id={userId};Password={password};Pooling=False";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM [dbo].[People]";
                        command.ExecuteScalar().Is(2);
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM [dbo].[People] ORDER BY [Id]";

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            ((long)reader["Id"]).Is(1);
                            ((string)reader["Name"]).Is("ABC");

                            reader.Read();
                            ((long)reader["Id"]).Is(2);
                            ((string)reader["Name"]).Is("\"ABC\"\"");
                        }
                    }
                }

                target.ExecuteQueryString($"DROP DATABASE {database}");
            }

            {
                var database = $"SqlcmdHelperTest_{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}_ExecuteQueryString";

                var target = new SqlcmdHelper(path, server);

                target.ExecuteQueryString($"CREATE DATABASE {database}");
                target.ExecuteQueryString($"CREATE TABLE [dbo].[People]([Id] [bigint] NOT NULL, [Name] [nvarchar](50) NOT NULL)", database);
                target.ExecuteQueryString($"INSERT INTO [dbo].[People] ([Id], [Name]) VALUES (1, 'ABC')", database);
                target.ExecuteQueryString($"INSERT INTO [dbo].[People] ([Id], [Name]) VALUES (2, '\"ABC\"\"')", database);

                var connectionString = $"Data Source={server};Initial Catalog={database};Integrated Security=True;Pooling=False";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM [dbo].[People]";
                        command.ExecuteScalar().Is(2);
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM [dbo].[People] ORDER BY [Id]";

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            ((long)reader["Id"]).Is(1);
                            ((string)reader["Name"]).Is("ABC");

                            reader.Read();
                            ((long)reader["Id"]).Is(2);
                            ((string)reader["Name"]).Is("\"ABC\"\"");
                        }
                    }
                }

                target.ExecuteQueryString($"DROP DATABASE {database}");
            }
        }

        [Test]
        public void ExecuteQueryFileTest()
        {
            var path = Config.ExecutablePath;
            var server = Config.Server;
            var userId = Config.UserId;
            var password = Config.Password;

            {
                var database = $"SqlcmdHelperTest_{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}_ExecuteQueryString";

                var target = new SqlcmdHelper(path, server, userId, password);

                var appDataDirecotryPath = Path.GetFullPath(Path.Combine(Config.ProjectDirectoryPath, "App_Data"));
                var insertIntoFilePath = Path.Combine(appDataDirecotryPath, "insert_into_people.sql");

                target.ExecuteQueryString($"CREATE DATABASE {database}");
                target.ExecuteQueryString($"CREATE TABLE [dbo].[People]([Id] [bigint] NOT NULL, [Name] [nvarchar](50) NOT NULL)", database);
                target.ExecuteQueryFile(insertIntoFilePath, database);

                var connectionString = $"Data Source={server};Initial Catalog={database};User Id={userId};Password={password};Pooling=False";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM [dbo].[People]";
                        command.ExecuteScalar().Is(2);
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM [dbo].[People] ORDER BY [Id]";

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            ((long)reader["Id"]).Is(1);
                            ((string)reader["Name"]).Is("ABC");

                            reader.Read();
                            ((long)reader["Id"]).Is(2);
                            ((string)reader["Name"]).Is("\"ABC\"\"");
                        }
                    }
                }

                target.ExecuteQueryString($"DROP DATABASE {database}");
            }

            {
                var database = $"SqlcmdHelperTest_{DateTime.Now.ToString("yyyyMMdd_HHmmss_fff")}_ExecuteQueryString";

                var target = new SqlcmdHelper(path, server);

                var appDataDirecotryPath = Path.GetFullPath(Path.Combine(Config.ProjectDirectoryPath, "App_Data"));
                var insertIntoFilePath = Path.Combine(appDataDirecotryPath, "insert_into_people.sql");

                target.ExecuteQueryString($"CREATE DATABASE {database}");
                target.ExecuteQueryString($"CREATE TABLE [dbo].[People]([Id] [bigint] NOT NULL, [Name] [nvarchar](50) NOT NULL)", database);
                target.ExecuteQueryFile(insertIntoFilePath, database);

                var connectionString = $"Data Source={server};Initial Catalog={database};Integrated Security=True;Pooling=False";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM [dbo].[People]";
                        command.ExecuteScalar().Is(2);
                    }

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM [dbo].[People] ORDER BY [Id]";

                        using (var reader = command.ExecuteReader())
                        {
                            reader.Read();
                            ((long)reader["Id"]).Is(1);
                            ((string)reader["Name"]).Is("ABC");

                            reader.Read();
                            ((long)reader["Id"]).Is(2);
                            ((string)reader["Name"]).Is("\"ABC\"\"");
                        }
                    }
                }

                target.ExecuteQueryString($"DROP DATABASE {database}");
            }
        }
    }
}
