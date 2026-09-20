using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LV.SignFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSystemProfile = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSystemRole = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionProfilePermissions",
                columns: table => new
                {
                    PermissionProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionProfilePermissions", x => new { x.PermissionProfileId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_PermissionProfilePermissions_PermissionProfiles_PermissionProfileId",
                        column: x => x.PermissionProfileId,
                        principalTable: "PermissionProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionProfilePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExternalIdentityId = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DetailsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Signatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Signatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Signatures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stamps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stamps_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OwnerUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Templates_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissionProfiles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AssignedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissionProfiles", x => new { x.UserId, x.PermissionProfileId });
                    table.ForeignKey(
                        name: "FK_UserPermissionProfiles_PermissionProfiles_PermissionProfileId",
                        column: x => x.PermissionProfileId,
                        principalTable: "PermissionProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPermissionProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AssignedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateShares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedWithUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SharedWithDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccessLevel = table.Column<int>(type: "int", nullable: false),
                    SharedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SharedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateShares", x => x.Id);
                    table.CheckConstraint("CK_TemplateShares_ExactlyOneTarget", "(\r\n                    ([SharedWithUserId] IS NOT NULL AND [SharedWithDepartmentId] IS NULL)\r\n                    OR\r\n                    ([SharedWithUserId] IS NULL AND [SharedWithDepartmentId] IS NOT NULL)\r\n                  )");
                    table.ForeignKey(
                        name: "FK_TemplateShares_Departments_SharedWithDepartmentId",
                        column: x => x.SharedWithDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateShares_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateShares_Users_SharedByUserId",
                        column: x => x.SharedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateShares_Users_SharedWithUserId",
                        column: x => x.SharedWithUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Envelopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceTemplateVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    SentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    VoidedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Envelopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Envelopes_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Envelopes_TemplateVersions_SourceTemplateVersionId",
                        column: x => x.SourceTemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Envelopes_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BlobPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateDocuments_TemplateVersions_TemplateVersionId",
                        column: x => x.TemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateRecipientRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    RoutingOrder = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    DefaultUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AllowSenderEditRecipient = table.Column<bool>(type: "bit", nullable: false),
                    AllowSenderDeleteRecipient = table.Column<bool>(type: "bit", nullable: false),
                    AllowSenderChangeRoutingOrder = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateRecipientRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateRecipientRoles_TemplateVersions_TemplateVersionId",
                        column: x => x.TemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateRecipientRoles_Users_DefaultUserId",
                        column: x => x.DefaultUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateRoutingRuleSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateRoutingRuleSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateRoutingRuleSets_TemplateVersions_TemplateVersionId",
                        column: x => x.TemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvelopeDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OriginalBlobPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FinalBlobPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    OriginalHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FinalHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvelopeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvelopeDocuments_Envelopes_EnvelopeId",
                        column: x => x.EnvelopeId,
                        principalTable: "Envelopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvelopeRecipients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RoutingOrder = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ViewedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeclinedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeclineReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AllowSenderEditRecipient = table.Column<bool>(type: "bit", nullable: false),
                    AllowSenderDeleteRecipient = table.Column<bool>(type: "bit", nullable: false),
                    AllowSenderChangeRoutingOrder = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvelopeRecipients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvelopeRecipients_Envelopes_EnvelopeId",
                        column: x => x.EnvelopeId,
                        principalTable: "Envelopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnvelopeRecipients_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EnvelopeRoutingRuleSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvelopeRoutingRuleSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvelopeRoutingRuleSets_Envelopes_EnvelopeId",
                        column: x => x.EnvelopeId,
                        principalTable: "Envelopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvelopeStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: true),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    ChangedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ChangedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvelopeStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvelopeStatusHistories_Envelopes_EnvelopeId",
                        column: x => x.EnvelopeId,
                        principalTable: "Envelopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnvelopeStatusHistories_Users_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TemplateFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipientRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    PageNumber = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Y = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Width = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateFields_TemplateDocuments_TemplateDocumentId",
                        column: x => x.TemplateDocumentId,
                        principalTable: "TemplateDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateFields_TemplateRecipientRoles_RecipientRoleId",
                        column: x => x.RecipientRoleId,
                        principalTable: "TemplateRecipientRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateRoutingRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateRoutingRuleSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetRecipientRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MatchType = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateRoutingRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateRoutingRules_TemplateRecipientRoles_TargetRecipientRoleId",
                        column: x => x.TargetRecipientRoleId,
                        principalTable: "TemplateRecipientRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateRoutingRules_TemplateRoutingRuleSets_TemplateRoutingRuleSetId",
                        column: x => x.TemplateRoutingRuleSetId,
                        principalTable: "TemplateRoutingRuleSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvelopeFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeDocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeRecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldType = table.Column<int>(type: "int", nullable: false),
                    PageNumber = table.Column<int>(type: "int", nullable: false),
                    X = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Y = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Width = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvelopeFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvelopeFields_EnvelopeDocuments_EnvelopeDocumentId",
                        column: x => x.EnvelopeDocumentId,
                        principalTable: "EnvelopeDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnvelopeFields_EnvelopeRecipients_EnvelopeRecipientId",
                        column: x => x.EnvelopeRecipientId,
                        principalTable: "EnvelopeRecipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnvelopeFields_Envelopes_EnvelopeId",
                        column: x => x.EnvelopeId,
                        principalTable: "Envelopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvelopeEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OccurredAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvelopeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvelopeEvents_EnvelopeRecipients_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "EnvelopeRecipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvelopeEvents_Envelopes_EnvelopeId",
                        column: x => x.EnvelopeId,
                        principalTable: "Envelopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EvelopeEvents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EnvelopeRoutingRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeRoutingRuleSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetRecipientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MatchType = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvelopeRoutingRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvelopeRoutingRules_EnvelopeRecipients_TargetRecipientId",
                        column: x => x.TargetRecipientId,
                        principalTable: "EnvelopeRecipients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnvelopeRoutingRules_EnvelopeRoutingRuleSets_EnvelopeRoutingRuleSetId",
                        column: x => x.EnvelopeRoutingRuleSetId,
                        principalTable: "EnvelopeRoutingRuleSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateRoutingConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateRoutingRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceTemplateFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Operator = table.Column<int>(type: "int", nullable: false),
                    ComparisonValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateRoutingConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateRoutingConditions_TemplateFields_SourceTemplateFieldId",
                        column: x => x.SourceTemplateFieldId,
                        principalTable: "TemplateFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateRoutingConditions_TemplateRoutingRules_TemplateRoutingRuleId",
                        column: x => x.TemplateRoutingRuleId,
                        principalTable: "TemplateRoutingRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvelopeRoutingConditions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnvelopeRoutingRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceEnvelopeFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Operator = table.Column<int>(type: "int", nullable: false),
                    ComparisonValue = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvelopeRoutingConditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvelopeRoutingConditions_EnvelopeFields_SourceEnvelopeFieldId",
                        column: x => x.SourceEnvelopeFieldId,
                        principalTable: "EnvelopeFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnvelopeRoutingConditions_EnvelopeRoutingRules_EnvelopeRoutingRuleId",
                        column: x => x.EnvelopeRoutingRuleId,
                        principalTable: "EnvelopeRoutingRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Action",
                table: "AuditLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType_EntityId",
                table: "AuditLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_OccurredAt",
                table: "AuditLogs",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_OrganizationId_Name",
                table: "Departments",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeDocuments_EnvelopeId_Order",
                table: "EnvelopeDocuments",
                columns: new[] { "EnvelopeId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeFields_EnvelopeDocumentId",
                table: "EnvelopeFields",
                column: "EnvelopeDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeFields_EnvelopeId",
                table: "EnvelopeFields",
                column: "EnvelopeId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeFields_EnvelopeRecipientId",
                table: "EnvelopeFields",
                column: "EnvelopeRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRecipients_EnvelopeId",
                table: "EnvelopeRecipients",
                column: "EnvelopeId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRecipients_EnvelopeId_RoutingOrder",
                table: "EnvelopeRecipients",
                columns: new[] { "EnvelopeId", "RoutingOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRecipients_UserId",
                table: "EnvelopeRecipients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRoutingConditions_EnvelopeRoutingRuleId",
                table: "EnvelopeRoutingConditions",
                column: "EnvelopeRoutingRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRoutingConditions_SourceEnvelopeFieldId",
                table: "EnvelopeRoutingConditions",
                column: "SourceEnvelopeFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRoutingRules_EnvelopeRoutingRuleSetId_Priority",
                table: "EnvelopeRoutingRules",
                columns: new[] { "EnvelopeRoutingRuleSetId", "Priority" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRoutingRules_TargetRecipientId",
                table: "EnvelopeRoutingRules",
                column: "TargetRecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeRoutingRuleSets_EnvelopeId",
                table: "EnvelopeRoutingRuleSets",
                column: "EnvelopeId");

            migrationBuilder.CreateIndex(
                name: "IX_Envelopes_CreatedByUserId",
                table: "Envelopes",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Envelopes_OrganizationId",
                table: "Envelopes",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Envelopes_OrganizationId_Status",
                table: "Envelopes",
                columns: new[] { "OrganizationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Envelopes_SourceTemplateVersionId",
                table: "Envelopes",
                column: "SourceTemplateVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeStatusHistories_ChangedByUserId",
                table: "EnvelopeStatusHistories",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvelopeStatusHistories_EnvelopeId_ChangedAt",
                table: "EnvelopeStatusHistories",
                columns: new[] { "EnvelopeId", "ChangedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EvelopeEvents_EnvelopeId_OccurredAt",
                table: "EvelopeEvents",
                columns: new[] { "EnvelopeId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_EvelopeEvents_RecipientId",
                table: "EvelopeEvents",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_EvelopeEvents_UserId",
                table: "EvelopeEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Code",
                table: "Organizations",
                column: "Code",
                unique: true,
                filter: "[Code] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionProfilePermissions_PermissionId",
                table: "PermissionProfilePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionProfiles_Name",
                table: "PermissionProfiles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Code",
                table: "Permissions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Signatures_UserId",
                table: "Signatures",
                column: "UserId",
                unique: true,
                filter: "[IsDefault] = 1 AND [IsActive] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Stamps_CreatedByUserId",
                table: "Stamps",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stamps_Name",
                table: "Stamps",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateDocuments_TemplateVersionId_Order",
                table: "TemplateDocuments",
                columns: new[] { "TemplateVersionId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFields_RecipientRoleId",
                table: "TemplateFields",
                column: "RecipientRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFields_TemplateDocumentId",
                table: "TemplateFields",
                column: "TemplateDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRecipientRoles_DefaultUserId",
                table: "TemplateRecipientRoles",
                column: "DefaultUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRecipientRoles_TemplateVersionId_RoutingOrder",
                table: "TemplateRecipientRoles",
                columns: new[] { "TemplateVersionId", "RoutingOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRoutingConditions_SourceTemplateFieldId",
                table: "TemplateRoutingConditions",
                column: "SourceTemplateFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRoutingConditions_TemplateRoutingRuleId",
                table: "TemplateRoutingConditions",
                column: "TemplateRoutingRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRoutingRules_TargetRecipientRoleId",
                table: "TemplateRoutingRules",
                column: "TargetRecipientRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRoutingRules_TemplateRoutingRuleSetId_Priority",
                table: "TemplateRoutingRules",
                columns: new[] { "TemplateRoutingRuleSetId", "Priority" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TemplateRoutingRuleSets_TemplateVersionId",
                table: "TemplateRoutingRuleSets",
                column: "TemplateVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Name",
                table: "Templates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_OrganizationId",
                table: "Templates",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_OrganizationId_Name",
                table: "Templates",
                columns: new[] { "OrganizationId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_OwnerUserId",
                table: "Templates",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateShares_SharedByUserId",
                table: "TemplateShares",
                column: "SharedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateShares_SharedWithDepartmentId",
                table: "TemplateShares",
                column: "SharedWithDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateShares_SharedWithUserId",
                table: "TemplateShares",
                column: "SharedWithUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateShares_TemplateId",
                table: "TemplateShares",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_CreatedByUserId",
                table: "TemplateVersions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_TemplateId_VersionNumber",
                table: "TemplateVersions",
                columns: new[] { "TemplateId", "VersionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionProfiles_PermissionProfileId",
                table: "UserPermissionProfiles",
                column: "PermissionProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissionProfiles_UserId",
                table: "UserPermissionProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalIdentityId",
                table: "Users",
                column: "ExternalIdentityId",
                unique: true,
                filter: "[ExternalIdentityId] is not null");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                table: "Users",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "EnvelopeRoutingConditions");

            migrationBuilder.DropTable(
                name: "EnvelopeStatusHistories");

            migrationBuilder.DropTable(
                name: "EvelopeEvents");

            migrationBuilder.DropTable(
                name: "PermissionProfilePermissions");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Signatures");

            migrationBuilder.DropTable(
                name: "Stamps");

            migrationBuilder.DropTable(
                name: "TemplateRoutingConditions");

            migrationBuilder.DropTable(
                name: "TemplateShares");

            migrationBuilder.DropTable(
                name: "UserPermissionProfiles");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "EnvelopeFields");

            migrationBuilder.DropTable(
                name: "EnvelopeRoutingRules");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "TemplateFields");

            migrationBuilder.DropTable(
                name: "TemplateRoutingRules");

            migrationBuilder.DropTable(
                name: "PermissionProfiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "EnvelopeDocuments");

            migrationBuilder.DropTable(
                name: "EnvelopeRecipients");

            migrationBuilder.DropTable(
                name: "EnvelopeRoutingRuleSets");

            migrationBuilder.DropTable(
                name: "TemplateDocuments");

            migrationBuilder.DropTable(
                name: "TemplateRecipientRoles");

            migrationBuilder.DropTable(
                name: "TemplateRoutingRuleSets");

            migrationBuilder.DropTable(
                name: "Envelopes");

            migrationBuilder.DropTable(
                name: "TemplateVersions");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
