using Microsoft.EntityFrameworkCore;
using MySecretFriend.Entities;

namespace MySecretFriend.Context
{
    public class FriendsDbContext : DbContext
    {
        public DbSet<Friend> DbFriends {  get; set; }

        public FriendsDbContext(DbContextOptions<FriendsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>(entity =>
            {
                entity.ToTable("friends");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(80)");
                entity.Property(e => e.Nome).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Amigo).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Dica1).HasColumnType("text");
                entity.Property(e => e.Dica2).HasColumnType("text");
                entity.Property(e => e.Dica3).HasColumnType("text");
                entity.Property(e => e.Dica4).HasColumnType("text");
            });
        }
    }
}
