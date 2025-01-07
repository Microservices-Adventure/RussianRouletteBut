using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Profile.Api.DataAccess.Entity
{
    public class DropInfo
    {
        public long Id { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTimeOffset Moment { get; set; }
        public long UserProfileId { get; set; }

    }
}
