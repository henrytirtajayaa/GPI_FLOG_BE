using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FLOG_BE.Model.Central.Entities;
using FLOG_BE.Model.Central.Mapping;

namespace FLOG_BE.Model.Central
{
    public class FlogContext : DbContext
    {
        public FlogContext(DbContextOptions<FlogContext> options)
            : base(options)
        {
        }

        #region Generated Properties
        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<CompanySecurity> CompanySecurities { get; set; }

        public virtual DbSet<Form> Forms { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<PersonCategory> PersonCategories { get; set; }

        public virtual DbSet<SecurityRoleAccess> SecurityRoleAccesses { get; set; }

        public virtual DbSet<SecurityRole> SecurityRoles { get; set; }
        public virtual DbSet<SessionState> SessionStates { get; set; }
        public virtual DbSet<SessionActivityLog> SessionActivityLogs { get; set; }
        public virtual DbSet<SmartView> Smartviews { get; set; }
        public virtual DbSet<SmartRole> SmartRoles { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Generated Configuration
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new CompanySecurityMap());
            modelBuilder.ApplyConfiguration(new FormMap());
            modelBuilder.ApplyConfiguration(new PersonCategoryMap());
            modelBuilder.ApplyConfiguration(new PersonMap());
            modelBuilder.ApplyConfiguration(new SecurityRoleAccessMap());
            modelBuilder.ApplyConfiguration(new SecurityRoleMap());
            modelBuilder.ApplyConfiguration(new SessionStateMap());
            modelBuilder.ApplyConfiguration(new SessionActivityLogMap());
            modelBuilder.ApplyConfiguration(new SmartviewMap());
            modelBuilder.ApplyConfiguration(new SmartRoleMap());
            
            #endregion

            modelBuilder.Entity<CompanySecurity>()
                .HasOne(p => p.SecurityRole)
                .WithMany(p => p.CompanySecurities)
                .HasForeignKey(k => k.SecurityRoleId)
                .HasPrincipalKey(k => k.SecurityRoleId);

            modelBuilder.Entity<CompanySecurity>()
                .HasOne(p => p.Company)
                .WithMany(p => p.CompanySecurities)
                .HasForeignKey(k => k.CompanyId)
                .HasPrincipalKey(k => k.CompanyId);

            modelBuilder.Entity<SecurityRoleAccess>()
                .HasOne(p => p.Form)
                .WithMany(p => p.RoleAccesses)
                .HasForeignKey(k => k.FormId)
                .HasPrincipalKey(k => k.FormId);

            modelBuilder.Entity<SecurityRole>()
                .HasMany(p => p.RoleAccess)
                .WithOne(p => p.SecurityRoles)
                .HasForeignKey(k => k.SecurityRoleId)
                .HasPrincipalKey(k => k.SecurityRoleId);

        }
    }
}
