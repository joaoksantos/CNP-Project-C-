using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoCNP.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByAtCriminosoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CriadoPorUsuarioId",
                table: "Criminosos",
                type: "int",
                nullable: true
                );

            migrationBuilder.AddColumn<int>(
                name: "SituacaoPena",
                table: "Criminosos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Criminosos_CriadoPorUsuarioId",
                table: "Criminosos",
                column: "CriadoPorUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Criminosos_Usuarios_CriadoPorUsuarioId",
                table: "Criminosos",
                column: "CriadoPorUsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criminosos_Usuarios_CriadoPorUsuarioId",
                table: "Criminosos");

            migrationBuilder.DropIndex(
                name: "IX_Criminosos_CriadoPorUsuarioId",
                table: "Criminosos");

            migrationBuilder.DropColumn(
                name: "CriadoPorUsuarioId",
                table: "Criminosos");

            migrationBuilder.DropColumn(
                name: "SituacaoPena",
                table: "Criminosos");
        }
    }
}
