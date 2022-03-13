using TheWorkBook.Shared.Dto;
using TheWorkBook.Shared.ServiceModels;

namespace TheWorkBook.Backend.Service.Abstraction
{
    public interface IUserService
    {
        Task<UserDto> GetUser(int userId);
        Task RegisterUser(CreateUserRequest createUserRequest);
    }
}
