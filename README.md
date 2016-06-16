SqlcmdHelper
====

Helps sqlcmd.exe automation. Using SqlcmdHelper, you can execute query (string and .sql) by code.

## Installation

```
PM> Install-Package SqlcmdHelper
```

## Usage

### Execute query (string)

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

or

```
static class Config
{
    public const string ExecutablePath = @"C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\SQLCMD.EXE";
    public const string Server = ".";
    public const string Database = "MyDatabase";
}

// Using windows authentication
var helper = new SqlcmdHelper(Config.ExecutablePath, Config.Server);

helper.ExecuteQueryString($"CREATE DATABASE {Config.Database}");
helper.ExecuteQueryString($"CREATE TABLE [dbo].[People]([Id] [bigint] NOT NULL, [Name] [nvarchar](50) NOT NULL)", Config.Database);
helper.ExecuteQueryString($"INSERT INTO [dbo].[People] ([Id], [Name]) VALUES (1, 'ABC')", Config.Database);
```

### Execute query (.sql)

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

or

```
static class Config
{
    public const string ExecutablePath = @"C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\SQLCMD.EXE";
    public const string Server = ".";
    public const string Database = "MyDatabase";
}

// Using windows authentication
var helper = new SqlcmdHelper(Config.ExecutablePath, Config.Server);

helper.ExecuteQueryFile("create_database.sql");
helper.ExecuteQueryFile("create_table.sql");
helper.ExecuteQueryFile("insert_into.sql");
```

## License
----

See [LICENSE](LICENSE)
