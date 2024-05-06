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
    private readonly IFilterableRepository<ConcreteGrade, int> repository;
    private readonly string checkUrl;

    public ConcreteGradeController(IFilterableRepository<ConcreteGrade, int> repository, CheckUrl checkUrl)
    {
        this.repository = repository;
        this.checkUrl = checkUrl.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        var concreteGrades = await repository.GetAll().ToListAsync();
        return Ok(concreteGrades);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        var concreteGrade = repository.GetBy(id);
        return concreteGrade is null ? NotFound() : Ok(concreteGrade);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserConcreteGrade userConcreteGrade)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Create(userConcreteGrade.ToConcreteGrade());

        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }

    [HttpPut]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserConcreteGrade userConcreteGrade)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Update(userConcreteGrade.ToConcreteGrade(id));

        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        var status = await repository.DeleteBy(id);

        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }
}