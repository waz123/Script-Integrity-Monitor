namespace Security_App__Blazor.Data.Models;

public class Policy
{
    public string Name { get; set; }
    public DateTime CompletedOn { get; set; }
    public DateTime DueDate { get; set; }
    public string CompletedBy { get; set; }
}

