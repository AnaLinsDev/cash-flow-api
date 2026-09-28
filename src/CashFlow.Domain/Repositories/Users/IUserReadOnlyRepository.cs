using CashFlow.Domain.Entities;
using System.Threading.Tasks;

namespace CashFlow.Domain.Repositories.Users;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);
    Task<User?> GetUserByEmail(string email);
}