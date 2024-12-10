using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ViaAPI.Migrations;
using ViaAPI.Models;

namespace ViaAPI.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }


        public DbSet<CreateUserModel> Usuario { get; set; }
        public DbSet<TourismModel> Turismo { get; set; }
        public DbSet<TravelHistoryModel> TravelHistory { get; set; }
        public DbSet<LocalizationModel> Localizacao { get; set; }
        public DbSet<FeedbackModel> Feedback { get; set; }


    }
}
