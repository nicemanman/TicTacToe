namespace Database.Options;

/// <summary>
/// Типизированная конфигурация подключения к бд
/// </summary>
/// <remarks>CloudWMS-26</remarks>
/// <author>v.vorobiev@axelot.ru</author>
public class DbConnectionOptions
{
    public const string SectionName = "DBConfig";
    public const string ConnectionStringConfigString = SectionName + ":ConnectionString";
    public const string UseAutoMigrationConfigString = SectionName + ":UseAutoMigration";
}