using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabProject.Migrations
{
    public partial class SeedTestClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 100; i++)
            {
                migrationBuilder.InsertData(
                    table: "Classes",
                    columns: new[] { "Name", "PersonCount", "Description", "IsActive" },
                    values: new object[] 
                    { 
                        $"Class {i:000}", 
                        i + 10, 
                        $"This is description for Class {i:000}", 
                        true 
                    });
            }
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}