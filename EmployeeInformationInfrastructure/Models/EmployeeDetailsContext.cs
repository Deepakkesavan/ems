using System;
using System.Collections.Generic;
using EmpInfoInfra.ConncectionStrings;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoInfra.Models;

public partial class EmployeeDetailsContext : DbContext
{
    private readonly DbConnectionStrings _dbConnectionStrings;
    
    public EmployeeDetailsContext(DbContextOptions<EmployeeDetailsContext> options,DbConnectionStrings dbConnectionStrings)
        : base(options)
    {
        _dbConnectionStrings = dbConnectionStrings;
    }

    public virtual DbSet<Billing> Billings { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<DesignationPermission> DesignationPermissions { get; set; }

    public virtual DbSet<EmpType> EmpTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<IdentityInfo> IdentityInfos { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<PersonalDetail> PersonalDetails { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<PublicUser> PublicUsers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SsoUser> SsoUsers { get; set; }

    public virtual DbSet<WorkInfo> WorkInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(_dbConnectionStrings.MainDb);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.Bid).HasName("PK__Billing__C6DE0D217014A653");

            entity.ToTable("Billing");

            entity.Property(e => e.Bid).HasColumnName("BID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Cid).HasName("PK__Clients__C1F8DC592A6648F7");

            entity.Property(e => e.Cid).HasColumnName("CID");
            entity.Property(e => e.ClientName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Client");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProjId).HasColumnName("ProjID");

            entity.HasOne(d => d.Proj).WithMany(p => p.Clients)
                .HasForeignKey(d => d.ProjId)
                .HasConstraintName("FK_Client_proj");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PK__Department");

            entity.ToTable("Department");

            entity.Property(e => e.DeptId).HasColumnName("DeptID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Department1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Department");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NewDeptId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("NewDeptID");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.DesgId).HasName("PK__Designat__F5B4FB8203CD2CEE");

            entity.ToTable("Designation");

            entity.HasIndex(e => e.Id, "UQ_Designation_Id").IsUnique();

            entity.Property(e => e.DesgId).HasColumnName("DesgID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Desg)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id)
                .IsRequired()
                .HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<DesignationPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_applications");

            entity.ToTable("DesignationPermission");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CreatedAt");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CreatedBy");
            entity.Property(e => e.DesignationGuid).HasColumnName("DesignationGuid");
            entity.Property(e => e.PermissionGuid).HasColumnName("PermissionGuid");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UpdatedAt");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UpdatedBy");

            entity.HasOne(d => d.Designation).WithMany(p => p.DesignationPermissions)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.DesignationGuid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dp_designation");
        });

        modelBuilder.Entity<EmpType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__EmpType__516F039530A76B42");

            entity.ToTable("EmpType");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__Employee__AF2DBA7961504EE4");

            entity.ToTable("Employee");

            entity.Property(e => e.EmpId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("EmpID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<IdentityInfo>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK_IdentityInfo_EmpId");

            entity.ToTable("IdentityInfo");

            entity.Property(e => e.EmpId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Ifsc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("IFSC");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Pan)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PAN");
            entity.Property(e => e.Panfile).HasColumnName("PANFile");
            entity.Property(e => e.Uan)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("UAN");

            entity.HasOne(d => d.Emp).WithOne(p => p.IdentityInfo)
                .HasForeignKey<IdentityInfo>(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdentityInfo_Emp");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocId).HasName("PK__Location__6A46DEE96C37A3B6");

            entity.ToTable("Location");

            entity.Property(e => e.LocId).HasColumnName("LocID");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Location1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Location");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Otp__3213E83F2DC77E8E");

            entity.ToTable("Otp");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Expiry).HasColumnName("expiry");
            entity.Property(e => e.OtpCode)
                .HasMaxLength(50)
                .HasColumnName("otpCode");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<PersonalDetail>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK_PersonalDetails_EmpId");

