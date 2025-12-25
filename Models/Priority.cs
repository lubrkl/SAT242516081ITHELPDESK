namespace ITHelpDesk.Models;

public class Priority
{
    public int PriorityId { get; set; }
    public string PriorityName { get; set; } = "";
    public string? ColorCode { get; set; }
}