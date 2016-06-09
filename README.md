SqlcmdHelper
====

Helps sqlcmd.exe automation. Using SqlcmdHelper, you can execute query (string and .sql) by code.


## Installation

```
PM> Install-Package SqlcmdHelper
```

See also [nuget.org](https://www.nuget.org/packages/SqlcmdHelper/)

## Usage

```
static class Config
{
    public const string ExecutablePath = @"C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\SQLCMD.EXE";
    public const string Server = ".";
    public const string UserId = "sa";
    public const string Password = "P@ssw0rd";
    public const string Database = "MyDatabase";
}

var helper = new SqlcmdHelper(Config.ExecutablePath, Config.Server, Config.UserId, config.Password);

helper.ExecuteQueryString($"CREATE DATABASE {Config.Database}");
helper.ExecuteQueryString($"CREATE TABLE [dbo].[People]([Id] [bigint] NOT NULL, [Name] [nvarchar](50) NOT NULL)", Config.Database);
helper.ExecuteQueryString($"INSERT INTO [dbo].[People] ([Id], [Name]) VALUES (1, 'ABC')", Config.Database);
```

```
static class Config
{
    public const string ExecutablePath = @"C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\SQLCMD.EXE";
    public const string Server = ".";
    public const string UserId = "sa";
    public const string Password = "P@ssw0rd";
    public const string Database = "MyDatabase";
}

var helper = new SqlcmdHelper(Config.ExecutablePath, Config.Server, Config.UserId, config.Password);

helper.ExecuteQueryFile("create_database.sql");
helper.ExecuteQueryFile("create_table.sql");
helper.ExecuteQueryFile("insert_into.sql");
```


## License
----

See [LICENSE](LICENSE)
