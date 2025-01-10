using Profile.Api.Models;

namespace Profile.Api.DataAccess.Entity
{
    public class UserProfile
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<DropInfo> History { get; set; } = [];

    }
}
