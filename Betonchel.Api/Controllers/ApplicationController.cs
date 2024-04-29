using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.Helpers;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace Betonchel.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly IFilterableRepository<Application, int> repository;

    public ApplicationController(IFilterableRepository<Application, int> repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> Get([FromQuery] int? id)
    {
        if (id is null) return Ok(repository.GetAll());
        var application = repository.GetBy(id.Value);
        return application is null ? NotFound() : Ok(application);
    }

    [HttpGet]
    [Route("get/filters")]
    public async Task<IActionResult> Get([FromQuery] ApplicationStatus? status, [FromQuery] DateTime? date)
    {
        if (status is null && date is null)
            return Ok(repository.GetAll());

        var filter = new ApplicationStatusFilter(status) && new ApplicationDateFilter(date);
        var application = repository.GetFiltered(filter);
        return Ok(application);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserApplication userApplication)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        return repository.Create(userApplication.ToApplication()) switch
        {
            RepositoryOperationStatus.Success => Ok(),
            RepositoryOperationStatus.ForeignKeyViolation => BadRequest("At least one foreign key does not exist"),
            _ => StatusCode(500, "The database was unable to save the entity")
        };
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> Edit([FromQuery] int id, [FromBody] UserApplication userApplication)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
        
        return repository.Update(userApplication.ToApplication(id)) switch
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
        throw new NotImplementedException();
    }
}