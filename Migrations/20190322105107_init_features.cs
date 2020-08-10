using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VEGA.Migrations
{
    public partial class init_features : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Features(name) values ('ABS'), ('ESP'), ('Leather seats'), ('Heated seats'), ('Air-conditioner')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Features"); 
        }
    }
}
