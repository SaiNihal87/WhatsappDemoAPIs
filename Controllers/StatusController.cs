using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Methods;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/status")]

public class StatusController : ControllerBase
{
    private readonly IStatusRepository _status;
    public StatusController(IStatusRepository status)
    {
        _status = status;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetStatus()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var status = await _status.GetStatus(userId);
        return Ok(status);
    }


    [HttpGet("{id}")]
    [Authorize]

    public async Task<ActionResult> GetStatusById([FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var status = await _status.GetStatusById(id);
        if(status == null)
            return NotFound("Status Not Found");

        if(status.PostedByUserId != userId)
            return Unauthorized();
        return Ok(status);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateStatus([FromBody] StatusCreateDto status)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var toCreate = new Status
        {
            PostedByUserId = userId,
            MediaUrl = status.MediaUrl,
            Description = status.Description
        };

        var createdStatus = await _status.CreateStatus(toCreate);
        if(createdStatus == null)
            return BadRequest("Status creation failed");
        
        return Ok(createdStatus);
    }

    [HttpDelete("{id}")]
    [Authorize]

    public async Task<ActionResult> DeleteStatus([FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var currentStatus = await _status.GetStatusById(id);
        if(currentStatus == null)
            return NotFound("Status Not Found");

        if(currentStatus.PostedByUserId != userId)
            return Unauthorized();

        var deleteStatus = await _status.DeleteStatus(currentStatus.Id);

        if(!deleteStatus)
            return BadRequest("Status Deletion Failed");
        return NoContent();
    }

}