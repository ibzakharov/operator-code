using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OperatorCode.Api.Dtos;
using OperatorCode.Api.Models;
using OperatorCode.Api.Repositories;

namespace OperatorCode.Api.Controllers;

[Route("operators")]
[ApiController]
public class OperatorController : ControllerBase
{
    private readonly IOperatorRepository _operatorRepository;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OperatorController(IOperatorRepository operatorRepository,
        ApplicationDbContext context, 
        IMapper mapper)
    {
        _operatorRepository = operatorRepository;
        _context = context;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<ActionResult<OperatorDto>> Post(CreateOperatorDto createOperatorDto)
    {
        if (await _operatorRepository.ExistsName(createOperatorDto.Name))
        {
            return Conflict(new { message = "An operator with the same name already exists." });
        }
        var operatorDto = await _operatorRepository.Create(createOperatorDto);
        return CreatedAtAction("GetByCode", new { code = operatorDto.Code }, operatorDto);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> Get()
    {
        var operators = await _operatorRepository.GetAll();
        return operators.ToList();
    }
    
    [HttpGet("{code}")]
    public async Task<ActionResult<OperatorDto>> GetByCode(int code)
    {
        var operatorDto = await _operatorRepository.GetByCode(code);
        
        if (operatorDto == null)
        {
            return NotFound();
        }

        return operatorDto;
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> Put(int code, [FromBody] string name)
    {
        if (await _operatorRepository.ExistsName(name))
        {
            return Conflict(new { message = "An operator with the same name already exists." });
        }
        
        var operatorDto = await _operatorRepository.GetByCode(code);

        if (operatorDto == null)
        {
            return NotFound();
        }

        await _operatorRepository.Update(code, name);
        return NoContent();
    }
    
    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(int code)
    {
        var operatorDto = await _operatorRepository.GetByCode(code);

        if (operatorDto == null)
        {
            return NotFound();
        }

        await _operatorRepository.Delete(code);
        return NoContent();
    }
}