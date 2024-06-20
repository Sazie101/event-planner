using AutoMapper;
using Entities.Models;
using Entities.DTOs;

namespace EventPlanner
{
    public class MyMappingProfile : Profile
    {
        public MyMappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();
            CreateMap<Event, EventAddDTO>();
            CreateMap<EventAddDTO, Event>();
            CreateMap<Event, EventUpdateDTO>();
            CreateMap<EventUpdateDTO, Event>();
        }
    }
}
