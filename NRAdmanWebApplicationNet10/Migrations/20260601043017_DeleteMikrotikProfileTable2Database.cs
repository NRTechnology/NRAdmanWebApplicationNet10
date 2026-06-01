using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class DeleteMikrotikProfileTable2Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mikrotik_radius_accounting");

            migrationBuilder.DropTable(
                name: "mikrotik_radius_policy");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_routers_IpAddress",
                table: "routers");

            migrationBuilder.DropIndex(
                name: "IX_routers_Name",
                table: "routers");

            migrationBuilder.CreateTable(
                name: "mikrotik_radius_accounting",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    acct_input_octets = table.Column<long>(type: "bigint", nullable: true),
                    acct_input_packets = table.Column<long>(type: "bigint", nullable: true),
                    acct_output_octets = table.Column<long>(type: "bigint", nullable: true),
                    acct_output_packets = table.Column<long>(type: "bigint", nullable: true),
                    acct_session_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    acct_session_time = table.Column<long>(type: "bigint", nullable: true),
                    acct_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    acct_status_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    acct_stop_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    acct_terminate_cause = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    called_station_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    calling_station_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    framed_ip_address = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    nas_ip_address = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    nas_port = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
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
                    burst_limit_down = table.Column<decimal>(type: "numeric", nullable: true),
                    burst_limit_up = table.Column<decimal>(type: "numeric", nullable: true),
                    burst_threshold_down = table.Column<int>(type: "integer", nullable: true),
                    burst_threshold_up = table.Column<int>(type: "integer", nullable: true),
                    burst_time = table.Column<int>(type: "integer", nullable: true),
                    created_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    download_limit = table.Column<decimal>(type: "numeric", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    max_limit_down = table.Column<decimal>(type: "numeric", nullable: true),
                    max_limit_up = table.Column<decimal>(type: "numeric", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    policy_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    queue_type = table.Column<int>(type: "integer", nullable: false),
                    upload_limit = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_radius_policy", x => x.id);
                });
        }
    }
}
