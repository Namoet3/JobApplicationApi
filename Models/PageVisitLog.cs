using System;
using System.Collections.Generic;

namespace JobApplicationApi.Models
{
public class PageVisitLog
{
    public int Id { get; set; }
    public string? PageUrl { get; set; }
    public DateTime Timestamp { get; set; }
    public string PerformedBy { get; set; } = "Anonymous";
    public string? IpAddress { get; set; } 
    public string? Location { get; set; }  
}
}