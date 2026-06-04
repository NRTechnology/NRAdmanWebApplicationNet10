using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRouter2NetworkRouter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_Routers_RouterId",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikSimpleQueues_Routers_RouterId",
                table: "MikrotikSimpleQueues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routers",
                table: "Routers");

            migrationBuilder.RenameTable(
                name: "Routers",
                newName: "NetworkRouters");

            migrationBuilder.RenameIndex(
                name: "IX_Routers_Name",
                table: "NetworkRouters",
                newName: "IX_NetworkRouters_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Routers_IpAddress",
                table: "NetworkRouters",
                newName: "IX_NetworkRouters_IpAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NetworkRouters",
                table: "NetworkRouters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikQueueConfigs_NetworkRouters_RouterId",
                table: "MikrotikQueueConfigs",
                column: "RouterId",
                principalTable: "NetworkRouters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikSimpleQueues_NetworkRouters_RouterId",
                table: "MikrotikSimpleQueues",
                column: "RouterId",
                principalTable: "NetworkRouters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_NetworkRouters_RouterId",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikSimpleQueues_NetworkRouters_RouterId",
                table: "MikrotikSimpleQueues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NetworkRouters",
                table: "NetworkRouters");

            migrationBuilder.RenameTable(
                name: "NetworkRouters",
                newName: "Routers");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkRouters_Name",
                table: "Routers",
                newName: "IX_Routers_Name");

            migrationBuilder.RenameIndex(
                name: "IX_NetworkRouters_IpAddress",
                table: "Routers",
                newName: "IX_Routers_IpAddress");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routers",
                table: "Routers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikQueueConfigs_Routers_RouterId",
                table: "MikrotikQueueConfigs",
                column: "RouterId",
                principalTable: "Routers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikSimpleQueues_Routers_RouterId",
                table: "MikrotikSimpleQueues",
                column: "RouterId",
                principalTable: "Routers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
