using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Builders.Interfaces
{
    public interface IMonsterCardBuilder
    {
        MonsterCardBuilder AddBasicMonsterElements(int passCode, string name, string description, string type, string race, string imageUrl, string smallImageUrl, string attack, string defence, int levelOrRank, Archetype archetype);
        MonsterCardBuilder AddLinkElements(int linkValue, bool topLeftLinkMarker, bool topLinkMarker, bool topRightLinkMarker, bool middleLeftLinkMarker, bool middleRightLinkMarker, bool bottomLeftLinkMarker, bool bottomLinkMarker, bool bottomRightLinkMarker);
        MonsterCardBuilder AddPendulumElements(int scale);
        MonsterCard Build();
    }
}