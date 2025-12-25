namespace ITHelpDesk.Models;

public class Ticket
{
    public int TicketId { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public int RequesterId { get; set; }
    public string? RequesterName { get; set; }
    public int? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
    public int PriorityId { get; set; }
    public string? PriorityName { get; set; }
    public string? PriorityColor { get; set; }
    public int StatusId { get; set; }
    public string? StatusName { get; set; }
    public string? StatusColor { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? ClosedDate { get; set; }
    public int TotalRecordCount { get; set; }
}