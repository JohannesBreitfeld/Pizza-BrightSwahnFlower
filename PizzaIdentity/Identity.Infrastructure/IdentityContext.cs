using Identity.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class IdentityContext : IdentityDbContext<IdentityUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RefreshToken>()
            .HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .IsRequired();
    }
}