using Profile.Api.DataAccess.Entity;
using Profile.Api.Models;

namespace Profile.Api.Services
{
    public interface IDropInfoService
    {
        Task<(int TotalRecords, IEnumerable<DropInfo> DropInfos)> GetDropInfosAsync(GetDropInfoRequest request);
        Task<DropInfo> AddDropInfoAsync(AddDropInfoRequest request);
    }
}
