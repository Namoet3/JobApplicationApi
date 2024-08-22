using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JobApplicationApi.Models; 
using JobApplicationApi.Data;
// using JobApplicationApi.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;


namespace JobApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<UserController> _logger;
        // private readonly ApplicationStageRepository _stageRepository;
        private async Task<string?> GetLocationFromIpAsync(string ipAddress)
        {
            try
            {
                var apiKey = "API KEY"; 
                var url = $"http://api.ipstack.com/{ipAddress}?access_key={apiKey}";

                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var locationData = JsonConvert.DeserializeObject<dynamic>(response);

                    if (locationData == null)
                    {
                        _logger.LogWarning($"Could not retrieve location data for IP {ipAddress}. Response was null.");
                        return null;
                    }

                    string? city = locationData.city?.ToString();
                    string? country = locationData.country_name?.ToString();

                    if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(country))
                    {
                        return $"{city}, {country}";
                    }
                    else
                    {
                        _logger.LogWarning($"City or Country name is missing for IP {ipAddress}.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching location for IP {ipAddress}: {ex.Message}");
                return null;
            }
        }

        

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public string GetTurkishDayOfWeek(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Pazartesi",
                DayOfWeek.Tuesday => "Salı",
                DayOfWeek.Wednesday => "Çarşamba",
                DayOfWeek.Thursday => "Perşembe",
                DayOfWeek.Friday => "Cuma",
                DayOfWeek.Saturday => "Cumartesi",
                DayOfWeek.Sunday => "Pazar",
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null)
            };
        }
        private string GetClientIpAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new InvalidOperationException("Unable to determine the client's IP address.");
            }
            if (ipAddress == "::1") ipAddress = "127.0.0.1"; 
            return ipAddress;
        }
        


        public UserController(UserDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
            // _stageRepository = stageRepository; 
        }
        
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                return await _context.Users.Include(u => u.UserDetails)
                                           .Include(u => u.JobExperiences)
                                           .Include(u => u.References)
                                           .Include(u => u.SkillCertificates)
                                           .Include(u => u.Memberships)
                                           .Include(u => u.Languages)
                                           .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUsers: Hata {ex.Message}"); 
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.UserDetails)
                                               .Include(u => u.JobExperiences)
                                               .Include(u => u.References)
                                               .Include(u => u.SkillCertificates)
                                               .Include(u => u.Memberships)
                                               .Include(u => u.Languages)
                                               .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUser: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            try
            {
                if (user.UserDetails == null)
                {
                    return BadRequest("UserDetails cannot be null.");
                }

                user.CvFile = user.CvFile ?? ""; 
                user.CriminalRecordFile = ""; 
                if (user.SkillCertificates != null && user.SkillCertificates.Count > 0)
                {
                    foreach (var certificate in user.SkillCertificates)
                    {
                        
                        certificate.CertificateFile = certificate.CertificateFile ?? ""; 
                    }
                }

                user.ConvertDatesToUtc(); 
                user.GraduateDate = user.GraduateDate.HasValue ? DateTime.SpecifyKind(user.GraduateDate.Value, DateTimeKind.Utc) : (DateTime?)null;
                user.MilitaryDate = user.MilitaryDate.HasValue ? DateTime.SpecifyKind(user.MilitaryDate.Value, DateTimeKind.Utc) : (DateTime?)null;
                user.CriminalDate = user.CriminalDate.HasValue ? DateTime.SpecifyKind(user.CriminalDate.Value, DateTimeKind.Utc) : (DateTime?)null;

                var ipAddress = GetClientIpAddress();
                user.IpAddress = ipAddress;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

            
                // await _stageRepository.LogStageCompletionAsync(user.Id.ToString(), "Profile Completion");

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database update error in PostUser: {dbEx.Message} Inner Exception: {dbEx.InnerException?.Message}");
                if (dbEx.InnerException != null)
                {
                    _logger.LogError($"Inner Exception Stack Trace: {dbEx.InnerException.StackTrace}");
                }
                return StatusCode(500, "Internal server error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in PostUser: {ex.Message} Inner Exception: {ex.InnerException?.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner Exception Stack Trace: {ex.InnerException.StackTrace}");
                }
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, int userId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var sanitizedFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, sanitizedFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                
                return Ok(new { FileName = sanitizedFileName });
            }
            catch (Exception ex)
            {
                _logger.LogError($"File upload failed: {ex.Message}");
                return StatusCode(500, "Internal server error while uploading the file.");
            }
        }
        [HttpGet("files")]
        public IActionResult GetUploadedFiles()
        {
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    return NotFound("Uploads directory does not exist.");
                }

                var files = Directory.GetFiles(uploadsFolder)
                                    .Select(Path.GetFileName)
                                    .ToList();

                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving files: {ex.Message}");
                return StatusCode(500, "Internal server error while retrieving files.");
            }
        }


        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }



       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            
            try
            {
                if (id != user.Id)
                {
                    return BadRequest();
                }

                if (user.UserDetails == null)
                {
                    return BadRequest("UserDetails cannot be null.");
                }

                user.ConvertDatesToUtc(); 

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in PutUser: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("deleted-users")]
        public async Task<ActionResult<IEnumerable<DeletedUser>>> GetDeletedUsers()
        {
            try
            {
                var deletedUsers = await _context.DeletedUsers
                                                .Include(d => d.UserDetails) 
                                                .ToListAsync();

                return Ok(deletedUsers);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Database error fetching deleted users: {ex.Message}");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching deleted users: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, "Internal server error");
            }
}

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.Include(u => u.UserDetails)
                                            .Include(u => u.JobExperiences)
                                            .Include(u => u.References)
                                            .Include(u => u.SkillCertificates)
                                            .Include(u => u.Memberships)
                                            .Include(u => u.Languages)
                                            .FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
                 

                
                var deletedUser = new DeletedUser
                {
                    UserDetailsId = user.UserDetailsId,
                    UserDetails = user.UserDetails,
                    EducationLevel = user.EducationLevel,
                    Highschool = user.Highschool,
                    University = user.University,
                    Program = user.Program,
                    GraduateDate = user.GraduateDate,
                    Gpa = user.Gpa,
                    MilitaryStatus = user.MilitaryStatus,
                    MilitaryDate = user.MilitaryDate,
                    HealthStat = user.HealthStat,
                    Health = user.Health,
                    Disability = user.Disability,
                    Criminal = user.Criminal,
                    CriminalDate = user.CriminalDate,
                    CriminalReason = user.CriminalReason,
                    CriminalRecordFile = user.CriminalRecordFile,
                    JobExperiences = user.JobExperiences,
                    References = user.References,
                    Skills = user.Skills,
                    SkillCertificates = user.SkillCertificates,
                    Hobbies = user.Hobbies,
                    Memberships = user.Memberships,
                    Languages = user.Languages,
                    CvFile = user.CvFile,
                    Policy = user.Policy,
                    SubmissionDate = user.SubmissionDate,
                    DeletedAt = DateTime.UtcNow
                };

                
                _context.DeletedUsers.Add(deletedUser);

                
                _context.Users.Remove(user);

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteUser: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        

        [HttpDelete("delete-all-logs")]
        public async Task<IActionResult> ClearAllLogs()
        {
            try
            {
                var lastClearAction = await _context.ActionLogs
                                                    .Where(log => log.Action == "Clear All Logs")
                                                    .OrderByDescending(log => log.Timestamp)
                                                    .FirstOrDefaultAsync();

                var logsToDelete = _context.ActionLogs
                                           .Where(log => log.Action != "Clear All Logs")
                                           .ToList();

                if (logsToDelete.Any())
                {
                    _context.ActionLogs.RemoveRange(logsToDelete);
                    await _context.SaveChangesAsync();
                }

                if (lastClearAction != null)
                {
                    _context.ActionLogs.Remove(lastClearAction);
                    await _context.SaveChangesAsync();
                    
                    lastClearAction.Timestamp = DateTime.UtcNow;
                    _context.ActionLogs.Add(lastClearAction);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error clearing logs: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("delete-all-deleted-users")]
        public async Task<IActionResult> DeleteAllDeletedUsers()
        {
            try
            {
                var allDeletedUsers = _context.DeletedUsers.ToList();

                if (allDeletedUsers.Any())
                {
                    _context.DeletedUsers.RemoveRange(allDeletedUsers);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting all deleted users: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("delete-selected-deleted-users")]
        public async Task<IActionResult> DeleteSelectedDeletedUsers([FromBody] List<int> userIds)
        {
            try
            {
                if (userIds == null || !userIds.Any())
                {
                    return BadRequest("No user IDs provided.");
                }

                var selectedDeletedUsers = _context.DeletedUsers.Where(u => userIds.Contains(u.Id)).ToList();

                if (selectedDeletedUsers.Any())
                {
                    _context.DeletedUsers.RemoveRange(selectedDeletedUsers);
                    await _context.SaveChangesAsync();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting selected deleted users: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("last-clear-action")]
        public async Task<ActionResult<ActionLog>> GetLastClearAction()
        {
            try
            {
                var lastClearAction = await _context.ActionLogs
                    .Where(log => log.Action == "Clear All Logs")
                    .OrderByDescending(log => log.Timestamp)
                    .FirstOrDefaultAsync();

                if (lastClearAction == null)
                {
                    return NotFound("No clear logs action found.");
                }

                return Ok(lastClearAction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching last clear action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("validate-password")]
        public IActionResult ValidatePassword([FromBody] PasswordValidationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid password data.");
            }

            const string correctPassword = "123456";

            if (request.Password == correctPassword)
            {
                return Ok();
            }

            return Unauthorized();
        }

        public class PasswordValidationRequest
        {
            public string? Password { get; set; }
        }

        [HttpPost("action-logs")]
        public async Task<IActionResult> LogAction([FromBody] ActionLog actionLog)
        {
            try
            {
                if (actionLog == null || string.IsNullOrEmpty(actionLog.Action) || string.IsNullOrEmpty(actionLog.PerformedBy))
                {
                    return BadRequest("Invalid log data.");
                }

                
                actionLog.Timestamp = DateTime.UtcNow;
                _context.ActionLogs.Add(actionLog);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging action: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("action-logs")]
        public async Task<ActionResult<IEnumerable<ActionLog>>> GetActionLogs()
        {
            try
            {
                var logs = await _context.ActionLogs.OrderByDescending(log => log.Timestamp).ToListAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching action logs: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("recover/{id}")]
        public async Task<IActionResult> RecoverUser(int id)
        {
            var deletedUser = await _context.DeletedUsers.Include(u => u.UserDetails)
                                                        .FirstOrDefaultAsync(u => u.Id == id);
            if (deletedUser == null)
            {
                return NotFound();
            }

            
            var user = new User
            {
                UserDetailsId = deletedUser.UserDetailsId,
                UserDetails = deletedUser.UserDetails,
                EducationLevel = deletedUser.EducationLevel,
                Highschool = deletedUser.Highschool,
                University = deletedUser.University,
                Program = deletedUser.Program,
                GraduateDate = deletedUser.GraduateDate,
                Gpa = deletedUser.Gpa,
                MilitaryStatus = deletedUser.MilitaryStatus,
                MilitaryDate = deletedUser.MilitaryDate,
                HealthStat = deletedUser.HealthStat,
                Health = deletedUser.Health,
                Disability = deletedUser.Disability,
                Criminal = deletedUser.Criminal,
                CriminalDate = deletedUser.CriminalDate,
                CriminalReason = deletedUser.CriminalReason,
                CriminalRecordFile = deletedUser.CriminalRecordFile,
                JobExperiences = deletedUser.JobExperiences,
                References = deletedUser.References,
                Skills = deletedUser.Skills,
                SkillCertificates = deletedUser.SkillCertificates,
                Hobbies = deletedUser.Hobbies,
                Memberships = deletedUser.Memberships,
                Languages = deletedUser.Languages,
                CvFile = deletedUser.CvFile,
                Policy = deletedUser.Policy,
                SubmissionDate = deletedUser.SubmissionDate
            };

            _context.Users.Add(user);
            _context.DeletedUsers.Remove(deletedUser);

            await _context.SaveChangesAsync();

            return Ok(user);
        }
        [HttpGet("applications/today")]
        public async Task<ActionResult<int>> GetApplicationsToday()
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var count = await _context.Users.CountAsync(u => u.SubmissionDate.Date == today);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching applications today: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("applications/this-week")]
        public async Task<ActionResult<int>> GetApplicationsThisWeek()
        {
            try
            {
                var startOfWeek = DateTime.UtcNow.AddDays(-(int)DateTime.UtcNow.DayOfWeek).Date;
                var count = await _context.Users.CountAsync(u => u.SubmissionDate >= startOfWeek);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching applications this week: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("page-visits/today")]
        public async Task<ActionResult<int>> GetPageVisitsToday([FromQuery] string pageUrl)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var count = await _context.ActionLogs.CountAsync(log => log.Action == "Page Visit" && log.Timestamp.Date == today && log.PageUrl == pageUrl);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching page visits today: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("page-visits/this-week")]
        public async Task<ActionResult<int>> GetPageVisitsThisWeek()
        {
            try
            {
                var startOfWeek = DateTime.UtcNow.AddDays(-(int)DateTime.UtcNow.DayOfWeek).Date;
                var count = await _context.PageVisitLogs.CountAsync(log => log.Timestamp >= startOfWeek);
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching page visits this week: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("applications/popular-time")]
        public async Task<ActionResult<string>> GetPopularApplicationTime()
        {
            try
            {
                var popularHour = await _context.Users
                    .Where(u => u.SubmissionDate != DateTime.MinValue)
                    .GroupBy(u => u.SubmissionDate.Hour)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync();

                if (popularHour == default(int))
                {
                    return NotFound(new { message = "No applications found." });
                }

                return Ok(new { timeRange = $"{popularHour}:00 - {popularHour + 1}:00" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching popular application time: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }


        [HttpGet("applications/busiest-day")]
        public async Task<ActionResult<string>> GetBusiestDay()
        {
            try
            {
                var popularDay = await _context.Users
                    .Where(u => u.SubmissionDate != DateTime.MinValue)
                    .GroupBy(u => u.SubmissionDate.DayOfWeek)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefaultAsync();

                if (popularDay == default)
                {
                    return NotFound(new { message = "No applications found." });
                }

                var turkishDay = GetTurkishDayOfWeek(popularDay);
                return Ok(new { day = turkishDay });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching busiest day: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
        public class PageVisitLog
        {
            public string? PageUrl { get; set; }
        }
        [HttpGet("page-visit-logs")]
        public async Task<ActionResult<IEnumerable<PageVisitLog>>> GetPageVisitLogs()
        {
            try
            {
                var logs = await _context.PageVisitLogs.OrderByDescending(log => log.Timestamp).ToListAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching page visit logs: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("submit-form")]
        public async Task<IActionResult> SubmitForm([FromBody] User user)
        {
            try
            {
                if (user == null || user.UserDetails == null)
                {
                    return BadRequest("Invalid user data.");
                }

                
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ipAddress == "::1") ipAddress = "127.0.0.1";  

                
                var existingUser = await _context.Users.Include(u => u.UserDetails)
                                                    .FirstOrDefaultAsync(u => u.IpAddress == ipAddress);

                if (existingUser != null)
                {
                    
                    existingUser.UserDetails.FirstName = user.UserDetails.FirstName;
                    existingUser.UserDetails.LastName = user.UserDetails.LastName;
                    existingUser.IpAddress = ipAddress;
                }
                else
                {
                    
                    user.IpAddress = ipAddress;
                    _context.Users.Add(user);
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error submitting form: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("log-page-visit")]
        public async Task<IActionResult> LogPageVisit([FromBody] Models.PageVisitLog pageVisitLog)
        {
            try
            {
                if (pageVisitLog == null || string.IsNullOrEmpty(pageVisitLog.PageUrl))
                {
                    return BadRequest("Invalid page visit log data.");
                }

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (ipAddress == "::1") ipAddress = "127.0.0.1";  

                var user = await _context.Users.Include(u => u.UserDetails)
                                            .FirstOrDefaultAsync(u => u.IpAddress == ipAddress);

                if (user != null && user.UserDetails != null)
                {
                    pageVisitLog.PerformedBy = $"{user.UserDetails.FirstName} {user.UserDetails.LastName}";
                }
                else
                {
                    pageVisitLog.PerformedBy = "Anonymous";
                }

                pageVisitLog.Timestamp = DateTime.UtcNow;
                _context.PageVisitLogs.Add(pageVisitLog);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging page visit: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("page-visits/user-form")]
        public async Task<ActionResult<int>> GetUserFormPageVisits()
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var count = await _context.ActionLogs.CountAsync(log => log.Action == "Page Visit" && log.PageUrl == "http://localhost:4200/user-form");
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching user-form page visits: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        public class StatusUpdateRequest
        {
            public string? Status { get; set; }
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] StatusUpdateRequest request)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Status = request.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        

        
    }
}
