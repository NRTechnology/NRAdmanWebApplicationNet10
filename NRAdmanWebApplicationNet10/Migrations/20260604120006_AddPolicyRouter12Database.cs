using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicyRouter12Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_MikrotikRadiusPolicies_MikrotikRadiusP~",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_Routers_NetworkRouter",
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

            migrationBuilder.CreateIndex(
                name: "IX_MikrotikQueueConfigs_PolicyId",
                table: "MikrotikQueueConfigs",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_MikrotikQueueConfigs_RouterId",
                table: "MikrotikQueueConfigs",
                column: "RouterId");

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikQueueConfigs_MikrotikRadiusPolicies_PolicyId",
                table: "MikrotikQueueConfigs",
                column: "PolicyId",
                principalTable: "MikrotikRadiusPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MikrotikQueueConfigs_Routers_RouterId",
                table: "MikrotikQueueConfigs",
                column: "RouterId",
                principalTable: "Routers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_MikrotikRadiusPolicies_PolicyId",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_MikrotikQueueConfigs_Routers_RouterId",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropIndex(
                name: "IX_MikrotikQueueConfigs_PolicyId",
                table: "MikrotikQueueConfigs");

            migrationBuilder.DropIndex(
                name: "IX_MikrotikQueueConfigs_RouterId",
                table: "MikrotikQueueConfigs");

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
        }
    }
}
