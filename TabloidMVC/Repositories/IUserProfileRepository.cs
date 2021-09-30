using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        UserProfile GetUserById(int id);
        UserProfile IsActiveUser(int id);
        List<UserProfile> GetAllUsers();
    }
}