using System.Net;

public class RequestModelBase
{
    public string Route { get; set; }
    
    public IDictionary<string, string> Headers { get; set; } = null;

    public object Body { get; set; } = null;
}