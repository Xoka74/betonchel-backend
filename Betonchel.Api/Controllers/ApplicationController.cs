using Betonchel.Api.Utils;
using Betonchel.Data.Repositories;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace Betonchel.Api.Controllers;

[Route("api/applications")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly IFilterableRepository<Application, int> repository;
    private readonly string checkUrl;

    public ApplicationController(ApplicationRepository repository, CheckUrl checkUrl)
    {
        this.repository = repository;
        this.checkUrl = checkUrl.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ApplicationStatus? status, [FromQuery] DateTime? date)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        var applications = repository.GetAll(
            new ApplicationDateFilter(date),
            new ApplicationStatusFilter(status)
        );

        return Ok(applications);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();
        
        var application = repository.GetBy(id);
        return application is null ? NotFound() : Ok(application);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserApplication userApplication)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Create(userApplication.ToApplication());

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpPut]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserApplication userApplication)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Update(userApplication.ToApplication(id));

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}