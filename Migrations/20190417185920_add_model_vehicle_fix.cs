using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VEGA.Migrations
{
    public partial class add_model_vehicle_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Features",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContactEmail = table.Column<string>(maxLength: 64, nullable: true),
                    ContactName = table.Column<string>(maxLength: 64, nullable: true),
                    ContactPhone = table.Column<string>(maxLength: 64, nullable: true),
                    IsRegistered = table.Column<bool>(nullable: false),
                    modelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Models_modelId",
                        column: x => x.modelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_VehicleId",
                table: "Features",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_modelId",
                table: "Vehicles",
                column: "modelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Vehicles_VehicleId",
                table: "Features",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Vehicles_VehicleId",
                table: "Features");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Features_VehicleId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Features");
        }
    }
}
