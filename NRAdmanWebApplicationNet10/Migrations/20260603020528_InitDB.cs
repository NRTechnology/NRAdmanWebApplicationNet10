using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    WargaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfilePicture = table.Column<byte[]>(type: "bytea", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoginAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    IpAddress = table.Column<string>(type: "text", nullable: true),
                    AttemptedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    AttemptType = table.Column<string>(type: "text", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mikrotik_radius_accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    NasIpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AcctInputOctets = table.Column<long>(type: "bigint", nullable: true),
                    AcctOutputOctets = table.Column<long>(type: "bigint", nullable: true),
                    AcctInputPackets = table.Column<long>(type: "bigint", nullable: true),
                    AcctOutputPackets = table.Column<long>(type: "bigint", nullable: true),
                    AcctSessionTime = table.Column<long>(type: "bigint", nullable: true),
                    AcctStatusType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    AcctSessionId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AcctTerminateCause = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_radius_accounting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "mikrotik_radius_policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PolicyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DownloadLimit = table.Column<decimal>(type: "numeric", nullable: true),
                    UploadLimit = table.Column<decimal>(type: "numeric", nullable: true),
                    BurstLimitDown = table.Column<decimal>(type: "numeric", nullable: true),
                    BurstLimitUp = table.Column<decimal>(type: "numeric", nullable: true),
                    BurstThresholdDown = table.Column<int>(type: "integer", nullable: true),
                    BurstThresholdUp = table.Column<int>(type: "integer", nullable: true),
                    BurstTime = table.Column<int>(type: "integer", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_radius_policies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nasname = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    shortname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ports = table.Column<int>(type: "integer", nullable: false),
                    secret = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    server = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    community = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RateLimit = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    SessionTimeout = table.Column<int>(type: "integer", nullable: false),
                    IdleTimeout = table.Column<int>(type: "integer", nullable: true),
                    QuotaMb = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "radacct",
                columns: table => new
                {
                    radacctid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    acctsessionid = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    acctuniqueid = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    username = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    realm = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    nasipaddress = table.Column<IPAddress>(type: "inet", nullable: false),
                    nasportid = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    nasporttype = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    acctstarttime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    acctupdatetime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    acctstoptime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    acctinterval = table.Column<long>(type: "bigint", nullable: true),
                    acctsessiontime = table.Column<long>(type: "bigint", nullable: true),
                    acctauthentic = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    connectinfo_start = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    connectinfo_stop = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    acctinputoctets = table.Column<long>(type: "bigint", nullable: true),
                    acctoutputoctets = table.Column<long>(type: "bigint", nullable: true),
                    calledstationid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    callingstationid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    acctterminatecause = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    servicetype = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    framedprotocol = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    framedipaddress = table.Column<IPAddress>(type: "inet", nullable: true),
                    @class = table.Column<string>(name: "class", type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radacct", x => x.radacctid);
                });

            migrationBuilder.CreateTable(
                name: "radcheck",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    value = table.Column<string>(type: "character varying(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radcheck", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radgroupcheck",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupname = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    value = table.Column<string>(type: "character varying(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radgroupcheck", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radgroupreply",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupname = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    value = table.Column<string>(type: "character varying(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radgroupreply", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radreply",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    value = table.Column<string>(type: "character varying(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radreply", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "radusergroup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    groupname = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radusergroup", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "routers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouterType = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SShPort = table.Column<int>(type: "integer", nullable: false),
                    ApiPort = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PaidAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValidityDays = table.Column<int>(type: "integer", nullable: false),
                    MaxDevices = table.Column<int>(type: "integer", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    UsedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UsedBy = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mikrotik_queue_config",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouterId = table.Column<Guid>(type: "uuid", nullable: false),
                    PolicyId = table.Column<Guid>(type: "uuid", nullable: true),
                    MikrotikQueueId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    QueueName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    TargetAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DeploymentStatus = table.Column<int>(type: "integer", nullable: false),
                    SyncStatus = table.Column<int>(type: "integer", nullable: true),
                    LastError = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DeployedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConfigVersion = table.Column<int>(type: "integer", nullable: false),
                    ConfigMetadata = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ModifiedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_queue_config", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mikrotik_queue_config_mikrotik_radius_policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "mikrotik_radius_policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_mikrotik_queue_config_routers_RouterId",
                        column: x => x.RouterId,
                        principalTable: "routers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "mikrotik_simple_queues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouterId = table.Column<Guid>(type: "uuid", nullable: false),
                    QueueName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TargetAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Parent = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MaxLimit = table.Column<long>(type: "bigint", nullable: true),
                    BurstLimit = table.Column<long>(type: "bigint", nullable: true),
                    BurstThreshold = table.Column<long>(type: "bigint", nullable: true),
                    BurstTime = table.Column<int>(type: "integer", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    PacketMark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Disabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    UpdatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_simple_queues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mikrotik_simple_queues_routers_RouterId",
                        column: x => x.RouterId,
                        principalTable: "routers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserName",
                table: "Customers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_queue_config_PolicyId",
                table: "mikrotik_queue_config",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_queue_config_RouterId",
                table: "mikrotik_queue_config",
                column: "RouterId");

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_radius_accounting_CreatedDate",
                table: "mikrotik_radius_accounting",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_radius_policies_PolicyName",
                table: "mikrotik_radius_policies",
                column: "PolicyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_router_queue_name",
                table: "mikrotik_simple_queues",
                columns: new[] { "RouterId", "QueueName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "NasName_index_unique",
                table: "nas",
                column: "nasname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_Code",
                table: "Packages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routers_IpAddress",
                table: "routers",
                column: "IpAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routers_Name",
                table: "routers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RouterId_index_unique",
                table: "routers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CustomerId",
                table: "Transactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PackageId",
                table: "Transactions",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionCode",
                table: "Transactions",
                column: "TransactionCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_Code",
                table: "Vouchers",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_PackageId",
                table: "Vouchers",
                column: "PackageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LoginAttempts");

            migrationBuilder.DropTable(
                name: "mikrotik_queue_config");

            migrationBuilder.DropTable(
                name: "mikrotik_radius_accounting");

            migrationBuilder.DropTable(
                name: "mikrotik_simple_queues");

            migrationBuilder.DropTable(
                name: "nas");

            migrationBuilder.DropTable(
                name: "radacct");

            migrationBuilder.DropTable(
                name: "radcheck");

            migrationBuilder.DropTable(
                name: "radgroupcheck");

            migrationBuilder.DropTable(
                name: "radgroupreply");

            migrationBuilder.DropTable(
                name: "radreply");

            migrationBuilder.DropTable(
                name: "radusergroup");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "mikrotik_radius_policies");

            migrationBuilder.DropTable(
                name: "routers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Packages");
        }
    }
}
