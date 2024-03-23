namespace Database.Options;

public class DbConnectionOptions
{
    public const string SectionName = "DBConfig";
    public const string ConnectionStringConfigString = SectionName + ":ConnectionString";
    public const string UseAutoMigrationConfigString = SectionName + ":UseAutoMigration";
}