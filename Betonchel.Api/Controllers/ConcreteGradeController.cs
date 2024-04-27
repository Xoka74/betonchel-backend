using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace Betonchel.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConcreteGradeController : ControllerBase
{
    private readonly IBaseRepository<ConcreteGrade, int> repository;

    public ConcreteGradeController(IBaseRepository<ConcreteGrade, int> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserConcreteGrade userConcreteGrade)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        return repository.Create(userConcreteGrade.ToConcreteGrade())
            ? Ok()
            : StatusCode(500, "The database was unable to save the entity");
    }

    [HttpPost]
    [Route("get/{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var concreteGrade = repository.GetBy(id);
        return concreteGrade is null ? NotFound() : Ok(concreteGrade.Mark);
    }

    [HttpPost]  
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserConcreteGrade userConcreteGrade)
    {
        return repository.Update(userConcreteGrade.ToConcreteGrade(id))
            ? Ok()
            : NotFound();
    }

    [HttpPost]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return repository.DeleteBy(id)
            ? Ok()
            : NotFound();
    }
}