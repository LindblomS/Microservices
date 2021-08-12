﻿namespace Services.Identity.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class IdentityContext : IdentityDbContext<IdentityUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUser>(b =>
            {
                b.ToTable("user", "dbo"); // efter som funkar
                b.HasKey(u => u.Id);

                b.Property(u => u.Id).HasColumnName("id");
                b.Property(u => u.UserName).HasColumnName("username");
                b.Property(u => u.PasswordHash).HasColumnName("password_hash");
                b.Property(u => u.NormalizedUserName).HasColumnName("normalized_username");

                b.Ignore(u => u.Email);
                b.Ignore(u => u.NormalizedEmail);
                b.Ignore(u => u.LockoutEnd);
                b.Ignore(u => u.TwoFactorEnabled);
                b.Ignore(u => u.PhoneNumberConfirmed);
                b.Ignore(u => u.PhoneNumber);
                b.Ignore(u => u.ConcurrencyStamp);
                b.Ignore(u => u.SecurityStamp);
                b.Ignore(u => u.EmailConfirmed);
                b.Ignore(u => u.LockoutEnabled);
                b.Ignore(u => u.AccessFailedCount);

                b.HasMany<IdentityUserClaim<string>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany<IdentityUserLogin<string>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany<IdentityUserToken<string>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
                b.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("user_claim");
                b.HasKey(uc => uc.Id);
                b.Property(uc => uc.ClaimType).HasColumnName("claim_type");
                b.Property(uc => uc.ClaimValue).HasColumnName("claim_value");
                b.Property(uc => uc.Id).HasColumnName("id");
                b.Property(uc => uc.UserId).HasColumnName("user_id");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("user_role");
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                b.Property(ur => ur.UserId).HasColumnName("user_id");
                b.Property(ur => ur.RoleId).HasColumnName("role_id");
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("role", "dbo");
                b.HasKey(r => r.Id);
                b.Property(r => r.Id).HasColumnName("id");
                b.Property(r => r.Name).HasColumnName("display_name");
                b.Ignore(r => r.NormalizedName);
                b.Ignore(r => r.ConcurrencyStamp);

                b.HasMany<IdentityUserRole<string>>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasMany<IdentityRoleClaim<string>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            });

            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("role_claim");
                b.HasKey(rc => rc.Id);
                b.Property(rc => rc.Id).HasColumnName("id");
                b.Property(rc => rc.RoleId).HasColumnName("role_id");
                b.Property(rc => rc.ClaimType).HasColumnName("claim_type");
                b.Property(rc => rc.ClaimValue).HasColumnName("claim_value");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("user_token");
                b.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
                b.Property(ut => ut.UserId).HasColumnName("user_id");
                b.Property(ut => ut.LoginProvider).HasColumnName("login_provider");
                b.Property(ut => ut.Name).HasColumnName("name");
                b.Property(ut => ut.Value).HasColumnName("[value]");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("user_login");
                b.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });
                b.Property(ul => ul.UserId).HasColumnName("user_id");
                b.Property(ul => ul.ProviderKey).HasColumnName("provider_key");
                b.Property(ul => ul.LoginProvider).HasColumnName("login_provider");
                b.Property(ul => ul.ProviderDisplayName).HasColumnName("provider_display_name");
            });
        }
    }
}