            entity.Property(e => e.EmpId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Age)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.EmergencyContact1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContact2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactName1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmergencyContactName2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PermanentAddress)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.PersonalEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PersonalPhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PresentAddress)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.Emp).WithOne(p => p.PersonalDetail)
                .HasForeignKey<PersonalDetail>(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalDetails_Emp");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjId).HasName("PK__Project__16212AFC0762C759");

            entity.ToTable("Project");

            entity.Property(e => e.ProjId).HasColumnName("ProjID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProjectName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Project");
        });

        modelBuilder.Entity<PublicUser>(entity =>
        {
            entity.HasKey(e => e.PublicId).HasName("PK__PublicUs__26B39584EC4E0B91");

            entity.ToTable("PublicUser");

            entity.Property(e => e.PublicId).HasColumnName("publicId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.IsFirstTimeLogin).HasColumnName("isFirstTimeLogin");
            entity.Property(e => e.PublicEmail)
                .HasMaxLength(255)
                .HasColumnName("publicEmail");
            entity.Property(e => e.PublicPassword)
                .HasMaxLength(255)
                .HasColumnName("publicPassword");
            entity.Property(e => e.PublicUsername)
                .HasMaxLength(255)
                .HasColumnName("publicUsername");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });


        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Resource__4ED1814FEDEE0256");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SsoUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SsoUsers__3213E83F9F7D9583");

            entity.HasIndex(e => e.Email, "UQ__SsoUsers__AB6E6164269E604D").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__SsoUsers__F3DBC57222A97289").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.ExitDate).HasColumnName("exit_date");
            entity.Property(e => e.FirstTimeLogin)
                .HasDefaultValue(true)
                .HasColumnName("first_time_login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });


        modelBuilder.Entity<WorkInfo>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK_WorkInfo_EmpId");

            entity.ToTable("WorkInfo");

            entity.Property(e => e.EmpId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("EmpID");
            entity.Property(e => e.Bid).HasColumnName("BID");
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CurrExp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DeptId).HasColumnName("DeptID");
            entity.Property(e => e.DesgnId).HasColumnName("DesgnID");
            entity.Property(e => e.Doc)
                .HasColumnType("datetime")
                .HasColumnName("DOC");
            entity.Property(e => e.Doj)
                .HasColumnType("datetime")
                .HasColumnName("DOJ");
            entity.Property(e => e.EmailTriggerStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LocId).HasColumnName("LocID");
            entity.Property(e => e.ManagerEmpCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ReportingManager)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.SourceOfHire)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalExp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransferFromDate).HasColumnType("datetime");
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.BidNavigation).WithMany(p => p.WorkInfos)
                .HasForeignKey(d => d.Bid)
                .HasConstraintName("FK__WorkInfo__BID__0B91BA14");

            entity.HasOne(d => d.Dept).WithMany(p => p.WorkInfos)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK__WorkInfo__DeptID__08B54D69");

            entity.HasOne(d => d.Desgn).WithMany(p => p.WorkInfos)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.DesgnId)
                .HasConstraintName("FK_WorkInfo_Designation");

            entity.HasOne(d => d.Emp).WithOne(p => p.WorkInfo)
                .HasForeignKey<WorkInfo>(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkInfo_Emp");

            entity.HasOne(d => d.Loc).WithMany(p => p.WorkInfos)
                .HasForeignKey(d => d.LocId)
                .HasConstraintName("FK__WorkInfo__LocID__07C12930");

            entity.HasOne(d => d.Proj).WithMany(p => p.WorkInfos)
                .HasForeignKey(d => d.ProjId)
                .HasConstraintName("FK_WorkInfo_Proj");

            entity.HasOne(d => d.Role).WithMany(p => p.WorkInfos)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__WorkInfo__Resour__0A9D95DB");

            entity.HasOne(d => d.Type).WithMany(p => p.WorkInfos)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__WorkInfo__TypeID__0C85DE4D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
