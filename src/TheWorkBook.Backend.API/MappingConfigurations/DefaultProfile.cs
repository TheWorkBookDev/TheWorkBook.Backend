using AutoMapper;
using TheWorkBook.Backend.Model;
using TheWorkBook.Shared.Dto;
using TheWorkBook.Shared.ServiceModels;

namespace TheWorkBook.Backend.API.MappingConfigurations
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<CreateUserRequest, User>();

            CreateMap<Listing, ListingDto>();
            CreateMap<ListingDto, Listing>();
        }
    }
}
