using AutoMapper;
using ToysStore.DataContracts;
using ToysStore.Entities;

namespace ToysStore
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserRequest, UserData>();
        }
    }
}