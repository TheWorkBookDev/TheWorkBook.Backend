using AutoMapper;
using Microsoft.Extensions.Logging;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Model;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Utils.Abstraction;
using Microsoft.EntityFrameworkCore;
using TheWorkBook.Shared.ServiceModels;

namespace TheWorkBook.Backend.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IMapper mapper, ILogger<BaseService> logger,
            IApplicationUser applicationUser,
            IEnvVariableHelper envVariableHelper,
            TheWorkBookContext theWorkBookContext
        )
            : base(mapper, logger, applicationUser, envVariableHelper, theWorkBookContext) { }

        public async Task<UserDto> GetUser(int userId)
        {
            User? user = await TheWorkBookContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            UserDto userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task RegisterUser(CreateUserRequest createUserRequest)
        {
            User user = Mapper.Map<User>(createUserRequest);
            TheWorkBookContext.Users.Add(user);
            await TheWorkBookContext.SaveChangesAsync();
        }
    }
}
