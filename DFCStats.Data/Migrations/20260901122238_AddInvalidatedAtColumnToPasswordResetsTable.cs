using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFCStats.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInvalidatedAtColumnToPasswordResetsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InvalidatedAt",
                table: "PasswordResets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvalidatedAt",
                table: "PasswordResets");
        }
    }
}
