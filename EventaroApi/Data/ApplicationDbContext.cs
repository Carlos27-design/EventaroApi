using EventaroApi.Entities;
using EventaroApi.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventaroApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options){}

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Ubication> Ubications { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventImg> EventImgs { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Event>().Property(e => e.Status).HasConversion<string>();
            builder.Entity<Event>().Property(e => e.StatusEvent).HasConversion<string>();

            builder.Entity<Inscription>().Property(i => i.Status).HasConversion<string>();
            builder.Entity<Inscription>().Property(i => i.StatusInscription).HasConversion<string>();

            builder.Entity<User>().Property(u => u.Status).HasConversion<string>();
            builder.Entity<Organization>().Property(o => o.Status).HasConversion<string>();
            builder.Entity<EventType>().Property(et => et.Status).HasConversion<string>();
            builder.Entity<Ubication>().Property(u => u.Status).HasConversion<string>();
            builder.Entity<EventImg>().Property(ei => ei.Status).HasConversion<string>();

            base.OnModelCreating(builder);

            //Configurar las relaciones desde las entidades
            builder.Entity<User>()
                .HasOne(u => u.Organization)
                .WithMany(o => o.Users)
                .HasForeignKey(u => u.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                .HasOne(e => e.Organization)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>()
                .HasOne(e => e.Ubication)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UbicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<EventImg>()
                .HasOne(ei => ei.Event)
                .WithMany(e => e.EventImgs)
                .HasForeignKey(ei => ei.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inscription>()
                .HasOne(i => i.User)
                .WithMany(u => u.Inscriptions)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Inscription>()
                .HasOne(i => i.Event)
                .WithMany(e => e.Inscriptions)
                .HasForeignKey(i => i.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Organization>().HasQueryFilter(o => o.Status != StatusBase.Delete);
            builder.Entity<User>().HasQueryFilter(u => u.Status != StatusBase.Delete);
            builder.Entity<EventType>().HasQueryFilter(et => et.Status != StatusBase.Delete);
            builder.Entity<Ubication>().HasQueryFilter(u => u.Status != StatusBase.Delete);
            builder.Entity<EventImg>().HasQueryFilter(ei => ei.Status != StatusBase.Delete);
            builder.Entity<Event>().HasQueryFilter(e => e.Status != StatusBase.Delete);
            builder.Entity<Inscription>().HasQueryFilter(i => i.Status != StatusBase.Delete);
        }

    }
}
