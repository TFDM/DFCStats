using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePeopleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Nationalities_NationalityID",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "NationalityID",
                table: "People",
                newName: "NationalityId");

            migrationBuilder.RenameIndex(
                name: "IX_People_NationalityID",
                table: "People",
                newName: "IX_People_NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Nationalities_NationalityId",
                table: "People",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Nationalities_NationalityId",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "NationalityId",
                table: "People",
                newName: "NationalityID");

            migrationBuilder.RenameIndex(
                name: "IX_People_NationalityId",
                table: "People",
                newName: "IX_People_NationalityID");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Nationalities_NationalityID",
                table: "People",
                column: "NationalityID",
                principalTable: "Nationalities",
                principalColumn: "Id");
        }
    }
}
