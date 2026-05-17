using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NRAdmanWebApplicationNet10.Migrations
{
    /// <inheritdoc />
    public partial class MikrotikSimpleQueue2Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mikrotik_simple_queues",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nas_id = table.Column<int>(type: "integer", nullable: false),
                    queue_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    target_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    parent = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    max_limit = table.Column<long>(type: "bigint", nullable: true),
                    burst_limit = table.Column<long>(type: "bigint", nullable: true),
                    burst_threshold = table.Column<long>(type: "bigint", nullable: true),
                    burst_time = table.Column<int>(type: "integer", nullable: true),
                    priority = table.Column<int>(type: "integer", nullable: false),
                    packet_mark = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    comment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    disabled = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    updated_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mikrotik_simple_queues", x => x.id);
                    table.ForeignKey(
                        name: "FK_mikrotik_simple_queues_nas_nas_id",
                        column: x => x.nas_id,
                        principalTable: "nas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_nas_queue_name",
                table: "mikrotik_simple_queues",
                columns: new[] { "nas_id", "queue_name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mikrotik_simple_queues");
        }
    }
}
