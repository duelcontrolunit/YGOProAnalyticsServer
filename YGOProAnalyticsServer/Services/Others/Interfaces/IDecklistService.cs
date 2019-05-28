using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    public interface IDecklistService
    {
        Task<System.Collections.Generic.IEnumerable<Decklist>> Get(int howManyTake, int howManySkip, int banlistId);
        Task<Decklist> GetByIdWithAllDataIncluded(int id);
    }
}