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

        public async Task<UserProfile> AddUserProfileAsync(AddUserProfileRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "UserProfile data is null.");
            }

            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                throw new ArgumentException("Username are required fields.");
            }

            // Проверка, существует ли пользователь с таким же Username
            var existingUser = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
            {
                throw new ArgumentException("User with this username already exists.");
            }

            // Создание сущности UserProfile из запроса
            var userProfile = new UserProfile
            {
                Username = request.Username,
                Email = request.Email,
                History = new List<DropInfo>()
            };

            // Добавление записи в базу данных
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
            userProfile = await GetUserProfileAsync(new GetUserProfileRequest { Username = request.Username });
            return userProfile;
        }

        public async Task<UserProfile> GetUserProfileAsync(GetUserProfileRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request data is null.");
            }

            // Поиск пользователя по Username
            var userProfile = await _context.UserProfiles
                .Include(u => u.History)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (userProfile == null)
            {
                throw new ArgumentException("User not found.");
            }

            return userProfile;
        }

        public async Task<DropInfo> AddDropInfoByUsernameAsync(AddDropInfoByUsernameRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "DropInfo data is null.");
            }

            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.ServiceName))
            {
                throw new ArgumentException("Username and ServiceName are required fields.");
            }

            // Поиск пользователя по Username
            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (userProfile == null)
            {
                userProfile = await AddUserProfileAsync(new AddUserProfileRequest()
                {
                    Email = String.Empty,
                    Username = request.Username,
                });
            }

            // Создание сущности DropInfo из запроса
            var dropInfo = new DropInfo
            {
                ServiceName = request.ServiceName,
                Moment = request.Moment,
                UserProfileId = userProfile.Id
            };

            // Добавление записи в базу данных
            _context.DropInfos.Add(dropInfo);
            await _context.SaveChangesAsync();

            return dropInfo;
        }
    }
}
