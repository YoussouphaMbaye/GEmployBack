using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GesEmploy.Migrations
{
    /// <inheritdoc />
    public partial class n01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employers",
                columns: table => new
                {
                    IdEmp = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameEmp = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmailEmp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneEmp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDay = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Occupation = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    CodeEmp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlQrcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employers", x => x.IdEmp);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    IdLog = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    RoleEmp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.IdLog);
                    table.ForeignKey(
                        name: "FK_Logins_Employers_IdLog",
                        column: x => x.IdLog,
                        principalTable: "Employers",
                        principalColumn: "IdEmp",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Employers");
        }
    }
}
