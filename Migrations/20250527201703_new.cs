using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPDV.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorDaComissao",
                table: "vendedores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ValorDaComissao",
                table: "vendedores",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
