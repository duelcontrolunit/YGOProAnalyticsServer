using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YGOProAnalyticsServer;
using YGOProAnalyticsServer.Database;
using YGOProAnalyticsServer.DbModels;
using YGOProAnalyticsServer.Services.Others;
using YGOProAnalyticsServer.Services.Others.Interfaces;
using YGOProAnalyticsServerTests.TestingHelpers;

namespace YGOProAnalyticsServerTests.Others
{
    [TestFixture]
    class ArchetypeServiceTests
    {
        [Test]
        public async Task GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache_WeHaveOnePureArchetypeInDb_WeGetOneValidDTO()
        {
            var cacheMock = new Mock<IMemoryCache>();
            var configMock = new Mock<IAdminConfig>();
            using (var db = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>()))
            {
                await db.Database.EnsureCreatedAsync();
                db.Archetypes.Add(new Archetype(Archetype.Default, true));
                await db.SaveChangesAsync();
                var archetypeService = new ArchetypeService(db, cacheMock.Object, configMock.Object);

                var resultDto =  (await archetypeService.GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache(true))
                    .First();

                Assert.IsTrue(resultDto.Name == Archetype.Default);
            }
        }

        [Test]
        public async Task GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache_WeHaveOnePureArchetypeInDbAndTwoNonPure_WeGetOneDto()
        {
            var cacheMock = new Mock<IMemoryCache>();
            var configMock = new Mock<IAdminConfig>();
            using (var db = new YgoProAnalyticsDatabase(SqlInMemoryHelper.SqlLiteOptions<YgoProAnalyticsDatabase>()))
            {
                await db.Database.EnsureCreatedAsync();
                db.Archetypes.Add(new Archetype(Archetype.Default, true));
                db.Archetypes.Add(new Archetype("NotPure", false));
                db.Archetypes.Add(new Archetype("NotPure2", false));
                await db.SaveChangesAsync();
                var archetypeService = new ArchetypeService(db, cacheMock.Object, configMock.Object);

                var numberOfDtos = (await archetypeService.GetPureArchetypeListWithIdsAndNamesAsNoTrackingFromCache(true))
                    .Count();

                Assert.AreEqual(1, numberOfDtos);
            }
        }
    }
}
