using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace marketAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Table_Gudang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KodeGudang = table.Column<string>(type: "text", nullable: false),
                    NamaGudang = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table_Gudang", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table_Barang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KodeBarang = table.Column<string>(type: "text", nullable: false),
                    NamaBarang = table.Column<string>(type: "text", nullable: false),
                    HargaBarang = table.Column<decimal>(type: "numeric", nullable: false),
                    JumlahBarang = table.Column<int>(type: "integer", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GudangId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table_Barang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Table_Barang_Table_Gudang_GudangId",
                        column: x => x.GudangId,
                        principalTable: "Table_Gudang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Table_Barang_GudangId",
                table: "Table_Barang",
                column: "GudangId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Table_Barang");

            migrationBuilder.DropTable(
                name: "Table_Gudang");
        }
    }
}
