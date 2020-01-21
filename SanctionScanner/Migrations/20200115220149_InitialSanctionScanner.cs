using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SanctionScanner.Migrations
{
    public partial class InitialSanctionScanner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceSanctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceName = table.Column<string>(nullable: true),
                    SourceCode = table.Column<string>(nullable: true),
                    NameFile = table.Column<string>(nullable: true),
                    FormatFile = table.Column<string>(nullable: true),
                    HasFile = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceSanctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sanctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalName = table.Column<string>(nullable: true),
                    EntityType = table.Column<string>(nullable: true),
                    NameType = table.Column<string>(nullable: true),
                    DateofBirth = table.Column<string>(nullable: true),
                    PlaceofBirth = table.Column<string>(nullable: true),
                    Citizenship = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true),
                    ListingInformation = table.Column<string>(nullable: true),
                    Committees = table.Column<string>(nullable: true),
                    ControlDate = table.Column<string>(nullable: true),
                    InsertDate = table.Column<string>(nullable: true),
                    ModifiedDate = table.Column<string>(nullable: true),
                    IsActive = table.Column<byte>(nullable: false),
                    CountryRelated = table.Column<string>(nullable: true),
                    MatchNumber = table.Column<int>(nullable: false),
                    SactionUID = table.Column<Guid>(nullable: false),
                    SourceSaction_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sanctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sanctions_SourceSanctions_SourceSaction_Id",
                        column: x => x.SourceSaction_Id,
                        principalTable: "SourceSanctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sanctions_SourceSaction_Id",
                table: "Sanctions",
                column: "SourceSaction_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sanctions");

            migrationBuilder.DropTable(
                name: "SourceSanctions");
        }
    }
}
