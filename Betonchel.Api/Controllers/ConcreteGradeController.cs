using Betonchel.Api.Utils;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Api.Controllers;

[Route("api/concrete_grades")]
[ApiController]
public class ConcreteGradeController : ControllerBase
{
    private readonly IFilterableRepository<ConcreteGrade, int> _repository;
    private readonly Authentication _authentication;

    public ConcreteGradeController(IFilterableRepository<ConcreteGrade, int> repository, Authentication authentication)
    {
        this._repository = repository;
        _authentication = authentication;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

        var concreteGrades = await _repository.GetAll().ToListAsync();
        return Ok(concreteGrades);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

        var concreteGrade = _repository.GetBy(id);
        return concreteGrade is null ? NotFound() : Ok(concreteGrade);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserConcreteGrade userConcreteGrade)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

        var status = await _repository.Create(userConcreteGrade.ToConcreteGrade());

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpPut]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserConcreteGrade userConcreteGrade)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();
        
        var status = await _repository.Update(userConcreteGrade.ToConcreteGrade(id));

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

        var status = await _repository.DeleteBy(id);

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }
}