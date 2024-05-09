using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Api.Controllers;

[Route("api/concrete_grades")]
[ApiController]
[Authorize]
public class ConcreteGradeController : ControllerBase
{
    private readonly IFilterableRepository<ConcreteGrade, int> repository;

    public ConcreteGradeController(IFilterableRepository<ConcreteGrade, int> repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var concreteGrades = await repository.GetAll().ToListAsync();
        return Ok(concreteGrades);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var concreteGrade = repository.GetBy(id);
        return concreteGrade is null ? NotFound() : Ok(concreteGrade);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserConcreteGrade userConcreteGrade)
    {
        var status = await repository.Create(userConcreteGrade.ToConcreteGrade());

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpPut]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserConcreteGrade userConcreteGrade)
    {
        var status = await repository.Update(userConcreteGrade.ToConcreteGrade(id));

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = await repository.DeleteBy(id);

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }
}