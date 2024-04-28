namespace Security_App__Blazor.Data.Models;

public class ScriptModel
{
    public string Src { get; set; }
    public string Hash { get; set; }
    public bool? Allowed { get; set; }

    public string Subdomain { get; set; }
    public string Type { get; set; }
}
