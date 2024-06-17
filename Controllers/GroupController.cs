using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Methods;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/group")]

public class GroupController : ControllerBase
{
    private readonly IGroupRepository _group;
    public GroupController(IGroupRepository group)
    {
        _group = group;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetGroups()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var groups = await _group.GetGroups(userId);
        return Ok(groups);
    }


    [HttpGet("{id}")]
    [Authorize]

    public async Task<ActionResult> GetGroupById([FromRoute] long id)
    {
        var group = await _group.GetGroupById(id);
        var userId = UserUtils.GetUserId(HttpContext);
        if(group == null)
            return NotFound("Group Not Found");
        if(userId != group.CreatedByUserId)
            return Unauthorized();
        return Ok(group);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateGroup([FromBody] GroupsCreateDto group)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var toCreate = new Groups
        {
            Name = group.Name,
            Description = group.Description,
            CreatedByUserId = userId,
            IsPublic = group.IsPublic
        };

        var CreatedGroup = await _group.CreateGroup(toCreate);
        if(CreatedGroup == null)
            return BadRequest("Group creation failed");
        
        return Ok(CreatedGroup);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateGroup([FromBody] GroupsUpdateDto group, [FromRoute] long id)
{
    var userId = UserUtils.GetUserId(HttpContext);
    var currentGroup = await _group.GetGroupById(id);
    
    if (currentGroup == null)
    {
        return NotFound("Group Not Found");
    }

    if (currentGroup.CreatedByUserId != userId)
    {
        return Unauthorized();
    }

    var toUpdate = currentGroup with
    {
        Name = group.Name ?? currentGroup.Name,
        Description = group.Description ?? currentGroup.Description,
        IsPublic = group.IsPublic 
    };

    var updatedGroup = _group.UpdateGroup(toUpdate);
    
    return NoContent();
}


    [HttpDelete("{id}")]
    [Authorize]

    public async Task<ActionResult> DeleteGroup([FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var currentGroup = await _group.GetGroupById(id);
        if(currentGroup == null)
            return NotFound("Group Not Found");

        if (currentGroup.CreatedByUserId != userId)
            return Unauthorized();

        var deleteGroup = await _group.DeleteGroup(currentGroup.Id);

        if(!deleteGroup)
            return BadRequest("Group Deletion Failed");
        return NoContent();
    }
}