using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class AddMikrotikRadius2Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ports",
                table: "routers",
                newName: "SShPort");

            migrationBuilder.AddColumn<int>(
                name: "ApiPort",
                table: "routers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "mikrotik_radius_accounting",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    nas_ip_address = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    nas_port = table.Column<int>(type: "integer", nullable: true),
                    acct_session_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    acct_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    acct_stop_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    acct_session_time = table.Column<long>(type: "bigint", nullable: true),
                    acct_input_octets = table.Column<long>(type: "bigint", nullable: true),
                    acct_output_octets = table.Column<long>(type: "bigint", nullable: true),
                    acct_input_packets = table.Column<long>(type: "bigint", nullable: true),
                    acct_output_packets = table.Column<long>(type: "bigint", nullable: true),
                    acct_terminate_cause = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    framed_ip_address = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    called_station_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    calling_station_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    acct_status_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_radius_accounting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mikrotik_radius_policy",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    policy_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    download_limit = table.Column<decimal>(type: "numeric", nullable: true),
                    upload_limit = table.Column<decimal>(type: "numeric", nullable: true),
                    queue_type = table.Column<int>(type: "integer", nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    burst_limit_down = table.Column<decimal>(type: "numeric", nullable: true),
                    burst_limit_up = table.Column<decimal>(type: "numeric", nullable: true),
                    burst_threshold_down = table.Column<int>(type: "integer", nullable: true),
                    burst_threshold_up = table.Column<int>(type: "integer", nullable: true),
                    burst_time = table.Column<int>(type: "integer", nullable: true),
                    max_limit_down = table.Column<decimal>(type: "numeric", nullable: true),
                    max_limit_up = table.Column<decimal>(type: "numeric", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    modified_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_radius_policy", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mikrotik_radius_accounting");

            migrationBuilder.DropTable(
                name: "mikrotik_radius_policy");

            migrationBuilder.DropColumn(
                name: "ApiPort",
                table: "routers");

            migrationBuilder.RenameColumn(
                name: "SShPort",
                table: "routers",
                newName: "Ports");
        }
    }
}
