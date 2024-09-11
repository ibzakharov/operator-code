using Microsoft.AspNetCore.Mvc;
using OperatorCode.Api.Dtos;
using OperatorCode.Api.Models;
using OperatorCode.Api.Repositories;

namespace OperatorCode.Api.Controllers;

[Route("operators")]
[ApiController]
public class OperatorController : ControllerBase
{
    private readonly IOperatorRepository _operatorRepository;

    public OperatorController(IOperatorRepository operatorRepository)
    {
        _operatorRepository = operatorRepository;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Operator>> Post([FromBody] ModifyOperatorDto modifyOperatorDto)
    {
        if (await _operatorRepository.ExistsName(modifyOperatorDto.Name))
        {
            ModelState.AddModelError("Name", "Name already exists");
            return Conflict(new ValidationProblemDetails(ModelState));
        }
        
        var operatorDto = await _operatorRepository.Create(modifyOperatorDto);
        return CreatedAtAction(nameof(GetByCode), new { code = operatorDto.Code }, operatorDto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Operator>>> Get()
    {
        var operators = await _operatorRepository.GetAll();
        return Ok(operators);
    }

    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Put(int code, ModifyOperatorDto modifyOperatorDto)
    {
        var operatorDto = await _operatorRepository.GetByCode(code);

        if (operatorDto == null)
        {
            return NotFound();
        }

        if (await _operatorRepository.ExistsName(modifyOperatorDto.Name))
        {
            ModelState.AddModelError("Name", "Name already exists");
            return Conflict(new ValidationProblemDetails(ModelState));
        }
        
        await _operatorRepository.Update(code, modifyOperatorDto.Name);
        return NoContent();
    }

    [HttpDelete("{code}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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