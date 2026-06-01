using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class AddMikrotikPolicyContext2Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mikrotik_queue_config_mikrotik_radius_policies_policy_id",
                table: "mikrotik_queue_config");

            migrationBuilder.DropForeignKey(
                name: "FK_mikrotik_queue_config_routers_router_id",
                table: "mikrotik_queue_config");

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_radius_policies_policy_name",
                table: "mikrotik_radius_policies",
                column: "policy_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_radius_accounting_created_date",
                table: "mikrotik_radius_accounting",
                column: "created_date");

            migrationBuilder.AddForeignKey(
                name: "FK_mikrotik_queue_config_mikrotik_radius_policies_policy_id",
                table: "mikrotik_queue_config",
                column: "policy_id",
                principalTable: "mikrotik_radius_policies",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_mikrotik_queue_config_routers_router_id",
                table: "mikrotik_queue_config",
                column: "router_id",
                principalTable: "routers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mikrotik_queue_config_mikrotik_radius_policies_policy_id",
                table: "mikrotik_queue_config");

            migrationBuilder.DropForeignKey(
                name: "FK_mikrotik_queue_config_routers_router_id",
                table: "mikrotik_queue_config");

            migrationBuilder.DropIndex(
                name: "IX_mikrotik_radius_policies_policy_name",
                table: "mikrotik_radius_policies");

            migrationBuilder.DropIndex(
                name: "IX_mikrotik_radius_accounting_created_date",
                table: "mikrotik_radius_accounting");

            migrationBuilder.AddForeignKey(
                name: "FK_mikrotik_queue_config_mikrotik_radius_policies_policy_id",
                table: "mikrotik_queue_config",
                column: "policy_id",
                principalTable: "mikrotik_radius_policies",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_mikrotik_queue_config_routers_router_id",
                table: "mikrotik_queue_config",
                column: "router_id",
                principalTable: "routers",
                principalColumn: "Id");
        }
    }
}
