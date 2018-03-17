using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace authentication_service.Models
{
    public partial class user_auth_dbContext : DbContext
    {
        public user_auth_dbContext(DbContextOptions<user_auth_dbContext> options) : base(options) { }

        public virtual DbSet<AcctState> AcctState { get; set; }
        public virtual DbSet<Domain> Domain { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupDomain> GroupDomain { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcctState>(entity =>
            {
                entity.ToTable("acct_state");

                entity.ForNpgsqlHasComment("Account State tracking table to track the lifetime of an account and determine whether the account is active/disabled");

                entity.Property(e => e.AcctStateId)
                    .HasColumnName("acct_state_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AcctStateDesc).HasColumnName("acct_state_desc");

                entity.Property(e => e.AcctStateName)
                    .IsRequired()
                    .HasColumnName("acct_state_name");
            });

            modelBuilder.Entity<Domain>(entity =>
            {
                entity.ToTable("domain");

                entity.Property(e => e.DomainId)
                    .HasColumnName("domain_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DomainName)
                    .IsRequired()
                    .HasColumnName("domain_name");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("group");

                entity.ForNpgsqlHasComment("Group table to allow for access for shared content for all users associated to within the same group");

                entity.Property(e => e.GroupId)
                    .HasColumnName("group_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasColumnName("group_name");
            });

            modelBuilder.Entity<GroupDomain>(entity =>
            {
                entity.ToTable("group_domain");

                entity.HasIndex(e => e.DomainId)
                    .HasName("fki_domain_id_fk");

                entity.Property(e => e.GroupDomainId)
                    .HasColumnName("group_domain_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.DomainId).HasColumnName("domain_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.GroupDomain)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("domain_id_fk");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupDomain)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_id_fk");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permission");

                entity.Property(e => e.PermissionId)
                    .HasColumnName("permission_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.PermissionDesc).HasColumnName("permission_desc");

                entity.Property(e => e.PermissionName)
                    .IsRequired()
                    .HasColumnName("permission_name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.ForNpgsqlHasComment("Individual Roles table to adhere to RBAC.");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleTitle)
                    .IsRequired()
                    .HasColumnName("role_title");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.AcctStateId)
                    .HasName("fki_acct_state_id_fk");

                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.AcctStateId).HasColumnName("acct_state_id");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("date");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.LastLoginTimestamp)
                    .HasColumnName("last_login_timestamp")
                    .HasColumnType("date");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("varchar");

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasColumnType("varchar");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar");

                entity.HasOne(d => d.AcctState)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.AcctStateId)
                    .HasConstraintName("acct_state_id_fk");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("user_group");

                entity.ForNpgsqlHasComment("User to Group mapping table");

                entity.HasIndex(e => e.GroupId)
                    .HasName("fki_group_id_fk");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_user_to_group_fk");

                entity.Property(e => e.UserGroupId)
                    .HasColumnName("user_group_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("group_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_id_fk");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("user_permission");

                entity.ForNpgsqlHasComment("User to Permission mapping table");

                entity.HasIndex(e => e.PermissionId)
                    .HasName("fki_permission_id_fk");

                entity.Property(e => e.UserPermissionId)
                    .HasColumnName("user_permission_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.PermissionId).HasColumnName("permission_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("permission_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_id_fk");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role");

                entity.ForNpgsqlHasComment("User to Role mapping table");

                entity.HasIndex(e => e.RoleId)
                    .HasName("fki_role_id_fk");

                entity.HasIndex(e => e.UserId)
                    .HasName("fki_user_id_fk");

                entity.Property(e => e.UserRoleId)
                    .HasColumnName("user_role_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("role_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_id_fk");
            });

            modelBuilder.HasSequence("user_id_seq");
        }
    }
}
