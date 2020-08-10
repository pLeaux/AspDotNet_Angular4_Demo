using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VEGA.Migrations
{
    public partial class add_VehicleFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Vehicles_VehicleId",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Features_VehicleId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Features");

            migrationBuilder.CreateTable(
                name: "VehicleFeatures",
                columns: table => new
                {
                    VehicleId = table.Column<int>(nullable: false),
                    FeatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleFeatures", x => new { x.VehicleId, x.FeatureId });
                    table.UniqueConstraint("AK_VehicleFeatures_FeatureId_VehicleId", x => new { x.FeatureId, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_VehicleFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleFeatures_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleFeatures");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Features",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_VehicleId",
                table: "Features",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Vehicles_VehicleId",
                table: "Features",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
