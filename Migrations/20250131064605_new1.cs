﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoleBasedAuthorization.Migrations
{
    /// <inheritdoc />
    public partial class new1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_userRoles_RoleId",
                table: "userRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_userRoles_roles_RoleId",
                table: "userRoles",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userRoles_roles_RoleId",
                table: "userRoles");

            migrationBuilder.DropIndex(
                name: "IX_userRoles_RoleId",
                table: "userRoles");
        }
    }
}
