using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClientBasicCrud.Application.Migrations
{
    public partial class InitializeDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "Date", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_client", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "client",
                columns: new[] { "Id", "Birthdate", "Name", "RegistrationDate" },
                values: new object[] { 1, new DateTime(1994, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Victor Soares", new DateTime(2019, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "client",
                columns: new[] { "Id", "Birthdate", "Name", "RegistrationDate" },
                values: new object[] { 2, new DateTime(1980, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maria Camargo", new DateTime(2019, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "client");
        }
    }
}
