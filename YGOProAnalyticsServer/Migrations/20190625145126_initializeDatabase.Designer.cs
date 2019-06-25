﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YGOProAnalyticsServer.Database;

namespace YGOProAnalyticsServer.Migrations
{
    [DbContext(typeof(YgoProAnalyticsDatabase))]
    [Migration("20190625145126_initializeDatabase")]
    partial class initializeDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.AnalysisMetadata", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastDecklistsPackDate");

                    b.Property<DateTime>("LastDuelLogDateAnalyzed");

                    b.HasKey("Id");

                    b.ToTable("AnalysisMetadata");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.Archetype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsPureArchetype");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Archetypes");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.ArchetypeStatistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArchetypeId");

                    b.Property<DateTime>("DateWhenArchetypeWasUsed");

                    b.Property<int>("NumberOfDecksWhereWasUsed");

                    b.Property<int>("NumberOfTimesWhenArchetypeWon");

                    b.HasKey("Id");

                    b.HasIndex("ArchetypeId");

                    b.ToTable("ArchetypeStatistics");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.Banlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BanlistNumberInLfList");

                    b.Property<string>("Name");

                    b.Property<DateTime>("ReleaseDate");

                    b.HasKey("Id");

                    b.ToTable("Banlists");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.BanlistStatistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BanlistId");

                    b.Property<DateTime>("DateWhenBanlistWasUsed");

                    b.Property<int>("HowManyTimesWasUsed");

                    b.HasKey("Id");

                    b.HasIndex("BanlistId");

                    b.ToTable("BanlistStatistics");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArchetypeId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("PassCode");

                    b.Property<string>("Race")
                        .IsRequired();

                    b.Property<string>("SmallImageUrl");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ArchetypeId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.CardInExtraDeckDecklistJoin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CardId");

                    b.Property<int>("DecklistId");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("DecklistId");

                    b.ToTable("CardInExtraDeckDecklistJoin");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.CardInMainDeckDecklistJoin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CardId");

                    b.Property<int>("DecklistId");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("DecklistId");

                    b.ToTable("CardInMainDeckDecklistJoin");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.CardInSideDeckDecklistJoin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CardId");

                    b.Property<int>("DecklistId");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("DecklistId");

                    b.ToTable("CardInSideDeckDecklistJoin");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.DecklistsBanlistsJoin", b =>
                {
                    b.Property<int>("DecklistId");

                    b.Property<int>("BanlistId");

                    b.HasKey("DecklistId", "BanlistId");

                    b.HasIndex("BanlistId");

                    b.ToTable("DecklistsBanlistsJoin");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.Decklist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArchetypeId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("WhenDecklistWasFirstPlayed");

                    b.HasKey("Id");

                    b.HasIndex("ArchetypeId");

                    b.ToTable("Decklists");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DecklistStatistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateWhenDeckWasUsed");

                    b.Property<int>("DecklistId");

                    b.Property<int>("NumberOfTimesWhenDeckWasUsed");

                    b.Property<int>("NumberOfTimesWhenDeckWon");

                    b.HasKey("Id");

                    b.HasIndex("DecklistId");

                    b.ToTable("DecklistStatistics");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.ForbiddenCardBanlistJoin", b =>
                {
                    b.Property<int>("CardId");

                    b.Property<int>("BanlistId");

                    b.HasKey("CardId", "BanlistId");

                    b.HasIndex("BanlistId");

                    b.ToTable("ForbiddenCardBanlistJoin");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.LimitedCardBanlistJoin", b =>
                {
                    b.Property<int>("CardId");

                    b.Property<int>("BanlistId");

                    b.HasKey("CardId", "BanlistId");

                    b.HasIndex("BanlistId");

                    b.ToTable("LimitedCardBanlistJoin");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.LinkMonsterCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BottomLeftLinkMarker");

                    b.Property<bool>("BottomLinkMarker");

                    b.Property<bool>("BottomRightLinkMarker");

                    b.Property<int>("LinkValue");

                    b.Property<bool>("MiddleLeftLinkMarker");

                    b.Property<bool>("MiddleRightLinkMarker");

                    b.Property<int>("MonsterCardId");

                    b.Property<bool>("TopLeftLinkMarker");

                    b.Property<bool>("TopLinkMarker");

                    b.Property<bool>("TopRightLinkMarker");

                    b.HasKey("Id");

                    b.HasIndex("MonsterCardId")
                        .IsUnique();

                    b.ToTable("LinkMonsterCards");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.MonsterCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attack")
                        .IsRequired();

                    b.Property<string>("Attribute")
                        .IsRequired();

                    b.Property<int>("CardId");

                    b.Property<string>("Defence");

                    b.Property<int>("LevelOrRank");

                    b.HasKey("Id");

                    b.HasIndex("CardId")
                        .IsUnique();

                    b.ToTable("MonsterCards");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.PendulumMonsterCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MonsterCardId");

                    b.Property<int>("Scale");

                    b.HasKey("Id");

                    b.HasIndex("MonsterCardId")
                        .IsUnique();

                    b.ToTable("PendulumMonsterCards");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.SemiLimitedCardBanlistJoin", b =>
                {
                    b.Property<int>("CardId");

                    b.Property<int>("BanlistId");

                    b.HasKey("CardId", "BanlistId");

                    b.HasIndex("BanlistId");

                    b.ToTable("SemiLimitedCardBanlistJoin");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.ServerActivityStatistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("FromDate");

                    b.Property<int>("NumberOfGames");

                    b.HasKey("Id");

                    b.ToTable("ServerActivityStatistics");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.ArchetypeStatistics", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Archetype", "Archetype")
                        .WithMany("Statistics")
                        .HasForeignKey("ArchetypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.BanlistStatistics", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Banlist", "Banlist")
                        .WithMany("Statistics")
                        .HasForeignKey("BanlistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.Card", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Archetype", "Archetype")
                        .WithMany("Cards")
                        .HasForeignKey("ArchetypeId");
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.CardInExtraDeckDecklistJoin", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Card", "Card")
                        .WithMany("ExtraDeckJoin")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YGOProAnalyticsServer.DbModels.Decklist", "Decklist")
                        .WithMany("CardsInExtraDeckJoin")
                        .HasForeignKey("DecklistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.CardInMainDeckDecklistJoin", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Card", "Card")
                        .WithMany("MainDeckJoin")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YGOProAnalyticsServer.DbModels.Decklist", "Decklist")
                        .WithMany("CardsInMainDeckJoin")
                        .HasForeignKey("DecklistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.CardInSideDeckDecklistJoin", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Card", "Card")
                        .WithMany("SideDeckJoin")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YGOProAnalyticsServer.DbModels.Decklist", "Decklist")
                        .WithMany("CardsInSideDeckJoin")
                        .HasForeignKey("DecklistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DbJoinModels.DecklistsBanlistsJoin", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Banlist", "Banlist")
                        .WithMany("AllowedDecklistsBanlistsJoin")
                        .HasForeignKey("BanlistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YGOProAnalyticsServer.DbModels.Decklist", "Decklist")
                        .WithMany("DecklistPlayableOnBanlistsJoin")
                        .HasForeignKey("DecklistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.Decklist", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Archetype", "Archetype")
                        .WithMany("Decklists")
                        .HasForeignKey("ArchetypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.DecklistStatistics", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Decklist", "Decklist")
                        .WithMany("DecklistStatistics")
                        .HasForeignKey("DecklistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.ForbiddenCardBanlistJoin", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Banlist", "Banlist")
                        .WithMany("ForbiddenCardsJoin")
                        .HasForeignKey("BanlistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YGOProAnalyticsServer.DbModels.Card", "Card")
                        .WithMany("ForbiddenCardsJoin")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.LimitedCardBanlistJoin", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Banlist", "Banlist")
                        .WithMany("LimitedCardsJoin")
                        .HasForeignKey("BanlistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YGOProAnalyticsServer.DbModels.Card", "Card")
                        .WithMany("LimitedCardsJoin")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.LinkMonsterCard", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.MonsterCard", "MonsterCard")
                        .WithOne("LinkMonsterCard")
                        .HasForeignKey("YGOProAnalyticsServer.DbModels.LinkMonsterCard", "MonsterCardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.MonsterCard", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Card", "Card")
                        .WithOne("MonsterCard")
                        .HasForeignKey("YGOProAnalyticsServer.DbModels.MonsterCard", "CardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.PendulumMonsterCard", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.MonsterCard", "MonsterCard")
                        .WithOne("PendulumMonsterCard")
                        .HasForeignKey("YGOProAnalyticsServer.DbModels.PendulumMonsterCard", "MonsterCardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("YGOProAnalyticsServer.DbModels.SemiLimitedCardBanlistJoin", b =>
                {
                    b.HasOne("YGOProAnalyticsServer.DbModels.Banlist", "Banlist")
                        .WithMany("SemiLimitedCardsJoin")
                        .HasForeignKey("BanlistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("YGOProAnalyticsServer.DbModels.Card", "Card")
                        .WithMany("SemiLimitedCardsJoin")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
