using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YGOProAnalyticsServer.Migrations
{
    public partial class initializeDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalysisMetadata",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastDuelLogDateAnalyzed = table.Column<DateTime>(nullable: false),
                    LastDecklistsPackDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalysisMetadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Archetypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    IsPureArchetype = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archetypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banlists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    BanlistNumberInLfList = table.Column<int>(nullable: false),
                    ReleaseDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerActivityStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromDate = table.Column<DateTime>(nullable: false),
                    NumberOfGames = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerActivityStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArchetypeStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArchetypeId = table.Column<int>(nullable: false),
                    DateWhenArchetypeWasUsed = table.Column<DateTime>(nullable: false),
                    NumberOfDecksWhereWasUsed = table.Column<int>(nullable: false),
                    NumberOfTimesWhenArchetypeWon = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchetypeStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchetypeStatistics_Archetypes_ArchetypeId",
                        column: x => x.ArchetypeId,
                        principalTable: "Archetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PassCode = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ArchetypeId = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Race = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    SmallImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Archetypes_ArchetypeId",
                        column: x => x.ArchetypeId,
                        principalTable: "Archetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Decklists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    ArchetypeId = table.Column<int>(nullable: false),
                    WhenDecklistWasFirstPlayed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decklists_Archetypes_ArchetypeId",
                        column: x => x.ArchetypeId,
                        principalTable: "Archetypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BanlistStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BanlistId = table.Column<int>(nullable: false),
                    DateWhenBanlistWasUsed = table.Column<DateTime>(nullable: false),
                    HowManyTimesWasUsed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanlistStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BanlistStatistics_Banlists_BanlistId",
                        column: x => x.BanlistId,
                        principalTable: "Banlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForbiddenCardBanlistJoin",
                columns: table => new
                {
                    CardId = table.Column<int>(nullable: false),
                    BanlistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForbiddenCardBanlistJoin", x => new { x.CardId, x.BanlistId });
                    table.ForeignKey(
                        name: "FK_ForbiddenCardBanlistJoin_Banlists_BanlistId",
                        column: x => x.BanlistId,
                        principalTable: "Banlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForbiddenCardBanlistJoin_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LimitedCardBanlistJoin",
                columns: table => new
                {
                    CardId = table.Column<int>(nullable: false),
                    BanlistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimitedCardBanlistJoin", x => new { x.CardId, x.BanlistId });
                    table.ForeignKey(
                        name: "FK_LimitedCardBanlistJoin_Banlists_BanlistId",
                        column: x => x.BanlistId,
                        principalTable: "Banlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimitedCardBanlistJoin_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonsterCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Attack = table.Column<string>(nullable: false),
                    Defence = table.Column<string>(nullable: true),
                    LevelOrRank = table.Column<int>(nullable: false),
                    Attribute = table.Column<string>(nullable: false),
                    CardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonsterCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SemiLimitedCardBanlistJoin",
                columns: table => new
                {
                    CardId = table.Column<int>(nullable: false),
                    BanlistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemiLimitedCardBanlistJoin", x => new { x.CardId, x.BanlistId });
                    table.ForeignKey(
                        name: "FK_SemiLimitedCardBanlistJoin_Banlists_BanlistId",
                        column: x => x.BanlistId,
                        principalTable: "Banlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemiLimitedCardBanlistJoin_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInExtraDeckDecklistJoin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(nullable: false),
                    DecklistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInExtraDeckDecklistJoin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInExtraDeckDecklistJoin_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardInExtraDeckDecklistJoin_Decklists_DecklistId",
                        column: x => x.DecklistId,
                        principalTable: "Decklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInMainDeckDecklistJoin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(nullable: false),
                    DecklistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInMainDeckDecklistJoin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInMainDeckDecklistJoin_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardInMainDeckDecklistJoin_Decklists_DecklistId",
                        column: x => x.DecklistId,
                        principalTable: "Decklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardInSideDeckDecklistJoin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(nullable: false),
                    DecklistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardInSideDeckDecklistJoin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardInSideDeckDecklistJoin_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardInSideDeckDecklistJoin_Decklists_DecklistId",
                        column: x => x.DecklistId,
                        principalTable: "Decklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecklistsBanlistsJoin",
                columns: table => new
                {
                    DecklistId = table.Column<int>(nullable: false),
                    BanlistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecklistsBanlistsJoin", x => new { x.DecklistId, x.BanlistId });
                    table.ForeignKey(
                        name: "FK_DecklistsBanlistsJoin_Banlists_BanlistId",
                        column: x => x.BanlistId,
                        principalTable: "Banlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DecklistsBanlistsJoin_Decklists_DecklistId",
                        column: x => x.DecklistId,
                        principalTable: "Decklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecklistStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DecklistId = table.Column<int>(nullable: false),
                    DateWhenDeckWasUsed = table.Column<DateTime>(nullable: false),
                    NumberOfTimesWhenDeckWasUsed = table.Column<int>(nullable: false),
                    NumberOfTimesWhenDeckWon = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecklistStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecklistStatistics_Decklists_DecklistId",
                        column: x => x.DecklistId,
                        principalTable: "Decklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkMonsterCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LinkValue = table.Column<int>(nullable: false),
                    TopLeftLinkMarker = table.Column<bool>(nullable: false),
                    TopLinkMarker = table.Column<bool>(nullable: false),
                    TopRightLinkMarker = table.Column<bool>(nullable: false),
                    MiddleLeftLinkMarker = table.Column<bool>(nullable: false),
                    MiddleRightLinkMarker = table.Column<bool>(nullable: false),
                    BottomLeftLinkMarker = table.Column<bool>(nullable: false),
                    BottomLinkMarker = table.Column<bool>(nullable: false),
                    BottomRightLinkMarker = table.Column<bool>(nullable: false),
                    MonsterCardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkMonsterCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkMonsterCards_MonsterCards_MonsterCardId",
                        column: x => x.MonsterCardId,
                        principalTable: "MonsterCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendulumMonsterCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Scale = table.Column<int>(nullable: false),
                    MonsterCardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendulumMonsterCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendulumMonsterCards_MonsterCards_MonsterCardId",
                        column: x => x.MonsterCardId,
                        principalTable: "MonsterCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchetypeStatistics_ArchetypeId",
                table: "ArchetypeStatistics",
                column: "ArchetypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BanlistStatistics_BanlistId",
                table: "BanlistStatistics",
                column: "BanlistId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInExtraDeckDecklistJoin_CardId",
                table: "CardInExtraDeckDecklistJoin",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInExtraDeckDecklistJoin_DecklistId",
                table: "CardInExtraDeckDecklistJoin",
                column: "DecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInMainDeckDecklistJoin_CardId",
                table: "CardInMainDeckDecklistJoin",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInMainDeckDecklistJoin_DecklistId",
                table: "CardInMainDeckDecklistJoin",
                column: "DecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInSideDeckDecklistJoin_CardId",
                table: "CardInSideDeckDecklistJoin",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardInSideDeckDecklistJoin_DecklistId",
                table: "CardInSideDeckDecklistJoin",
                column: "DecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ArchetypeId",
                table: "Cards",
                column: "ArchetypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Decklists_ArchetypeId",
                table: "Decklists",
                column: "ArchetypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DecklistsBanlistsJoin_BanlistId",
                table: "DecklistsBanlistsJoin",
                column: "BanlistId");

            migrationBuilder.CreateIndex(
                name: "IX_DecklistStatistics_DecklistId",
                table: "DecklistStatistics",
                column: "DecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_ForbiddenCardBanlistJoin_BanlistId",
                table: "ForbiddenCardBanlistJoin",
                column: "BanlistId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitedCardBanlistJoin_BanlistId",
                table: "LimitedCardBanlistJoin",
                column: "BanlistId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkMonsterCards_MonsterCardId",
                table: "LinkMonsterCards",
                column: "MonsterCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonsterCards_CardId",
                table: "MonsterCards",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendulumMonsterCards_MonsterCardId",
                table: "PendulumMonsterCards",
                column: "MonsterCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SemiLimitedCardBanlistJoin_BanlistId",
                table: "SemiLimitedCardBanlistJoin",
                column: "BanlistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalysisMetadata");

            migrationBuilder.DropTable(
                name: "ArchetypeStatistics");

            migrationBuilder.DropTable(
                name: "BanlistStatistics");

            migrationBuilder.DropTable(
                name: "CardInExtraDeckDecklistJoin");

            migrationBuilder.DropTable(
                name: "CardInMainDeckDecklistJoin");

            migrationBuilder.DropTable(
                name: "CardInSideDeckDecklistJoin");

            migrationBuilder.DropTable(
                name: "DecklistsBanlistsJoin");

            migrationBuilder.DropTable(
                name: "DecklistStatistics");

            migrationBuilder.DropTable(
                name: "ForbiddenCardBanlistJoin");

            migrationBuilder.DropTable(
                name: "LimitedCardBanlistJoin");

            migrationBuilder.DropTable(
                name: "LinkMonsterCards");

            migrationBuilder.DropTable(
                name: "PendulumMonsterCards");

            migrationBuilder.DropTable(
                name: "SemiLimitedCardBanlistJoin");

            migrationBuilder.DropTable(
                name: "ServerActivityStatistics");

            migrationBuilder.DropTable(
                name: "Decklists");

            migrationBuilder.DropTable(
                name: "MonsterCards");

            migrationBuilder.DropTable(
                name: "Banlists");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Archetypes");
        }
    }
}
