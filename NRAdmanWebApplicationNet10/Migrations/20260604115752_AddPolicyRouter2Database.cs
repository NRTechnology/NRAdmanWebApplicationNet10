using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicyRouter2Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mikrotik_queue_config_mikrotik_radius_policies_PolicyId",
                table: "mikrotik_queue_config");

            migrationBuilder.DropForeignKey(
                name: "FK_mikrotik_queue_config_routers_RouterId",
                table: "mikrotik_queue_config");

            migrationBuilder.DropForeignKey(
                name: "FK_mikrotik_simple_queues_routers_RouterId",
                table: "mikrotik_simple_queues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_routers",
                table: "routers");

            migrationBuilder.DropIndex(
                name: "RouterId_index_unique",
                table: "routers");

            migrationBuilder.DropIndex(
                name: "NasName_index_unique",
                table: "nas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mikrotik_simple_queues",
                table: "mikrotik_simple_queues");

            migrationBuilder.DropIndex(
                name: "idx_router_queue_name",
                table: "mikrotik_simple_queues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mikrotik_radius_policies",
                table: "mikrotik_radius_policies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mikrotik_radius_accounting",
                table: "mikrotik_radius_accounting");

            migrationBuilder.DropIndex(
                name: "IX_mikrotik_radius_accounting_CreatedDate",
                table: "mikrotik_radius_accounting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mikrotik_queue_config",
                table: "mikrotik_queue_config");

            migrationBuilder.DropIndex(
                name: "IX_mikrotik_queue_config_PolicyId",
                table: "mikrotik_queue_config");

            migrationBuilder.DropIndex(
                name: "IX_mikrotik_queue_config_RouterId",
                table: "mikrotik_queue_config");

            migrationBuilder.RenameTable(
                name: "routers",
                newName: "Routers");

            migrationBuilder.RenameTable(
                name: "mikrotik_simple_queues",
                newName: "MikrotikSimpleQueues");

            migrationBuilder.RenameTable(
                name: "mikrotik_radius_policies",
                newName: "MikrotikRadiusPolicies");

            migrationBuilder.RenameTable(
                name: "mikrotik_radius_accounting",
                newName: "MikrotikRadiusAccounting");

            migrationBuilder.RenameTable(
                name: "mikrotik_queue_config",
                newName: "MikrotikQueueConfigs");

            migrationBuilder.RenameIndex(
                name: "IX_routers_Name",
                table: "Routers",
                newName: "IX_Routers_Name");

            migrationBuilder.RenameIndex(
                name: "IX_routers_IpAddress",
                table: "Routers",
                newName: "IX_Routers_IpAddress");

            migrationBuilder.RenameIndex(
                name: "IX_mikrotik_radius_policies_PolicyName",
                table: "MikrotikRadiusPolicies",
                newName: "IX_MikrotikRadiusPolicies_PolicyName");

            migrationBuilder.AlterColumn<Guid>(
                name: "PolicyId",
                table: "MikrotikQueueConfigs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MikrotikRadiusPolicy",
                table: "MikrotikQueueConfigs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NetworkRouter",
                table: "MikrotikQueueConfigs",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routers",
                table: "Routers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MikrotikSimpleQueues",
                table: "MikrotikSimpleQueues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MikrotikRadiusPolicies",
                table: "MikrotikRadiusPolicies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MikrotikRadiusAccounting",
                table: "MikrotikRadiusAccounting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MikrotikQueueConfigs",
                table: "MikrotikQueueConfigs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Status",
                table: "Transactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MikrotikSimpleQueues_RouterId",
                table: "MikrotikSimpleQueues",
                column: "RouterId");

            migrationBuilder.CreateIndex(
                name: "IX_MikrotikQueueConfigs_MikrotikRadiusPolicy",
                table: "MikrotikQueueConfigs",
                column: "MikrotikRadiusPolicy");

            migrationBuilder.CreateIndex(
                name: "IX_MikrotikQueueConfigs_NetworkRouter",
                table: "MikrotikQueueConfigs",
                column: "NetworkRouter");

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikQueueConfigs_MikrotikRadiusPolicies_MikrotikRadiusP~",
                table: "MikrotikQueueConfigs",
                column: "MikrotikRadiusPolicy",
                principalTable: "MikrotikRadiusPolicies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikQueueConfigs_Routers_NetworkRouter",
                table: "MikrotikQueueConfigs",
                column: "NetworkRouter",
                principalTable: "Routers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikSimpleQueues_Routers_RouterId",
                table: "MikrotikSimpleQueues",
                column: "RouterId",
                principalTable: "Routers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_MikrotikRadiusPolicies_MikrotikRadiusP~",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_Routers_NetworkRouter",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikSimpleQueues_Routers_RouterId",
                table: "MikrotikSimpleQueues");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Status",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routers",
                table: "Routers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MikrotikSimpleQueues",
                table: "MikrotikSimpleQueues");

            migrationBuilder.DropIndex(
                name: "IX_MikrotikSimpleQueues_RouterId",
                table: "MikrotikSimpleQueues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MikrotikRadiusPolicies",
                table: "MikrotikRadiusPolicies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MikrotikRadiusAccounting",
                table: "MikrotikRadiusAccounting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MikrotikQueueConfigs",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropIndex(
                name: "IX_MikrotikQueueConfigs_MikrotikRadiusPolicy",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropIndex(
                name: "IX_MikrotikQueueConfigs_NetworkRouter",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropColumn(
                name: "MikrotikRadiusPolicy",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropColumn(
                name: "NetworkRouter",
                table: "MikrotikQueueConfigs");

            migrationBuilder.RenameTable(
                name: "Routers",
                newName: "routers");

            migrationBuilder.RenameTable(
                name: "MikrotikSimpleQueues",
                newName: "mikrotik_simple_queues");

            migrationBuilder.RenameTable(
                name: "MikrotikRadiusPolicies",
                newName: "mikrotik_radius_policies");

            migrationBuilder.RenameTable(
                name: "MikrotikRadiusAccounting",
                newName: "mikrotik_radius_accounting");

            migrationBuilder.RenameTable(
                name: "MikrotikQueueConfigs",
                newName: "mikrotik_queue_config");

            migrationBuilder.RenameIndex(
                name: "IX_Routers_Name",
                table: "routers",
                newName: "IX_routers_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Routers_IpAddress",
                table: "routers",
                newName: "IX_routers_IpAddress");

            migrationBuilder.RenameIndex(
                name: "IX_MikrotikRadiusPolicies_PolicyName",
                table: "mikrotik_radius_policies",
                newName: "IX_mikrotik_radius_policies_PolicyName");

            migrationBuilder.AlterColumn<Guid>(
                name: "PolicyId",
                table: "mikrotik_queue_config",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_routers",
                table: "routers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mikrotik_simple_queues",
                table: "mikrotik_simple_queues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mikrotik_radius_policies",
                table: "mikrotik_radius_policies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mikrotik_radius_accounting",
                table: "mikrotik_radius_accounting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mikrotik_queue_config",
                table: "mikrotik_queue_config",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "RouterId_index_unique",
                table: "routers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "NasName_index_unique",
                table: "nas",
                column: "nasname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_router_queue_name",
                table: "mikrotik_simple_queues",
                columns: new[] { "RouterId", "QueueName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_radius_accounting_CreatedDate",
                table: "mikrotik_radius_accounting",
                column: "CreatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_queue_config_PolicyId",
                table: "mikrotik_queue_config",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_mikrotik_queue_config_RouterId",
                table: "mikrotik_queue_config",
                column: "RouterId");

            migrationBuilder.AddForeignKey(
                name: "FK_mikrotik_queue_config_mikrotik_radius_policies_PolicyId",
                table: "mikrotik_queue_config",
                column: "PolicyId",
                principalTable: "mikrotik_radius_policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_mikrotik_queue_config_routers_RouterId",
                table: "mikrotik_queue_config",
                column: "RouterId",
                principalTable: "routers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_mikrotik_simple_queues_routers_RouterId",
                table: "mikrotik_simple_queues",
                column: "RouterId",
                principalTable: "routers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
