using Microsoft.EntityFrameworkCore;
using Profile.Api.DataAccess;
using Profile.Api.DataAccess.Entity;
using Profile.Api.Models;

namespace Profile.Api.Services
{
    public class DropInfoService : IDropInfoService
    {
        private readonly AppDbContext _context;

        public DropInfoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(int TotalRecords, IEnumerable<DropInfo> DropInfos)> GetDropInfosAsync(GetDropInfoRequest request)
        {
            if (request.Page < 1 || request.Size < 1)
            {
                throw new ArgumentException("Page and size must be greater than 0.");
            }

            var query = _context.DropInfos.AsQueryable();

            if (!string.IsNullOrEmpty(request.ServiceName))
            {
                query = query.Where(drop => drop.ServiceName == request.ServiceName);
            }

            if (request.UserProfileId.HasValue)
            {
                query = query.Where(drop => drop.UserProfileId == request.UserProfileId.Value);
            }

            if (request.From.HasValue)
            {
                query = query.Where(drop => drop.Moment >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                query = query.Where(drop => drop.Moment <= request.To.Value);
            }


            var totalRecords = await query.CountAsync();
            var dropInfos = await query
                .Skip((int)((request.Page - 1) * request.Size))
                .Take((int)request.Size)
                .ToListAsync();

            return (totalRecords, dropInfos);
        }

        public async Task<DropInfo> AddDropInfoAsync(AddDropInfoRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "DropInfo data is null.");
            }

            if (string.IsNullOrWhiteSpace(request.ServiceName) || request.UserProfileId == 0)
            {
                throw new ArgumentException("ServiceName and UserProfileId are required fields.");
            }

            var dropInfo = new DropInfo
            {
                ServiceName = request.ServiceName,
                UserProfileId = request.UserProfileId,
                Moment = request.Moment
            };

            _context.DropInfos.Add(dropInfo);
            await _context.SaveChangesAsync();

            return dropInfo;
        }
    }
}
