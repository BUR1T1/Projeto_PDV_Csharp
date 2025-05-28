using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPDV.Migrations
{
    /// <inheritdoc />
    public partial class AddNomeProdutoEValorUnitarioToItemDaVenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeProduto",
                table: "ItensDaVenda",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorUnitario",
                table: "ItensDaVenda",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeProduto",
                table: "ItensDaVenda");

            migrationBuilder.DropColumn(
                name: "ValorUnitario",
                table: "ItensDaVenda");
        }
    }
}
