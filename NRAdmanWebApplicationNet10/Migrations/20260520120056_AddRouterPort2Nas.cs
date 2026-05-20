using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class AddRouterPort2Nas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ports",
                table: "nas",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "routerports",
                table: "nas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "routerports",
                table: "nas");

            migrationBuilder.AlterColumn<int>(
                name: "ports",
                table: "nas",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
