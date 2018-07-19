using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BobTheBot.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchKeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Word = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserToReplies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConversationId = table.Column<string>(maxLength: 128, nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    SendEmail = table.Column<bool>(nullable: false),
                    SkypeUserId = table.Column<string>(maxLength: 128, nullable: true),
                    SkypeUserName = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToReplies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipients");

            migrationBuilder.DropTable(
                name: "SearchKeys");

            migrationBuilder.DropTable(
                name: "UserToReplies");
        }
    }
}
