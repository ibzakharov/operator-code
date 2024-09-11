using AutoMapper;
using OperatorCode.Api.Dtos;
using OperatorCode.Api.Models;

namespace OperatorCode.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Operator, ModifyOperatorDto>()
            .ReverseMap();      
        CreateMap<Operator, OperatorDto>()
            .ReverseMap();
    }
}