using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheWorkBook.AspNetCore.IdentityModel;
using TheWorkBook.Backend.Data;
using TheWorkBook.Backend.Model;
using TheWorkBook.Backend.Service.Abstraction;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Shared.ServiceModels;
using TheWorkBook.Utils.Abstraction;

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
            User user = await TheWorkBookContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            UserDto userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task RegisterUser(CreateUserRequest createUserRequest)
        {
            User user = Mapper.Map<User>(createUserRequest);
            TheWorkBookContext.Users.Add(user);
            await TheWorkBookContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int userId, JsonPatchDocument<UserDto> patchDocUserDto)
        {
            JsonPatchDocument<User> patchDocument = Mapper.Map<JsonPatchDocument<User>>(patchDocUserDto);

            // We need to identify what fields in the UserDto object cannot be updated here.
            var uneditablePaths = new List<string> { "/RecordCreatedUtc" };

            if (patchDocument.Operations.Any(operation => uneditablePaths.Contains(operation.path)))
            {
                throw new UnauthorizedAccessException();
            }

            User userToUpdate = TheWorkBookContext.Users.Find(userId);
            patchDocument.ApplyTo(userToUpdate);
            await TheWorkBookContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int userId, UserDto userDto)
        {
            User userToUpdate = TheWorkBookContext.Users.Find(userId);
            Mapper.Map(userDto, userToUpdate);
            await TheWorkBookContext.SaveChangesAsync();
        }
    }
}
