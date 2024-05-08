using System.IdentityModel.Tokens.Jwt;
using Betonchel.Api.Utils;
using Betonchel.Data.Repositories;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Api.Controllers;

[Route("api/applications")]
[ApiController]
public class ApplicationController : ControllerBase
{
    private readonly IFilterableRepository<Application, int> _applicationRepository;
    private readonly IFilterableRepository<User, int> _userRepository;
    private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    private readonly Authentication _authentication;

    public ApplicationController(
        ApplicationRepository applicationRepository,
        Authentication authentication,
        IFilterableRepository<User, int> userRepository
    )
    {
        _applicationRepository = applicationRepository;
        _authentication = authentication;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ApplicationStatus? status, [FromQuery] DateTime? date)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

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
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

        var application = _applicationRepository.GetBy(id);
        return application is null ? NotFound() : Ok(application);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserApplication userApplication)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

        // TODO: Refactor repeated code
        var decodedToken = _jwtSecurityTokenHandler.ReadToken(accessToken.Replace("Bearer ", "")) as JwtSecurityToken;

        var email = decodedToken?.Claims.First(claim => claim.Type == "email");

        if (email == null)
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Email == email.Value);

        if (user == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await _applicationRepository.Create(userApplication.ToApplication());

        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }

    [HttpPut]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserApplication userApplication)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await _applicationRepository.Update(userApplication.ToApplication(id));

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

        var status = await _applicationRepository.DeleteBy(id);
        return status is SuccessOperationStatus
            ? Ok(status)
            : BadRequest(status);
    }
}