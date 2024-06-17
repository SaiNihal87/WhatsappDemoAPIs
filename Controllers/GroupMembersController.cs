using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Methods;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/group_member")]
public class GroupMembersController : ControllerBase
{
    private readonly IGroupMembersRepository _groupmem;
    public GroupMembersController(IGroupMembersRepository groupmem)
    {
        _groupmem = groupmem;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetGroupMembers()
    {
        var groupMembers = await _groupmem.GetGroupMembers();
        return Ok(groupMembers);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateGroupMember([FromBody] GroupMembersCreateDto groupmem)
    {

        var userId = UserUtils.GetUserId(HttpContext);
        var isAdmin = await _groupmem.IsUserAdminInGroup(userId, groupmem.GroupId);
        if (!isAdmin)
            return Forbid();

        var toCreate = new GroupMember
        {
            UserId = groupmem.UserId,
            GroupId = groupmem.GroupId,
            IsAdmin = groupmem.IsAdmin
        };

        var createdGroupMembers = await _groupmem.CreateGroupMembers(toCreate);
        if(createdGroupMembers == null)
            return BadRequest("Group Member creation failed");
        
        return Ok(createdGroupMembers);
    }

    [HttpPut("{group_id}/{user_id}")]
    [Authorize]
    public async Task<ActionResult> UpdateGroupMembers([FromBody] GroupMembersUpdateDto groupmem, [FromRoute] long group_id, [FromRoute] long user_id)
    {
        var CurrentGroupMember = await _groupmem.GetGroupMemberById(group_id, user_id);
        if(CurrentGroupMember == null)
            return NotFound("Group Member Not Found");
        var userId = UserUtils.GetUserId(HttpContext);
        var isAdmin = await _groupmem.IsUserAdminInGroup(userId, group_id);
        if (!isAdmin)
            return Forbid();
        var toUpdate = CurrentGroupMember with
        {
            IsAdmin = groupmem.IsAdmin
        };

        var updatedGroupMember = await _groupmem.UpdateGroupMembers(toUpdate);
        if(!updatedGroupMember)
            return BadRequest("Group member updting failed");
        return NoContent();
    }

    [HttpDelete("{group_id}/{user_id}")]
    [Authorize]
    public async Task<ActionResult> DeleteGroupMember([FromRoute] long group_id, [FromRoute] long user_id)
    {
        var CurrentGroupMember = await _groupmem.GetGroupMemberById(group_id, user_id);
        if(CurrentGroupMember == null)
            return NotFound("Member Not Found");

        var userId = UserUtils.GetUserId(HttpContext);
        var isAdmin = await _groupmem.IsUserAdminInGroup(userId, group_id);
        if (!isAdmin)
            return Forbid();
        
        var deleteGroupMember = await _groupmem.DeleteGroupMembers(user_id, group_id);

        if(!deleteGroupMember)
            return BadRequest("Group Deletion Failed");
        return NoContent();
    }
}