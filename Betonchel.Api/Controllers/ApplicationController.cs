using System.Security.Claims;
using Betonchel.Data.Repositories;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Api.Controllers;

[Route("api/applications")]
[ApiController]
[Authorize]
public class ApplicationController : ControllerBase
{
    private readonly IFilterableRepository<Application, int> _applicationRepository;
    private readonly IFilterableRepository<User, int> _userRepository;

    public ApplicationController(
        ApplicationRepository applicationRepository,
        IFilterableRepository<User, int> userRepository)
    {
        _applicationRepository = applicationRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ApplicationStatus? status, [FromQuery] DateTime? date)
    {
        var applications = _applicationRepository.GetAll(
            new ApplicationDateFilter(date),
            new ApplicationStatusFilter(status)
        );

        return Ok(applications);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application = _applicationRepository.GetBy(id);
        return application is null ? NotFound() : Ok(application);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserApplication userApplication)
    {
        var email = HttpContext.User.FindFirstValue("email");

        if (email == null)
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
        {
            return Unauthorized();
        }

        var status = await _applicationRepository.Create(userApplication.ToApplication());

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpPut]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserApplication userApplication)
    {
        var status = await _applicationRepository.Update(userApplication.ToApplication(id));

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = await _applicationRepository.DeleteBy(id);
        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status); 
    }
}