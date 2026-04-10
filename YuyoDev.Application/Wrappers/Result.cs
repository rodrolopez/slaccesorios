namespace YuyoDev.Application.Wrappers;

public class Result<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    // Método rápido para devolver un éxito
    public static Result<T> Ok(T data, string message = "Operación exitosa")
    {
        return new Result<T> { Success = true, Message = message, Data = data };
    }

    // Método rápido para devolver un error
    public static Result<T> Fail(string message, List<string>? errors = null)
    {
        return new Result<T> { Success = false, Message = message, Errors = errors };
    }
}