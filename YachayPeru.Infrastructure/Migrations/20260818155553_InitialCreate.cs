using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YachayPeru.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "aprendiz");

            migrationBuilder.EnsureSchema(
                name: "course");

            migrationBuilder.EnsureSchema(
                name: "content");

            migrationBuilder.EnsureSchema(
                name: "common");

            migrationBuilder.EnsureSchema(
                name: "access");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.EnsureSchema(
                name: "assessment");

            migrationBuilder.CreateTable(
                name: "courses",
                schema: "course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SourceTemplateId = table.Column<int>(type: "int", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ZoneCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_courses_courses_SourceTemplateId",
                        column: x => x.SourceTemplateId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "insignias",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MinPoints = table.Column<int>(type: "int", nullable: true),
                    MinRetosCompleted = table.Column<int>(type: "int", nullable: true),
                    MinPerfectRetos = table.Column<int>(type: "int", nullable: true),
                    RequireAllQuestionTypes = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MinLevel = table.Column<int>(type: "int", nullable: true),
                    RequirePremium = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MinLoginStreakDays = table.Column<int>(type: "int", nullable: true),
                    MinAnswerStreak = table.Column<int>(type: "int", nullable: true),
                    MinRegionsExplored = table.Column<int>(type: "int", nullable: true),
                    RequiredZoneCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MinZoneRegionsExplored = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insignias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "master_codes",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_master_codes", x => x.Id);
                    table.UniqueConstraint("AK_master_codes_Code", x => x.Code);
                    table.ForeignKey(
                        name: "FK_master_codes_master_codes_ParentCode",
                        column: x => x.ParentCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "noticias",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_noticias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                schema: "common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Ruc = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LegalRepresentative = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "platform_roles",
                schema: "access",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "predisenos",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TreeJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_predisenos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "premium_benefits",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_premium_benefits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "premium_plans",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_premium_plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "resources",
                schema: "access",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "RESOURCE_SCOPES_PLATFORM")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "community_posts",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_community_posts_courses_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "festividades",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_festividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_festividades_courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "regiones_destacadas",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_regiones_destacadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_regiones_destacadas_courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "retos",
                schema: "assessment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_retos_courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "insignia_required_regions",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsigniaId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insignia_required_regions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_insignia_required_regions_courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_insignia_required_regions_insignias_InsigniaId",
                        column: x => x.InsigniaId,
                        principalSchema: "content",
                        principalTable: "insignias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_versions",
                schema: "course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DurationHours = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    is_current = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_course_versions_courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_versions_master_codes_StatusCode",
                        column: x => x.StatusCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "media_items",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MediaTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ExternalUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LegendText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_media_items_courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_media_items_master_codes_MediaTypeCode",
                        column: x => x.MediaTypeCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    UserTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LockedReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_master_codes_UserTypeCode",
                        column: x => x.UserTypeCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "common",
                        principalTable: "persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_users_platform_roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "access",
                        principalTable: "platform_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "premium_plan_features",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    BenefitId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_premium_plan_features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_premium_plan_features_premium_benefits_BenefitId",
                        column: x => x.BenefitId,
                        principalSchema: "content",
                        principalTable: "premium_benefits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_premium_plan_features_premium_plans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "content",
                        principalTable: "premium_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                schema: "access",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    PermissionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_permissions_master_codes_PermissionCode",
                        column: x => x.PermissionCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_permissions_resources_ResourceId",
                        column: x => x.ResourceId,
                        principalSchema: "access",
                        principalTable: "resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "certificate_templates",
                schema: "course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetoId = table.Column<int>(type: "int", nullable: false),
                    MainTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BodyText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FooterText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludeCompletionDate = table.Column<bool>(type: "bit", nullable: false),
                    IncludeScore = table.Column<bool>(type: "bit", nullable: false),
                    IncludeDuration = table.Column<bool>(type: "bit", nullable: false),
                    IncludeQrCode = table.Column<bool>(type: "bit", nullable: false),
                    IncludeEmployeeId = table.Column<bool>(type: "bit", nullable: false),
                    IncludeCertificateId = table.Column<bool>(type: "bit", nullable: false),
                    IncludeInstructor = table.Column<bool>(type: "bit", nullable: false),
                    IncludeLocation = table.Column<bool>(type: "bit", nullable: false),
                    IncludeModality = table.Column<bool>(type: "bit", nullable: false),
                    IncludeValidity = table.Column<bool>(type: "bit", nullable: false),
                    Orientation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrimaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SecondaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccentColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FontFamily = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BorderStyle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BorderWidth = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShowLogo = table.Column<bool>(type: "bit", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SignerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SignerTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SignatureUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ShowSeal = table.Column<bool>(type: "bit", nullable: false),
                    SealUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ShowWatermark = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificate_templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_certificate_templates_retos_RetoId",
                        column: x => x.RetoId,
                        principalSchema: "assessment",
                        principalTable: "retos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "insignia_required_retos",
                schema: "content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsigniaId = table.Column<int>(type: "int", nullable: false),
                    RetoId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insignia_required_retos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_insignia_required_retos_insignias_InsigniaId",
                        column: x => x.InsigniaId,
                        principalSchema: "content",
                        principalTable: "insignias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_insignia_required_retos_retos_RetoId",
                        column: x => x.RetoId,
                        principalSchema: "assessment",
                        principalTable: "retos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reto_versions",
                schema: "assessment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetoId = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PassingScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TimeLimitMinutes = table.Column<int>(type: "int", nullable: true),
                    MaxAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    ShuffleQuestionOrder = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShuffleOptionOrder = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShowResultsAtEnd = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reto_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reto_versions_master_codes_StatusCode",
                        column: x => x.StatusCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reto_versions_retos_RetoId",
                        column: x => x.RetoId,
                        principalSchema: "assessment",
                        principalTable: "retos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_modules",
                schema: "course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseVersionId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    DurationHours = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_course_modules_course_versions_CourseVersionId",
                        column: x => x.CourseVersionId,
                        principalSchema: "course",
                        principalTable: "course_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aprendiz_activity_logs",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aprendiz_activity_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aprendiz_activity_logs_courses_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aprendiz_activity_logs_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aprendiz_insignias_earned",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InsigniaId = table.Column<int>(type: "int", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aprendiz_insignias_earned", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aprendiz_insignias_earned_insignias_InsigniaId",
                        column: x => x.InsigniaId,
                        principalSchema: "content",
                        principalTable: "insignias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_aprendiz_insignias_earned_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aprendiz_profiles",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Level = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    FavoriteRegionId = table.Column<int>(type: "int", nullable: true),
                    IsPremiumUser = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModulesDone = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LearningTimeMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastActiveDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CurrentLoginStreakDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BestLoginStreakDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CurrentAnswerStreak = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BestAnswerStreak = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aprendiz_profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aprendiz_profiles_courses_FavoriteRegionId",
                        column: x => x.FavoriteRegionId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aprendiz_profiles_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aprendiz_region_explored",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    FirstViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aprendiz_region_explored", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aprendiz_region_explored_courses_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aprendiz_region_explored_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "community_post_likes",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_community_post_likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_community_post_likes_community_posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "aprendiz",
                        principalTable: "community_posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_community_post_likes_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "festividad_reminders",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FestividadId = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_festividad_reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_festividad_reminders_festividades_FestividadId",
                        column: x => x.FestividadId,
                        principalSchema: "content",
                        principalTable: "festividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_festividad_reminders_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "premium_waitlist_entries",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ReceiptUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReviewSeen = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_premium_waitlist_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_premium_waitlist_entries_premium_plans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "content",
                        principalTable: "premium_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_premium_waitlist_entries_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LoginIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoginUserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalToken = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovalExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuspiciousIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_refresh_tokens_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_password_changes",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_password_changes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_password_changes_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "platform_role_permissions",
                schema: "access",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformRoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_role_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_platform_role_permissions_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "access",
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_platform_role_permissions_platform_roles_PlatformRoleId",
                        column: x => x.PlatformRoleId,
                        principalSchema: "access",
                        principalTable: "platform_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reto_attempts",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RetoId = table.Column<int>(type: "int", nullable: false),
                    RetoVersionId = table.Column<int>(type: "int", nullable: false),
                    EarnedPoints = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    TotalPoints = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    CorrectCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reto_attempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reto_attempts_reto_versions_RetoVersionId",
                        column: x => x.RetoVersionId,
                        principalSchema: "assessment",
                        principalTable: "reto_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reto_attempts_retos_RetoId",
                        column: x => x.RetoId,
                        principalSchema: "assessment",
                        principalTable: "retos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reto_attempts_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reto_version_questions",
                schema: "assessment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetoVersionId = table.Column<int>(type: "int", nullable: false),
                    QuestionTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reto_version_questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reto_version_questions_master_codes_QuestionTypeCode",
                        column: x => x.QuestionTypeCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reto_version_questions_reto_versions_RetoVersionId",
                        column: x => x.RetoVersionId,
                        principalSchema: "assessment",
                        principalTable: "reto_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aprendiz_region_activities",
                schema: "aprendiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aprendiz_region_activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_aprendiz_region_activities_course_modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "course",
                        principalTable: "course_modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aprendiz_region_activities_courses_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "course",
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_aprendiz_region_activities_users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "module_contents",
                schema: "course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module_contents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_module_contents_course_modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "course",
                        principalTable: "course_modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reto_version_question_blanks",
                schema: "assessment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetoVersionQuestionId = table.Column<int>(type: "int", nullable: false),
                    BlankIndex = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reto_version_question_blanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reto_version_question_blanks_reto_version_questions_RetoVersionQuestionId",
                        column: x => x.RetoVersionQuestionId,
                        principalSchema: "assessment",
                        principalTable: "reto_version_questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reto_version_question_choices",
                schema: "assessment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetoVersionQuestionId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reto_version_question_choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reto_version_question_choices_reto_version_questions_RetoVersionQuestionId",
                        column: x => x.RetoVersionQuestionId,
                        principalSchema: "assessment",
                        principalTable: "reto_version_questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "module_content_files",
                schema: "course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleContentId = table.Column<int>(type: "int", nullable: false),
                    FileTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module_content_files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_module_content_files_master_codes_FileTypeCode",
                        column: x => x.FileTypeCode,
                        principalSchema: "common",
                        principalTable: "master_codes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_module_content_files_module_contents_ModuleContentId",
                        column: x => x.ModuleContentId,
                        principalSchema: "course",
                        principalTable: "module_contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_activity_logs_RegionId",
                schema: "aprendiz",
                table: "aprendiz_activity_logs",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_activity_logs_UserId",
                schema: "aprendiz",
                table: "aprendiz_activity_logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_insignias_earned_InsigniaId",
                schema: "aprendiz",
                table: "aprendiz_insignias_earned",
                column: "InsigniaId");

            migrationBuilder.CreateIndex(
                name: "ux_aprendiz_insignias_earned_user_insignia",
                schema: "aprendiz",
                table: "aprendiz_insignias_earned",
                columns: new[] { "UserId", "InsigniaId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_profiles_FavoriteRegionId",
                schema: "aprendiz",
                table: "aprendiz_profiles",
                column: "FavoriteRegionId");

            migrationBuilder.CreateIndex(
                name: "ux_aprendiz_profiles_user",
                schema: "aprendiz",
                table: "aprendiz_profiles",
                column: "UserId",
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_region_activities_ModuleId",
                schema: "aprendiz",
                table: "aprendiz_region_activities",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_region_activities_RegionId",
                schema: "aprendiz",
                table: "aprendiz_region_activities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_region_activities_UserId",
                schema: "aprendiz",
                table: "aprendiz_region_activities",
                column: "UserId",
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_aprendiz_region_explored_RegionId",
                schema: "aprendiz",
                table: "aprendiz_region_explored",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "ux_aprendiz_region_explored_user_region",
                schema: "aprendiz",
                table: "aprendiz_region_explored",
                columns: new[] { "UserId", "RegionId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "ux_certificate_templates_reto",
                schema: "course",
                table: "certificate_templates",
                column: "RetoId",
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_community_post_likes_UserId",
                schema: "aprendiz",
                table: "community_post_likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "ux_community_post_likes_post_user",
                schema: "aprendiz",
                table: "community_post_likes",
                columns: new[] { "PostId", "UserId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_community_posts_RegionId",
                schema: "aprendiz",
                table: "community_posts",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_course_modules_CourseVersionId",
                schema: "course",
                table: "course_modules",
                column: "CourseVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_course_versions_StatusCode",
                schema: "course",
                table: "course_versions",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "ux_course_versions_is_current",
                schema: "course",
                table: "course_versions",
                column: "CourseId",
                unique: true,
                filter: "[is_current] = 1 AND [Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_courses_SourceTemplateId",
                schema: "course",
                table: "courses",
                column: "SourceTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_festividad_reminders_FestividadId",
                schema: "aprendiz",
                table: "festividad_reminders",
                column: "FestividadId");

            migrationBuilder.CreateIndex(
                name: "ux_festividad_reminders_user_festividad",
                schema: "aprendiz",
                table: "festividad_reminders",
                columns: new[] { "UserId", "FestividadId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_festividades_CourseId",
                schema: "content",
                table: "festividades",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_insignia_required_regions_CourseId",
                schema: "content",
                table: "insignia_required_regions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_insignia_required_regions_InsigniaId",
                schema: "content",
                table: "insignia_required_regions",
                column: "InsigniaId");

            migrationBuilder.CreateIndex(
                name: "IX_insignia_required_retos_InsigniaId",
                schema: "content",
                table: "insignia_required_retos",
                column: "InsigniaId");

            migrationBuilder.CreateIndex(
                name: "IX_insignia_required_retos_RetoId",
                schema: "content",
                table: "insignia_required_retos",
                column: "RetoId");

            migrationBuilder.CreateIndex(
                name: "IX_master_codes_Code",
                schema: "common",
                table: "master_codes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_master_codes_ParentCode",
                schema: "common",
                table: "master_codes",
                column: "ParentCode");

            migrationBuilder.CreateIndex(
                name: "IX_media_items_CourseId",
                schema: "content",
                table: "media_items",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_media_items_MediaTypeCode",
                schema: "content",
                table: "media_items",
                column: "MediaTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_module_content_files_FileTypeCode",
                schema: "course",
                table: "module_content_files",
                column: "FileTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_module_content_files_ModuleContentId",
                schema: "course",
                table: "module_content_files",
                column: "ModuleContentId");

            migrationBuilder.CreateIndex(
                name: "IX_module_contents_ModuleId",
                schema: "course",
                table: "module_contents",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_PermissionCode",
                schema: "access",
                table: "permissions",
                column: "PermissionCode");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_ResourceId_PermissionCode",
                schema: "access",
                table: "permissions",
                columns: new[] { "ResourceId", "PermissionCode" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_persons_DocumentNumber",
                schema: "common",
                table: "persons",
                column: "DocumentNumber",
                unique: true,
                filter: "[DocumentNumber] IS NOT NULL AND [Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_persons_Ruc",
                schema: "common",
                table: "persons",
                column: "Ruc",
                unique: true,
                filter: "[Ruc] IS NOT NULL AND [Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_platform_role_permissions_PermissionId",
                schema: "access",
                table: "platform_role_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_platform_role_permissions_PlatformRoleId_PermissionId",
                schema: "access",
                table: "platform_role_permissions",
                columns: new[] { "PlatformRoleId", "PermissionId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_platform_roles_RoleCode",
                schema: "access",
                table: "platform_roles",
                column: "RoleCode",
                unique: true,
                filter: "[RoleCode] IS NOT NULL AND [Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_premium_plan_features_BenefitId",
                schema: "content",
                table: "premium_plan_features",
                column: "BenefitId");

            migrationBuilder.CreateIndex(
                name: "ux_premium_plan_features_plan_benefit",
                schema: "content",
                table: "premium_plan_features",
                columns: new[] { "PlanId", "BenefitId" },
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_premium_waitlist_entries_PlanId",
                schema: "aprendiz",
                table: "premium_waitlist_entries",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "ux_premium_waitlist_entries_user",
                schema: "aprendiz",
                table: "premium_waitlist_entries",
                column: "UserId",
                unique: true,
                filter: "[Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_ApprovalToken",
                schema: "auth",
                table: "refresh_tokens",
                column: "ApprovalToken");

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_Token",
                schema: "auth",
                table: "refresh_tokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_UserId",
                schema: "auth",
                table: "refresh_tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_regiones_destacadas_CourseId",
                schema: "content",
                table: "regiones_destacadas",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_resources_Code",
                schema: "access",
                table: "resources",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reto_attempts_RetoId",
                schema: "aprendiz",
                table: "reto_attempts",
                column: "RetoId");

            migrationBuilder.CreateIndex(
                name: "IX_reto_attempts_RetoVersionId",
                schema: "aprendiz",
                table: "reto_attempts",
                column: "RetoVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_reto_attempts_UserId",
                schema: "aprendiz",
                table: "reto_attempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_reto_version_question_blanks_RetoVersionQuestionId",
                schema: "assessment",
                table: "reto_version_question_blanks",
                column: "RetoVersionQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_reto_version_question_choices_RetoVersionQuestionId",
                schema: "assessment",
                table: "reto_version_question_choices",
                column: "RetoVersionQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_reto_version_questions_QuestionTypeCode",
                schema: "assessment",
                table: "reto_version_questions",
                column: "QuestionTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_reto_version_questions_RetoVersionId",
                schema: "assessment",
                table: "reto_version_questions",
                column: "RetoVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_reto_versions_RetoId",
                schema: "assessment",
                table: "reto_versions",
                column: "RetoId");

            migrationBuilder.CreateIndex(
                name: "IX_reto_versions_StatusCode",
                schema: "assessment",
                table: "reto_versions",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_retos_CourseId",
                schema: "assessment",
                table: "retos",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_user_password_changes_UserId",
                schema: "auth",
                table: "user_password_changes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_PersonId",
                schema: "auth",
                table: "users",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                schema: "auth",
                table: "users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_users_UserTypeCode",
                schema: "auth",
                table: "users",
                column: "UserTypeCode");

            migrationBuilder.CreateIndex(
                name: "ux_users_email",
                schema: "auth",
                table: "users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL AND [Deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "ux_users_username",
                schema: "auth",
                table: "users",
                column: "Username",
                unique: true,
                filter: "[Deleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aprendiz_activity_logs",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "aprendiz_insignias_earned",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "aprendiz_profiles",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "aprendiz_region_activities",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "aprendiz_region_explored",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "certificate_templates",
                schema: "course");

            migrationBuilder.DropTable(
                name: "community_post_likes",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "festividad_reminders",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "insignia_required_regions",
                schema: "content");

            migrationBuilder.DropTable(
                name: "insignia_required_retos",
                schema: "content");

            migrationBuilder.DropTable(
                name: "media_items",
                schema: "content");

            migrationBuilder.DropTable(
                name: "module_content_files",
                schema: "course");

            migrationBuilder.DropTable(
                name: "noticias",
                schema: "content");

            migrationBuilder.DropTable(
                name: "platform_role_permissions",
                schema: "access");

            migrationBuilder.DropTable(
                name: "predisenos",
                schema: "content");

            migrationBuilder.DropTable(
                name: "premium_plan_features",
                schema: "content");

            migrationBuilder.DropTable(
                name: "premium_waitlist_entries",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "refresh_tokens",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "regiones_destacadas",
                schema: "content");

            migrationBuilder.DropTable(
                name: "reto_attempts",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "reto_version_question_blanks",
                schema: "assessment");

            migrationBuilder.DropTable(
                name: "reto_version_question_choices",
                schema: "assessment");

            migrationBuilder.DropTable(
                name: "user_password_changes",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "community_posts",
                schema: "aprendiz");

            migrationBuilder.DropTable(
                name: "festividades",
                schema: "content");

            migrationBuilder.DropTable(
                name: "insignias",
                schema: "content");

            migrationBuilder.DropTable(
                name: "module_contents",
                schema: "course");

            migrationBuilder.DropTable(
                name: "permissions",
                schema: "access");

            migrationBuilder.DropTable(
                name: "premium_benefits",
                schema: "content");

            migrationBuilder.DropTable(
                name: "premium_plans",
                schema: "content");

            migrationBuilder.DropTable(
                name: "reto_version_questions",
                schema: "assessment");

            migrationBuilder.DropTable(
                name: "users",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "course_modules",
                schema: "course");

            migrationBuilder.DropTable(
                name: "resources",
                schema: "access");

            migrationBuilder.DropTable(
                name: "reto_versions",
                schema: "assessment");

            migrationBuilder.DropTable(
                name: "persons",
                schema: "common");

            migrationBuilder.DropTable(
                name: "platform_roles",
                schema: "access");

            migrationBuilder.DropTable(
                name: "course_versions",
                schema: "course");

            migrationBuilder.DropTable(
                name: "retos",
                schema: "assessment");

            migrationBuilder.DropTable(
                name: "master_codes",
                schema: "common");

            migrationBuilder.DropTable(
                name: "courses",
                schema: "course");
        }
    }
}
