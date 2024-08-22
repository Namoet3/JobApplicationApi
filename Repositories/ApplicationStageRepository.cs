// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using JobApplicationApi.Data;
// using JobApplicationApi.Models;

// namespace JobApplicationApi.Repositories
// {
//     public class ApplicationStageRepository
//     {
//         private readonly UserDbContext _context;

//         public ApplicationStageRepository(UserDbContext context)
//         {
//             _context = context;
//         }

//         // Method to log a user's stage completion
//         public async Task LogStageCompletionAsync(int userId, string stage)
//         {
//             var applicationStage = new ApplicationStage
//             {
//                 UserId = userId,
//                 Stage = stage,
//                 CompletedAt = DateTime.UtcNow
//             };

//             _context.ApplicationStages.Add(applicationStage);
//             await _context.SaveChangesAsync();
//         }

//         // Method to get all stages completed by a specific user
//         public async Task<List<ApplicationStage>> GetStagesByUserAsync(int userId)
//         {
//             return await _context.ApplicationStages
//                 .Where(stage => stage.UserId == userId)
//                 .OrderBy(stage => stage.CompletedAt)
//                 .ToListAsync();
//         }

//         // Method to get the count of users at each stage
//         public async Task<Dictionary<string, int>> GetStageCountsAsync()
//         {
//             return await _context.ApplicationStages
//                 .GroupBy(stage => stage.Stage ?? "Unknown")  // Use "Unknown" or another default value for null stages
//                 .ToDictionaryAsync(group => group.Key, group => group.Count());
//         }
//     }
// }
