using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace Betonchel.Api.Controllers;

[Route("api/concrete_grades")]
[ApiController]
public class ConcreteGradeController : ControllerBase
{
    private readonly IFilterableRepository<ConcreteGrade, int> repository;

    public ConcreteGradeController(IFilterableRepository<ConcreteGrade, int> repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        var concreteGrade = repository.GetBy(id);
        return concreteGrade is null ? NotFound() : Ok(concreteGrade);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserConcreteGrade userConcreteGrade)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Create(userConcreteGrade.ToConcreteGrade());
        
        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromQuery] int id, [FromBody] UserConcreteGrade userConcreteGrade)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Update(userConcreteGrade.ToConcreteGrade(id));
        
        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var status = await repository.DeleteBy(id);
        
        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }
}