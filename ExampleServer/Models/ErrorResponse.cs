using System.Text.Json.Serialization;

namespace ExampleServer.Models;

public class ErrorResponse
{
    // Constructor method (can tell because no retrun type, name is same as class)
    public ErrorResponse(string message)
    {
        Message = message;
    }

    // Property
    [JsonPropertyName("mssage")]
    public string Message { get; }
}