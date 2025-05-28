using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPDV.Migrations
{
    /// <inheritdoc />
    public partial class AddValorDaVendaToVendasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NomeVendedor",
                table: "Vendas",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "NomeCliente",
                table: "Vendas",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FormaDePagamento",
                table: "Vendas",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorDaVenda",
                table: "Vendas",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorDaVenda",
                table: "Vendas");

            migrationBuilder.UpdateData(
                table: "Vendas",
                keyColumn: "NomeVendedor",
                keyValue: null,
                column: "NomeVendedor",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NomeVendedor",
                table: "Vendas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Vendas",
                keyColumn: "NomeCliente",
                keyValue: null,
                column: "NomeCliente",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NomeCliente",
                table: "Vendas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Vendas",
                keyColumn: "FormaDePagamento",
                keyValue: null,
                column: "FormaDePagamento",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FormaDePagamento",
                table: "Vendas",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
