using OperatorCode.Api.Dtos;

namespace OperatorCode.Api.Repositories;

public interface IOperatorRepository
{
    Task<OperatorDto> Create(CreateOperatorDto createOperatorDto);
    Task<IEnumerable<OperatorDto>> GetAll();
    Task<OperatorDto> GetByCode(int code);
    Task<bool> ExistsName(string name);
    Task Update(int code, string name);
    Task Delete(int code);
}