using Microsoft.EntityFrameworkCore.Migrations;

namespace VEGA.Migrations
{
    public partial class addBrandsModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("insert into Brands(name) select 'Jaguar' where not exists(select name from brands where name = 'Jaguar')");
            migrationBuilder.Sql("insert into Brands(name) select 'Wiesmann' where not exists(select name from brands where name = 'Wiesmann')");
            migrationBuilder.Sql("insert into Brands(name) select 'Volkswagen' where not exists(select name from brands where name = 'Volkswagen')"); 

            migrationBuilder.Sql("insert into models(brandId, name) select B.id, 'F-Type' from brands B left outer join models M on M.brandId = B.id and M.name = 'F-Type' where B.name = 'Jaguar' and M.id is null");
            migrationBuilder.Sql("insert into models(brandId, name) select B.id, 'S-Type' from brands B left outer join models M on M.brandId = B.id and M.name = 'S-Type' where B.name = 'Jaguar' and M.id is null");
            migrationBuilder.Sql("insert into models(brandId, name) select B.id, 'XK' from brands B left outer join models M on M.brandId = B.id and M.name = 'XK' where B.name = 'Jaguar' and M.id is null");
            migrationBuilder.Sql("insert into models(brandId, name) select B.id, 'Roadster' from brands B left outer join models M on M.brandId = B.id and M.name = 'Roadster' where B.name = 'Wiesmann' and M.id is null");
            migrationBuilder.Sql("insert into models(brandId, name) select B.id, 'XF' from brands B left outer join models M on M.brandId = B.id and M.name = 'XF' where B.name = 'Wiesmann' and M.id is null");
            migrationBuilder.Sql("insert into models(brandId, name) select B.id, 'Beetle Cabrio' from brands B left outer join models M on M.brandId = B.id and M.name = 'Beetle Cabrio' where B.name = 'Volkswagen' and M.id is null");
             
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from models where brandId in (select id from Brands where name in ('Jaguar', 'Wiesmann','Volkswagen'))");
            migrationBuilder.Sql("delete from Brands where name in ('Jaguar', 'Wiesmann','Volkswagen')");
        }
    }
}
