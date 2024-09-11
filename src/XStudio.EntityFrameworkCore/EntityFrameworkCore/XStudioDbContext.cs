using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShardingCore.Sharding.Abstractions;
using ShardingCore.Sharding;
using System.Linq;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using XStudio.Common;
using XStudio.Projects;
using XStudio.Schools.Places;
using System.Drawing;
using System.Reflection.Emit;
using XStudio.Schools.Timetable;

namespace XStudio.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class XStudioDbContext :
    AbpDbContext<XStudioDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    #region Self DB
    public DbSet<Project> Projects { get; set; }
    #endregion


    #region 学校场所
    public DbSet<School> Schools { get; set; }
    public DbSet<SchoolCampus> SchoolCampuses { get; set; }
    public DbSet<SchoolBuilding> SchoolBuildings { get; set; }
    public DbSet<BuildingFloor> BuildingFloors { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    #endregion

    #region 课表
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<TimePeriod> TimePeriods { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Course> Courses { get; set; }
    #endregion
    public XStudioDbContext(DbContextOptions<XStudioDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(XStudioConsts.DbTablePrefix + "YourEntities", XStudioConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});

        builder.Entity<Project>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "Projects", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            //自动添加注释
            AddCommentsToProperties(b);
        });

        #region 场所
        builder.Entity<School>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "Schools", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");

            //自动添加注释
            AddCommentsToProperties(b);
        });

        builder.Entity<SchoolCampus>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "SchoolCampuses", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            b.HasOne(c => c.School)
             .WithMany(s => s.Campuses)
             .HasForeignKey(c => c.SchoolCode);

            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");
            b.HasIndex(x => x.SchoolCode).HasDatabaseName("IX_TimePeriod_SchoolCode");

            //自动添加注释
            AddCommentsToProperties(b);
        });


        builder.Entity<SchoolBuilding>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "SchoolBuildings", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            b.HasOne(b => b.Campus)
             .WithMany(c => c.Buildings)
             .HasForeignKey(b => b.SchoolCampusCode);
            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");
            b.HasIndex(x => x.SchoolCode).HasDatabaseName("IX_TimePeriod_SchoolCode");
            b.HasIndex(x => x.SchoolCampusCode).HasDatabaseName("IX_TimePeriod_SchoolCampusCode");

            //自动添加注释
            AddCommentsToProperties(b);
        });


        builder.Entity<BuildingFloor>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "BuildingFloors", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            b.HasOne(f => f.Building)
             .WithMany(b => b.Floors)
             .HasForeignKey(f => f.BuildingCode);
            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");
            b.HasIndex(x => x.SchoolCode).HasDatabaseName("IX_TimePeriod_SchoolCode");
            b.HasIndex(x => x.SchoolCampusCode).HasDatabaseName("IX_TimePeriod_SchoolCampusCode");
            b.HasIndex(x => x.BuildingCode).HasDatabaseName("IX_TimePeriod_BuildingCode");

            //自动添加注释
            AddCommentsToProperties(b);
        });


        builder.Entity<Classroom>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "Classrooms", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            b.HasOne(c => c.Floor)
             .WithMany(f => f.Classrooms)
             .HasForeignKey(c => c.FloorCode);
            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");
            b.HasIndex(x => x.SchoolCode).HasDatabaseName("IX_TimePeriod_SchoolCode");
            b.HasIndex(x => x.SchoolCampusCode).HasDatabaseName("IX_TimePeriod_SchoolCampusCode");
            b.HasIndex(x => x.BuildingCode).HasDatabaseName("IX_TimePeriod_BuildingCode");
            b.HasIndex(x => x.FloorCode).HasDatabaseName("IX_TimePeriod_FloorCode");

            //自动添加注释
            AddCommentsToProperties(b);
        });

        #endregion

        #region 课表
        builder.Entity<TimePeriod>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "TimePeriods", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");

            //自动添加注释
            AddCommentsToProperties(b);
        });

        builder.Entity<Schedule>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "Schedules", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");

            //自动添加注释
            AddCommentsToProperties(b);
        });

        builder.Entity<Section>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "Sections", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            b.HasOne(b => b.Schedule)
             .WithMany(c => c.Sections)
             .HasForeignKey(b => b.ScheduleCode);
            // 设置 Code、 Name、 ScheduleCode为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");
            b.HasIndex(x => x.ScheduleCode).HasDatabaseName("IX_TimePeriod_ScheduleCode");
            //自动添加注释
            AddCommentsToProperties(b);
        });

        builder.Entity<Course>(b =>
        {
            b.ToTable(XStudioConsts.DbTablePrefix + "Courses", XStudioConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Code).IsRequired().HasMaxLength(128);
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasKey(x => x.Code);
            // 设置 Code 和 Name 为索引列
            b.HasIndex(x => x.Code).HasDatabaseName("IX_TimePeriod_Code");
            b.HasIndex(x => x.Name).HasDatabaseName("IX_TimePeriod_Name");

            //自动添加注释
            AddCommentsToProperties(b);
        });
        
        #endregion
    }

    // 扩展方法：为属性添加注释
    private void AddCommentsToProperties<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
    {
        var properties = typeof(TEntity).GetProperties();
        foreach (var property in properties)
        {
            // 获取属性上的CommentAttribute
            var commentAttribute = property.GetCustomAttributes(typeof(DbDescriptionAttribute), false)
                                            .FirstOrDefault() as DbDescriptionAttribute;
            if (commentAttribute != null)
            {
                // 为每个属性添加注释
                builder.Property(property.Name).HasComment(commentAttribute.DbDescription);
            }
        }
    }
}
