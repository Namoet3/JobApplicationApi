namespace JobApplicationApi.Models
{
    public class Reference
    {
        public int Id { get; set; }
        public string? ReferenceName { get; set; }
        public string? ReferenceCompany { get; set; }
        public string? ReferencePhone { get; set; }
        public string? ReferenceEmail { get; set; }
        public int UserId { get; set; }
    }
}
