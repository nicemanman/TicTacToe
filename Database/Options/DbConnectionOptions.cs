namespace Database.Options;

/// <summary>
/// Типизированная конфигурация подключения к бд
/// </summary>
/// <remarks>CloudWMS-26</remarks>
/// <author>v.vorobiev@axelot.ru</author>
public class DbConnectionOptions
{
    public const string SectionName = "DBConfig";

    /// <summary>
    /// Строка подключения
    /// </summary>
    /// <remarks>CloudWMS-26</remarks>
    /// <author>v.vorobiev@axelot.ru</author>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Нужна ли миграция при запуске
    /// </summary>
    /// <remarks>CloudWMS-78</remarks>
    /// <author>m.shkodin@axelot.ru</author>
    public bool UseAutoMigration { get; set; } = false;
}