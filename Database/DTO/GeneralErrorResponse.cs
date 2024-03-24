namespace Database.DTO;

/// <summary>
/// Объект, который вернется клиенту в случае общих ошибок
/// </summary>
public class GeneralErrorResponse
{
    public string Error { get; set; }
}