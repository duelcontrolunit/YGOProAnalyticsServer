using System.Linq;
using System.Threading.Tasks;
using YGOProAnalyticsServer.DbModels;

namespace YGOProAnalyticsServer.Services.Others.Interfaces
{
    public interface IDecklistService
    {
        Task<Decklist> GetByIdWithAllDataIncluded(int id);
    }
}