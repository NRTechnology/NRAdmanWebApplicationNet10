using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class AddMikrotikProfile2Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mikrotik_radius_accounting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    nas_ip_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    acct_input_octets = table.Column<long>(type: "bigint", nullable: true),
                    acct_output_octets = table.Column<long>(type: "bigint", nullable: true),
                    acct_input_packets = table.Column<long>(type: "bigint", nullable: true),
                    acct_output_packets = table.Column<long>(type: "bigint", nullable: true),
                    acct_session_time = table.Column<long>(type: "bigint", nullable: true),
                    acct_status_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    acct_session_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    acct_terminate_cause = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_radius_accounting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mikrotik_radius_policies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    policy_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    download_limit = table.Column<decimal>(type: "numeric", nullable: true),
                    upload_limit = table.Column<decimal>(type: "numeric", nullable: true),
                    burst_limit_down = table.Column<decimal>(type: "numeric", nullable: true),
                    burst_limit_up = table.Column<decimal>(type: "numeric", nullable: true),
                    burst_threshold_down = table.Column<int>(type: "integer", nullable: true),
                    burst_threshold_up = table.Column<int>(type: "integer", nullable: true),
                    burst_time = table.Column<int>(type: "integer", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    modified_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_radius_policies", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mikrotik_queue_config",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    router_id = table.Column<Guid>(type: "uuid", nullable: true),
                    policy_id = table.Column<int>(type: "integer", nullable: true),
                    mikrotik_queue_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    queue_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    target_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    deployment_status = table.Column<int>(type: "integer", nullable: false),
                    sync_status = table.Column<int>(type: "integer", nullable: true),
                    last_error = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    deployed_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sync_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    config_version = table.Column<int>(type: "integer", nullable: false),
                    config_metadata = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    modified_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_queue_config", x => x.id);
                    table.ForeignKey(
                        name: "FK_mikrotik_queue_config_mikrotik_radius_policies_policy_id",
                        column: x => x.policy_id,
                        principalTable: "mikrotik_radius_policies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_mikrotik_queue_config_routers_router_id",
                        column: x => x.router_id,
                        principalTable: "routers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_queue_config_policy_id",
                table: "mikrotik_queue_config",
                column: "policy_id");

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_queue_config_router_id",
                table: "mikrotik_queue_config",
                column: "router_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mikrotik_queue_config");

            migrationBuilder.DropTable(
                name: "mikrotik_radius_accounting");

            migrationBuilder.DropTable(
                name: "mikrotik_radius_policies");
        }
    }
}
