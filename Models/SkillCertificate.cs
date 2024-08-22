namespace JobApplicationApi.Models
{
    public class SkillCertificate
    {
        public int Id { get; set; }
        public string? CertificateName { get; set; }
        public string? CertificateFile { get; set; }
        public int UserId { get; set; }
    }
}
