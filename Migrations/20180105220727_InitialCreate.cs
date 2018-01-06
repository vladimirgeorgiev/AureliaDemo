using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace aureliadotnetcore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Athlete",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    age = table.Column<int>(nullable: false),
                    athlete = table.Column<string>(nullable: true),
                    bronze = table.Column<int>(nullable: false),
                    country = table.Column<string>(nullable: true),
                    date = table.Column<string>(nullable: true),
                    gold = table.Column<int>(nullable: true),
                    silver = table.Column<int>(nullable: true),
                    sport = table.Column<string>(nullable: true),
                    total = table.Column<int>(nullable: true),
                    year = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Athlete", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ReleaseYear = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Athlete");

            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
