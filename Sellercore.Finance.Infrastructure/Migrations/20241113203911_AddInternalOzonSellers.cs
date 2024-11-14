using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Sellercore.Finance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInternalOzonSellers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalOzonSellers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalOzonSellers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OzonSellerProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    OfferId = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Sku = table.Column<int>(type: "integer", nullable: true),
                    Cost = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OzonSellerProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OzonSellerProducts_InternalOzonSellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "InternalOzonSellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OzonSellerProducts_SellerId",
                table: "OzonSellerProducts",
                column: "SellerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OzonSellerProducts");

            migrationBuilder.DropTable(
                name: "InternalOzonSellers");
        }
    }
}
