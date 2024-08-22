using System;

namespace JobApplicationApi.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string? Birthplace { get; set; }
        public string? Gender { get; set; }
        public string? MarriageStatus { get; set; }
        public string? Nationality { get; set; }
        public string? TcNo { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Postal { get; set; }
        public string? TelNum { get; set; }
        public string? Email { get; set; }
    }
}
