using Microsoft.AspNetCore.Mvc;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Methods;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/status_target_users")]

public class StatusTargetUsersController : ControllerBase
{
    private readonly IStatusTargetRecieversRepository _statusrec;
    private readonly IStatusRepository _status;
    public StatusTargetUsersController(IStatusTargetRecieversRepository statusrec, IStatusRepository status)
    {
        _statusrec = statusrec;
        _status = status;
    }

    [HttpGet]
    public async Task<ActionResult> GetStatusTargetUsers()
    {
        var statusTargetUsers = await _statusrec.GetStatusTargetUsers();
        return Ok(statusTargetUsers);
    }

    [HttpPost]
    public async Task<ActionResult> CreateStatusTargetUsers([FromBody] StatusTargetUsersCreateDto statusrec)
    {
        var status = await _status.GetStatusById(statusrec.StatusId);
        if(status == null)
            return NotFound("status not found");
        var userId = UserUtils.GetUserId(HttpContext);
        if(status.PostedByUserId == userId)
        {
            var toCreate = new StatusTargetUser
            {
                UserId = statusrec.UserId,
                StatusId = statusrec.StatusId,
                IsSeen = statusrec.IsSeen
            };

            var createdStatusTargetUsers = await _statusrec.CreateStatusTargetUsers(toCreate);
            if(createdStatusTargetUsers == null)
                return BadRequest("Status Target Users creation failed");
            
            return Ok(createdStatusTargetUsers);
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPut("{user_id}/{status_id}")]
    public async Task<ActionResult> UpdateStatusTargetUsers([FromBody] StatusTargetUsersUpdateDto statusrec, [FromRoute] long user_id, [FromRoute] long status_id)
    {
        var CurrentStatusTargetUser = await _statusrec.GetStatusTargetUserById(user_id, status_id);
        if(CurrentStatusTargetUser == null)
            return NotFound("Status Target User Not Found");
        var status = await _status.GetStatusById(status_id);
        if(status == null)
            return NotFound("status not found");
        var userId = UserUtils.GetUserId(HttpContext);
        if(status.PostedByUserId == userId)
        {
            var toUpdate = CurrentStatusTargetUser with
            {
                IsSeen = statusrec.IsSeen
            };

            var updatedStatusTargetUser = await _statusrec.UpdateStatusTargetUsers(toUpdate);
            return NoContent();
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpDelete("{user_id}/{status_id}")]

    public async Task<ActionResult> DeleteStatusTargetUser([FromRoute] long user_id, [FromRoute] long status_id)
    {
        var CurrentStatusTargetUser = await _statusrec.GetStatusTargetUserById(user_id, status_id);
        if(CurrentStatusTargetUser == null)
            return NotFound("Status Target User Not Found");

        var status = await _status.GetStatusById(status_id);
        if(status == null)
            return NotFound("status not found");
        var userId = UserUtils.GetUserId(HttpContext);
        if(status.PostedByUserId == userId)
        {

            var deleteStatusTargetUser = await _statusrec.DeleteStatusTargetUsers(CurrentStatusTargetUser.UserId, CurrentStatusTargetUser.StatusId);

            if(deleteStatusTargetUser == false)
                return BadRequest("Status Target User Deletion Failed");
            return NoContent();
        }
        else
        {
            return Unauthorized();
        }
    }
}