using AutoMapper;
using BankSystem.App.DTO;
using BankSystem.Models;

namespace BankSystem.App;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Client, ClientDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name + " " + src.SecondName + " " + src.ThirdName));

        CreateMap<ClientDto, Client>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName.Split()[0]))
            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.FullName.Split()[1]))
            .ForMember(dest => dest.ThirdName, opt => opt.MapFrom(src => src.FullName.Split()[2]));
        
        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name + " " + src.SecondName + " " + src.ThirdName));

        CreateMap<EmployeeDto, Employee>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName.Split()[0]))
            .ForMember(dest => dest.SecondName, opt => opt.MapFrom(src => src.FullName.Split()[1]))
            .ForMember(dest => dest.ThirdName, opt => opt.MapFrom(src => src.FullName.Split()[2]));
    }
}