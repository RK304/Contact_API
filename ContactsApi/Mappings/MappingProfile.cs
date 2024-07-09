using AutoMapper;
using ContactsApi.DTOs;
using ContactsApi.Models;

namespace ContactsApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactCreateDto, Contact>();
            CreateMap<Contact, ContactDto>();
            CreateMap<ContactDto, Contact>();
        }
    }
}
