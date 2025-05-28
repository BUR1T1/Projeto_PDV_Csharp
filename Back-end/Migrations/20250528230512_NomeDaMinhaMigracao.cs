using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPDV.Migrations
{
    /// <inheritdoc />
    public partial class NomeDaMinhaMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CPF_do_Clinte",
                table: "clientes",
                newName: "Telefone");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "clientes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "clientes");

            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "clientes",
                newName: "CPF_do_Clinte");
        }
    }
}
