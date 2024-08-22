
namespace JobApplicationApi.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string? LanguageName { get; set; }
        public string? Proficiency { get; set; }
        public int UserId { get; set; }
    }
}
