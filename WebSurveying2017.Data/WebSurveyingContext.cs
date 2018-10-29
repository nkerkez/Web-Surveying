using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using WebSurveying2017.Model.Model;
using System.Data.Entity;

namespace WebSurveying2017.Data
{
    public class WebSurveyingContext : IdentityDbContext<ApplicationUser, MyRole, int, MyLogin, MyUserRole, MyClaim>
    {
        public WebSurveyingContext() : base("WebSurveyingDB")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }
        public static WebSurveyingContext Create()
        {
            return new WebSurveyingContext();
        }

        public virtual void Commit()
        {
            this.SaveChanges();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserNotification> UsersNotifications { get; set; }
        public DbSet<LikeOrDislikeComment> LikeOrDislikeComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserSurvey> UsersSurvays { get; set; }
        public DbSet<UserAnswer> UsersAnswers { get; set; }
        public DbSet<UserCategory> UsersCategories { get; set; }
        public DbSet<UserGroup> UsersGroups { get; set; }
        public DbSet<ExcelFiles> ExcelFiles { get; set; }
        public DbSet<RequestGroup> Requests { get; set; }
        public DbSet<Group> Groups { get; set; }


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


            modelBuilder
                        .Entity<Comment>()
                        .HasOptional(c => c.ParentComment)
                        .WithMany()
                        .HasForeignKey(c => c.ParentId);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //delete all questions
            modelBuilder
                       .Entity<Question>()
                       .HasRequired(p => p.Survey)
                       .WithMany(x => x.Questions)
                       .WillCascadeOnDelete(true);
            
            
            

            modelBuilder
                       .Entity<UserNotification>()
                       .HasRequired(p => p.Notification)
                       .WithMany(x => x.UsersNotifications)
                       .WillCascadeOnDelete(true);

            modelBuilder
                       .Entity<UserGroup>()
                       .HasRequired(p => p.Group)
                       .WithMany(x => x.GroupMembers)
                       .WillCascadeOnDelete(true);

            modelBuilder
                       .Entity<SurveyGroup>()
                       .HasRequired(p => p.Group)
                       .WithMany(x => x.Surveys)
                       .WillCascadeOnDelete(true);

            

            modelBuilder
                      .Entity<SurveyGroup>()
                      .HasRequired(p => p.Survey)
                      .WithMany(x => x.Groups)
                      .WillCascadeOnDelete(true);


            modelBuilder
                      .Entity<Question>()
                      .HasRequired(p => p.Survey)
                      .WithMany(x => x.Questions)
                      .WillCascadeOnDelete(true);

            modelBuilder
                      .Entity<UserSurvey>()
                      .HasRequired(p => p.Survey)
                      .WithMany(x => x.Users)
                      .WillCascadeOnDelete(true);

            modelBuilder
                      .Entity<FavoriteSurveys>()
                      .HasRequired(p => p.Survey)
                      .WithMany(x => x.FavoriteSurveys)
                      .WillCascadeOnDelete(true);

            modelBuilder
                      .Entity<ExcelFiles>()
                      .HasRequired(p => p.Survey)
                      .WithMany(x => x.ExcelFiles)
                      .WillCascadeOnDelete(true);

            modelBuilder
                      .Entity<Notification>()
                      .HasOptional(p => p.Survey)
                      .WithMany(x => x.Notifications)
                      .WillCascadeOnDelete(true);
            modelBuilder
                     .Entity<Notification>()
                     .HasOptional(p => p.Comment)
                     .WithMany(x => x.Notifications)
                     .WillCascadeOnDelete(true);

            modelBuilder
                     .Entity<Notification>()
                     .HasOptional(p => p.Group)
                     .WithMany(x => x.Notifications)
                     .WillCascadeOnDelete(true);

            modelBuilder
                     .Entity<RequestGroup>()
                     .HasRequired(p => p.Group)
                     .WithMany(x => x.Requests)
                     .WillCascadeOnDelete(true);

            modelBuilder
                      .Entity<LikeOrDislikeComment>()
                      .HasRequired(p => p.Comment)
                      .WithMany(x => x.LikesOrDislikes)
                      .WillCascadeOnDelete(true);


        }
    }
}
