using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Helpers;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace Betonchel.Api.Controllers;

[Route("api/[controller]")]
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

        return repository.Create(userConcreteGrade.ToConcreteGrade()) switch
        {
            RepositoryOperationStatus.Success => Ok(),
            _ => StatusCode(500, "The database was unable to save the entity")
        };
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromQuery] int id, [FromBody] UserConcreteGrade userConcreteGrade)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        return repository.Update(userConcreteGrade.ToConcreteGrade(id)) switch
        {
            RepositoryOperationStatus.ForeignKeyViolation => BadRequest("Foreign keys is incorrect"),
            RepositoryOperationStatus.NonExistentEntity => NotFound(),
            RepositoryOperationStatus.Success => Ok(),
            _ => StatusCode(500, "The database was unable to save the entity")
        };
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        return repository.DeleteBy(id) switch
        {
            RepositoryOperationStatus.NonExistentEntity => NotFound(),
            RepositoryOperationStatus.Success => Ok(),
            _ => StatusCode(500, "The database was unable to save the entity")
        };
    }
}