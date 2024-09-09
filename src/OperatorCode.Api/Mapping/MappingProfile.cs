using AutoMapper;
using OperatorCode.Api.Dtos;
using OperatorCode.Api.Models;

namespace OperatorCode.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Operator, CreateOperatorDto>()
            .ReverseMap();      
        CreateMap<Operator, OperatorDto>()
            .ReverseMap();
    }
}