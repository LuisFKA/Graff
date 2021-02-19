using Microsoft.EntityFrameworkCore.Migrations;

namespace Graff.Data.Migrations
{
    public partial class lanceupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lance_Produto_ProdutoId",
                table: "Lance");

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "Lance",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lance_Produto_ProdutoId",
                table: "Lance",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lance_Produto_ProdutoId",
                table: "Lance");

            migrationBuilder.AlterColumn<int>(
                name: "ProdutoId",
                table: "Lance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Lance_Produto_ProdutoId",
                table: "Lance",
                column: "ProdutoId",
                principalTable: "Produto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
