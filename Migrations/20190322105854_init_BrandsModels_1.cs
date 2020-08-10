using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VEGA.Migrations
{
    public partial class init_BrandsModels_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "SET IDENTITY_INSERT Brands ON; \n" +
                "insert into Brands(id, name) values (1, 'BMW'), (2, 'Mercedes Benz'); \n" +
                "SET IDENTITY_INSERT Brands OFF; \n" +

                "insert into Models(BrandId, name) values (1,'Z3'),  (1, 'Z4'),  (1,'X3'),  (1, 'M5') ; \n" +
                "insert into Models(BrandId, name) values (2,'C class'),  (2, 'E class'),  (2,'S class'),  (2, 'SL class'),  (2, 'SLK class'); \n"
                );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from models");
            migrationBuilder.Sql("delete from brands"); 
        }
    }
}
