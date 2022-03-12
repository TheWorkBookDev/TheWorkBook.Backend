using AutoMapper;
using TheWorkBook.Backend.Model;
using TheWorkBook.Shared.Dto;

namespace TheWorkBook.Backend.API.MappingConfigurations
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Listing, ListingDto>();
            CreateMap<ListingDto, Listing>();
        }
    }
}
