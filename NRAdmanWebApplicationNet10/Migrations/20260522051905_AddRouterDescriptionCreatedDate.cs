using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class AddRouterDescriptionCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "routers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "routers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "routers",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "routers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "routers");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "routers");
        }
    }
}
