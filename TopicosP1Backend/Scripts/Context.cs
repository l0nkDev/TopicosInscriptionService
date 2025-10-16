using CareerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace TopicosP1Backend.Scripts
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options)
        : base(options)
        {
        }

        public DbSet<Career> Careers { get; set; } = default!;
        public DbSet<Gestion> Gestions { get; set; } = default!;
        public DbSet<Group> Groups { get; set; } = default!;
        public DbSet<GroupInscription> GroupInscriptions { get; set; } = default!;
        public DbSet<Inscription> Inscriptions { get; set; } = default!;
        public DbSet<Module> Modules { get; set; } = default!;
        public DbSet<Period> Periods { get; set; } = default!;
        public DbSet<Room> Rooms { get; set; } = default!;
        public DbSet<SpSubject> SpSubjects { get; set; } = default!;
        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<StudentGroups> StudentGroups { get; set; } = default!;
        public DbSet<StudentStudyPlan> StudentStudyPlans { get; set; } = default!;
        public DbSet<StudyPlan> StudyPlans { get; set; } = default!;
        public DbSet<Subject> Subjects { get; set; } = default!;
        public DbSet<SubjectDependency> SubjectDependencies { get; set; } = default!;
        public DbSet<Teacher> Teachers { get; set; } = default!;
        public DbSet<TimeSlot> TimeSlots { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public char GetRequestUserRole(HttpRequest request)
        {
            string token = request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            Console.WriteLine(token);
            var user = Users.FirstOrDefault(x => x.Token == token);
            if (user == null) { Console.WriteLine("User not found."); return 'n'; }
            return user.Role;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>()
                .HasMany(e => e.StudyPlans)
                .WithMany(e => e.Subjects)
                .UsingEntity<SpSubject>();

            modelBuilder.Entity<StudyPlan>()
                .HasMany(e => e.Subjects)
                .WithMany(e => e.StudyPlans)
                .UsingEntity<SpSubject>();

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Inscriptions)
                .WithMany(e => e.Groups)
                .UsingEntity<GroupInscription>();

            modelBuilder.Entity<Inscription>()
                .HasMany(e => e.Groups)
                .WithMany(e => e.Inscriptions)
                .UsingEntity<GroupInscription>();

            modelBuilder.Entity<Student>()
                .HasMany(e => e.StudyPlans)
                .WithMany(e => e.Students)
                .UsingEntity<StudentStudyPlan>();

            modelBuilder.Entity<StudyPlan>()
                .HasMany(e => e.Students)
                .WithMany(e => e.StudyPlans)
                .UsingEntity<StudentStudyPlan>();

            modelBuilder.Entity<Subject>()
                .HasMany(e => e.Prerequisites)
                .WithMany(e => e.Postrequisites)
                .UsingEntity<SubjectDependency>(
                    r => r.HasOne(e => e.Prerequisite).WithMany(e => e.PostDependencies),
                    s => s.HasOne(e => e.Postrequisite).WithMany(e => e.PreDependencies)
                );

            modelBuilder.Entity<StudyPlan>().Navigation(_ => _.Subjects).AutoInclude();
            modelBuilder.Entity<StudyPlan>().Navigation(_ => _.Career).AutoInclude();
            modelBuilder.Entity<Subject>().Navigation(_ => _.PreDependencies).AutoInclude();
            modelBuilder.Entity<Subject>().Navigation(_ => _.Groups).AutoInclude();
            modelBuilder.Entity<Student>().Navigation(_ => _.StudentGroups).AutoInclude();
            modelBuilder.Entity<Student>().Navigation(_ => _.StudyPlans).AutoInclude();
            modelBuilder.Entity<Group>().Navigation(_ => _.Teacher).AutoInclude();
            modelBuilder.Entity<Group>().Navigation(_ => _.Period).AutoInclude();
            modelBuilder.Entity<Group>().Navigation(_ => _.Subject).AutoInclude();
            modelBuilder.Entity<Group>().Navigation(_ => _.TimeSlots).AutoInclude();
            modelBuilder.Entity<Period>().Navigation(_ => _.Gestion).AutoInclude();
            modelBuilder.Entity<Career>().Navigation(_ => _.StudyPlans).AutoInclude();
        }
    }

    public static class Extensions
    {
        public static void CreateDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<Context>();
            context.Database.EnsureCreated();
            DatabaseInitialization.Populate(context);
        }
    }
}
