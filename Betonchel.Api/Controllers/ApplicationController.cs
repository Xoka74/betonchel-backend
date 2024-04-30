using System.Net.Http.Headers;
using System.Text;
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

    public ApplicationController(ApplicationRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    [Route("{id:int?}")]
    public async Task<IActionResult> GetBy(int? id, [FromQuery] ApplicationStatus? status, [FromQuery] DateTime? date)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await CheckAuthenticationByAccessToken(accessToken))
            return Unauthorized();
        
        if (id is not null && status is null && date is null)
        {
            var application = repository.GetBy(id.Value);
            return application is null ? NotFound() : Ok(application);
        }

        var applications = repository.GetAll(
            new ApplicationDateFilter(date),
            new ApplicationStatusFilter(status)
        );

        return Ok(applications);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] UserApplication userApplication)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await CheckAuthenticationByAccessToken(accessToken))
            return Unauthorized();
        
        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Create(userApplication.ToApplication());

        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }

    [HttpPut]
    [Route("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UserApplication userApplication)
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await CheckAuthenticationByAccessToken(accessToken))
            return Unauthorized();

        if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

        var status = await repository.Update(userApplication.ToApplication(id));

        return status is ISuccessOperationStatus
            ? Ok(status.Tokenize())
            : BadRequest(status.Tokenize());
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> CheckAuthenticationByAccessToken(string accessToken)
    {
        try
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://localhost:5001/api/Authenticate/check", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}