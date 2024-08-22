using System;

namespace JobApplicationApi.Models
{
    public class JobExperience
    {
        public int Id { get; set; }
        public string? Company { get; set; }
        public string? Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Responsibilities { get; set; }
        public int UserId { get; set; }

        // public DateTime? StartDateUtc
        // {
        //     get => StartDate.HasValue ? DateTime.SpecifyKind(StartDate.Value, DateTimeKind.Utc) : (DateTime?)null;
        //     set => StartDate = value;
        // }

        // public DateTime? EndDateUtc
        // {
        //     get => EndDate.HasValue ? DateTime.SpecifyKind(EndDate.Value, DateTimeKind.Utc) : (DateTime?)null;
        //     set => EndDate = value;
        // }
    }
}
