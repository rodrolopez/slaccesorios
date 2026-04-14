namespace YuyoDev.AdminPanel.Models;

public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

// Clase genérica para envolver las respuestas de tu Result<T> del backend
public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; } = string.Empty;
    public T Value { get; set; } = default!;
}