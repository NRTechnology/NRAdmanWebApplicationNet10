using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class RadiusDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob");

            migrationBuilder.CreateTable(
                name: "nas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nasname = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    shortname = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    ports = table.Column<int>(type: "int", nullable: true),
                    secret = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    server = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    community = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nas", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "radacct",
                columns: table => new
                {
                    radacctid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    acctsessionid = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    acctuniqueid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    username = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    realm = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    nasipaddress = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    nasportid = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    nasporttype = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    acctstarttime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    acctupdatetime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    acctstoptime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    acctinterval = table.Column<int>(type: "int", nullable: true),
                    acctsessiontime = table.Column<uint>(type: "int unsigned", nullable: true),
                    acctauthentic = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    connectinfo_start = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    connectinfo_stop = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    acctinputoctets = table.Column<long>(type: "bigint", nullable: true),
                    acctoutputoctets = table.Column<long>(type: "bigint", nullable: true),
                    calledstationid = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    callingstationid = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    acctterminatecause = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    servicetype = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    framedprotocol = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    framedipaddress = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    framedipv6address = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    framedipv6prefix = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    framedinterfaceid = table.Column<string>(type: "varchar(44)", maxLength: 44, nullable: false),
                    delegatedipv6prefix = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    @class = table.Column<string>(name: "class", type: "varchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radacct", x => x.radacctid);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "radcheck",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    value = table.Column<string>(type: "varchar(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radcheck", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "radgroupcheck",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    groupname = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    value = table.Column<string>(type: "varchar(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radgroupcheck", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "radgroupreply",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    groupname = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    value = table.Column<string>(type: "varchar(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radgroupreply", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "radpostauth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    pass = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    reply = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    authdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    @class = table.Column<string>(name: "class", type: "varchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radpostauth", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "radreply",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    attribute = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    op = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    value = table.Column<string>(type: "varchar(253)", maxLength: 253, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radreply", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "radusergroup",
                columns: table => new
                {
                    id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    groupname = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radusergroup", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "nasname",
                table: "nas",
                column: "nasname");

            migrationBuilder.CreateIndex(
                name: "acctuniqueid",
                table: "radacct",
                column: "acctuniqueid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "radpostauth");

            migrationBuilder.DropTable(
                name: "radreply");

            migrationBuilder.DropTable(
                name: "radusergroup");

            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "longblob",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);
        }
    }
}
