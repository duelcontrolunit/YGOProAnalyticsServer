using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Builders.Inferfaces
{
    public interface ICardBuilder
    {
        CardBuilder AddBasicCardElements(int passCode, string name, string description, string type, string race, string imageUrl, string smallImageUrl, Archetype archetype);
        CardBuilder AddLinkMonsterCardElements(int linkValue, bool topLeftLinkMarker, bool topLinkMarker, bool topRightLinkMarker, bool middleLeftLinkMarker, bool middleRightLinkMarker, bool bottomLeftLinkMarker, bool bottomLinkMarker, bool bottomRightLinkMarker);
        CardBuilder AddMonsterCardElements(string attack,string defence,int levelOrRank,string attribute);
        CardBuilder AddPendulumMonsterCardElements(int scale);
        Card Build();
    }
}