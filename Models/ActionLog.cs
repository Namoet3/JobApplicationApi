using System;
using System.Collections.Generic;

namespace JobApplicationApi.Models
{
public class ActionLog
{
    public int Id { get; set; }
    public string? Action { get; set; } 
    public string? PerformedBy { get; set; } 
    public DateTime Timestamp { get; set; }
    public string? Details { get; set; } 
    public string? PageUrl { get; set; }
}
}