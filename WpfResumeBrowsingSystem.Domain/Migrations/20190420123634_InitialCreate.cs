using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WpfResumeBrowsingSystem.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ResumeBrowingSystemV00");

            migrationBuilder.CreateTable(
                name: "Staffs",
                schema: "ResumeBrowingSystemV00",
                columns: table => new
                {
                    Sid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Sex = table.Column<string>(type: "char(1)", nullable: true),
                    Age = table.Column<int>(type: "int(11)", nullable: true),
                    Post = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    School = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Tel = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Birthday = table.Column<DateTime>(type: "date", nullable: true),
                    Education = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Specialty = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Address = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Image = table.Column<short>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffs", x => x.Sid);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                schema: "ResumeBrowingSystemV00",
                columns: table => new
                {
                    Eid = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Start = table.Column<DateTime>(type: "date", nullable: true),
                    End = table.Column<DateTime>(type: "date", nullable: true),
                    Location = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Staff_Sid = table.Column<int>(type: "int(11)", nullable: true),
                    CompanyName = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Eid);
                    table.ForeignKey(
                        name: "FK_Experiences_Staffs_Staff_Sid",
                        column: x => x.Staff_Sid,
                        principalSchema: "ResumeBrowingSystemV00",
                        principalTable: "Staffs",
                        principalColumn: "Sid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Sid",
                schema: "ResumeBrowingSystemV00",
                table: "Experiences",
                column: "Staff_Sid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiences",
                schema: "ResumeBrowingSystemV00");

            migrationBuilder.DropTable(
                name: "Staffs",
                schema: "ResumeBrowingSystemV00");
        }
    }
}
