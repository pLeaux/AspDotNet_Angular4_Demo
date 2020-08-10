using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VEGA.Migrations
{
    public partial class model_vehicle_fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Models_modelId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "modelId",
                table: "Vehicles",
                newName: "ModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_modelId",
                table: "Vehicles",
                newName: "IX_Vehicles_ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Models_ModelId",
                table: "Vehicles",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Models_ModelId",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Vehicles",
                newName: "modelId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_ModelId",
                table: "Vehicles",
                newName: "IX_Vehicles_modelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Models_modelId",
                table: "Vehicles",
                column: "modelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
