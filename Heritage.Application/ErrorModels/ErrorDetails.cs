using System.Text.Json;

namespace Heritage.Application.ErrorModels;

public class ErrorDetails
{
    public int StatusCode { get; init; }
    public string? Message { get; init; }

    public override string ToString() => JsonSerializer.Serialize(this);
}
