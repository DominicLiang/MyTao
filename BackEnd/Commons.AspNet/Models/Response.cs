namespace Commons.AspNet.Models;

public class Response
{
    public bool Successed { get; set; }
    public string Message { get; set; }
    public object? Data { get; set; }
    public List<string> Errors { get; }
    public Response(bool successed, string message, object? data)
    {
        Successed = successed;
        Message = message;
        Data = data;
        Errors = new List<string>();
    }
}