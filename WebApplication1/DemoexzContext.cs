using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1;

public partial class DemoexzContext : DbContext
{
    public DemoexzContext()
    {
    }

    public DemoexzContext(DbContextOptions<DemoexzContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Equipment> Equipment { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TypeMalfunction> TypeMalfunctions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("host=localhost;user=root;password=1234;database=demoexz", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.IdApplication).HasName("PRIMARY");

            entity.ToTable("application");

            entity.HasIndex(e => e.IdExecutor, "fk_idExecutor_idx");

            entity.HasIndex(e => e.IdUser, "fk_id_user_idx");

            entity.HasIndex(e => e.IdType, "fk_typeMalfunction_idx");

            entity.HasIndex(e => e.IdEquipment, "id_equpment_idx");

            entity.Property(e => e.IdApplication)
                .ValueGeneratedNever()
                .HasColumnName("idApplication");
            entity.Property(e => e.Cost)
                .HasMaxLength(450)
                .HasColumnName("cost");
            entity.Property(e => e.DateAdd).HasColumnName("dateAdd");
            entity.Property(e => e.DateEnd).HasColumnName("dateEnd");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.IdEquipment).HasColumnName("idEquipment");
            entity.Property(e => e.IdExecutor).HasColumnName("idExecutor");
            entity.Property(e => e.IdType).HasColumnName("idType");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Material)
                .HasMaxLength(450)
                .HasColumnName("material");
            entity.Property(e => e.Phase)
                .HasMaxLength(45)
                .HasColumnName("phase");
            entity.Property(e => e.Status)
                .HasMaxLength(45)
                .HasColumnName("status");
            entity.Property(e => e.Time)
                .HasMaxLength(100)
                .HasColumnName("time");

            entity.HasOne(d => d.IdEquipmentNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.IdEquipment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idEqupment");

            entity.HasOne(d => d.IdExecutorNavigation).WithMany(p => p.ApplicationIdExecutorNavigations)
                .HasForeignKey(d => d.IdExecutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idExecutor");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Applications)
                .HasForeignKey(d => d.IdType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_typeMalfunction");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ApplicationIdUserNavigations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idUser");
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Idequipment).HasName("PRIMARY");

            entity.ToTable("equipment");

            entity.Property(e => e.Idequipment)
                .ValueGeneratedNever()
                .HasColumnName("idequipment");
            entity.Property(e => e.Equipment1)
                .HasMaxLength(200)
                .HasColumnName("equipment");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Idrole)
                .ValueGeneratedNever()
                .HasColumnName("idrole");
            entity.Property(e => e.Role1)
                .HasMaxLength(45)
                .HasColumnName("role");
        });

        modelBuilder.Entity<TypeMalfunction>(entity =>
        {
            entity.HasKey(e => e.IdtypeMalfunction).HasName("PRIMARY");

            entity.ToTable("type_malfunction");

            entity.Property(e => e.IdtypeMalfunction)
                .ValueGeneratedNever()
                .HasColumnName("idtype_malfunction");
            entity.Property(e => e.Type)
                .HasMaxLength(200)
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.IdRole, "fk_idRole_idx");

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("idUser");
            entity.Property(e => e.Fullname)
                .HasMaxLength(200)
                .HasColumnName("fullname");
            entity.Property(e => e.IdRole).HasColumnName("idRole");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(450)
                .HasColumnName("password");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idRole");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
