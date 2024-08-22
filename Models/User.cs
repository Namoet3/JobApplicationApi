using System;
using System.Collections.Generic;

namespace JobApplicationApi.Models
{
    public class User
{
    public int Id { get; set; }
    public int UserDetailsId { get; set; }
    public UserDetails UserDetails { get; set; } = null!;
    public string? EducationLevel { get; set; }
    public string? Highschool { get; set; }
    public string? University { get; set; }
    public string? Program { get; set; }
    public DateTime? GraduateDate { get; set; }
    public string? Gpa { get; set; }
    public string? MilitaryStatus { get; set; }
    public DateTime? MilitaryDate { get; set; }
    public string? HealthStat { get; set; }
    public string? Health { get; set; }
    public string? Disability { get; set; }
    public string? Criminal { get; set; }
    public DateTime? CriminalDate { get; set; }
    public string? CriminalReason { get; set; }
    public string? CriminalRecordFile { get; set; }
    public List<JobExperience> JobExperiences { get; set; } = new List<JobExperience>();
    public List<Reference> References { get; set; } = new List<Reference>();
    public string? Skills { get; set; }
    public List<SkillCertificate> SkillCertificates { get; set; } = new List<SkillCertificate>();
    public string? Hobbies { get; set; }
    public List<Membership> Memberships { get; set; } = new List<Membership>();
    public List<Language> Languages { get; set; } = new List<Language>();
    public string? CvFile { get; set; }
    public bool Policy { get; set; }
    public DateTime SubmissionDate { get; set; }
    public string? Status { get; set; }
    public string? IpAddress { get; set; }
    public string? Location { get; set; }

    
    public void ConvertDatesToUtc()
    {
        if (UserDetails != null)
        {
            UserDetails.Birthdate = DateTime.SpecifyKind(UserDetails.Birthdate, DateTimeKind.Utc);
        }
            
        if (GraduateDate.HasValue)
        {
            GraduateDate = DateTime.SpecifyKind(GraduateDate.Value, DateTimeKind.Utc);
        }
        if (MilitaryDate.HasValue)
        {
            MilitaryDate = DateTime.SpecifyKind(MilitaryDate.Value, DateTimeKind.Utc);
        }
        if (CriminalDate.HasValue)
        {
            CriminalDate = DateTime.SpecifyKind(CriminalDate.Value, DateTimeKind.Utc);
        }
        if (JobExperiences != null)
        {
            foreach (var jobExperience in JobExperiences)
            {
                jobExperience.StartDate = jobExperience.StartDate.HasValue ? DateTime.SpecifyKind(jobExperience.StartDate.Value, DateTimeKind.Utc) : (DateTime?)null;
                jobExperience.EndDate = jobExperience.EndDate.HasValue ? DateTime.SpecifyKind(jobExperience.EndDate.Value, DateTimeKind.Utc) : (DateTime?)null;
            }
        }
    }
}
}
