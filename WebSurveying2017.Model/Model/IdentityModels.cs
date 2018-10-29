using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSurveying2017.Model.Model
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    public class MyUserRole : IdentityUserRole<int>, IModelBase
    {
        [NotMapped]
        public int Id { get; set; }
    }
    public class MyRole : IdentityRole<int, MyUserRole>, IModelBase
    {

    }

    public class MyLogin : IdentityUserLogin<int>
    {

    }

    public class MyClaim : IdentityUserClaim<int>
    {

    }


    public class ApplicationUser : IdentityUser<int, MyLogin, MyUserRole, MyClaim>, IModelBase
    {
        public virtual UserExtend User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }


    }

    public enum Gender
    {
        MALE = 1,
        FEMALE
    }


    
    /*
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, MyRole, int, MyLogin, MyUserRole, MyClaim>
    {
        public ApplicationDbContext()
            : base("WebSurveyingDB")
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("MyUser");
            modelBuilder.Entity<MyUserRole>().ToTable("UserRole");
            modelBuilder.Entity<MyRole>().ToTable("Role");
            modelBuilder.Entity<MyLogin>().ToTable("UserLogin");
            modelBuilder.Entity<MyClaim>().ToTable("UserClaim");

            modelBuilder.Entity<ApplicationUser>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MyRole>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MyClaim>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }
    }
    */
}