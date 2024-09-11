using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OperatorCode.Api.Dtos;
using OperatorCode.Api.Models;

namespace OperatorCode.Api.Repositories;

public class OperatorRepository : IOperatorRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OperatorRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperatorDto> Create(ModifyOperatorDto modifyOperatorDto)
    {
        var entity = _mapper.Map<Operator>(modifyOperatorDto);
        _context.Operators.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<OperatorDto>(entity);
    }

    public async Task<IEnumerable<OperatorDto>> GetAll()
    {
        var operators = await _context.Operators.ToListAsync();
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<OperatorDto> GetByCode(int code)
    {
        var entity = await _context.Operators.FindAsync(code);
        return _mapper.Map<OperatorDto>(entity);
    }

    public async Task<bool> ExistsName(string name)
    {
        return await _context.Operators.AnyAsync(o => o.Name == name);
    }

    public async Task Update(int code, string name)
    {
        var entity = await _context.Operators.FindAsync(code);
        if (entity == null)
            return;

        if (entity.Name != name)
        {
            entity.Name = name;
            _context.Operators.Update(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(int code)
    {
        var entity = await _context.Operators.FindAsync(code);
        if (entity == null)
        {
            return;
        }
        _context.Operators.Remove(entity);
        await _context.SaveChangesAsync();
    }
}