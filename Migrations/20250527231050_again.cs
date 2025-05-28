using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebPDV.Migrations
{
    /// <inheritdoc />
    public partial class again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemDaVenda_Vendas_VendaId",
                table: "ItemDaVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDaVenda_produtos_ProdutoId",
                table: "ItemDaVenda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_produtos",
                table: "produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemDaVenda",
                table: "ItemDaVenda");

            migrationBuilder.RenameTable(
                name: "produtos",
                newName: "Produtos");

            migrationBuilder.RenameTable(
                name: "ItemDaVenda",
                newName: "ItensDaVenda");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDaVenda_VendaId",
                table: "ItensDaVenda",
                newName: "IX_ItensDaVenda_VendaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemDaVenda_ProdutoId",
                table: "ItensDaVenda",
                newName: "IX_ItensDaVenda_ProdutoId");

            migrationBuilder.AlterColumn<int>(
                name: "VendaId",
                table: "ItensDaVenda",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensDaVenda",
                table: "ItensDaVenda",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensDaVenda_Produtos_ProdutoId",
                table: "ItensDaVenda",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensDaVenda_Vendas_VendaId",
                table: "ItensDaVenda",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensDaVenda_Produtos_ProdutoId",
                table: "ItensDaVenda");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensDaVenda_Vendas_VendaId",
                table: "ItensDaVenda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensDaVenda",
                table: "ItensDaVenda");

            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "produtos");

            migrationBuilder.RenameTable(
                name: "ItensDaVenda",
                newName: "ItemDaVenda");

            migrationBuilder.RenameIndex(
                name: "IX_ItensDaVenda_VendaId",
                table: "ItemDaVenda",
                newName: "IX_ItemDaVenda_VendaId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensDaVenda_ProdutoId",
                table: "ItemDaVenda",
                newName: "IX_ItemDaVenda_ProdutoId");

            migrationBuilder.AlterColumn<int>(
                name: "VendaId",
                table: "ItemDaVenda",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_produtos",
                table: "produtos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemDaVenda",
                table: "ItemDaVenda",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDaVenda_Vendas_VendaId",
                table: "ItemDaVenda",
                column: "VendaId",
                principalTable: "Vendas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDaVenda_produtos_ProdutoId",
                table: "ItemDaVenda",
                column: "ProdutoId",
                principalTable: "produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
