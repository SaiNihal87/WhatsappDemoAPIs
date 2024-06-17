using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Methods;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/user")]

public class UserController : ControllerBase
{
    private readonly IUserRepository _user;
    public UserController(IUserRepository user)
    {
        _user = user;
    }

    [HttpGet]
    [Authorize]

    public async Task<ActionResult> GetCurrentUser()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var CurrentUser = await _user.GetCurrentUser(userId);
        return Ok(CurrentUser);
    }

    [HttpPost]
    [AllowAnonymous]

    public async Task<ActionResult> CreateUser([FromBody] UserCreateDto user)
    {
        var toCreate = new User
        {
            Name = user.Name,
            About = user.About,
            Phone = user.Phone,
            Email = user.Email,
            ProfileUrl = user.ProfileUrl
        };

        var CreatedUser = await _user.CreateUser(toCreate);
        if(CreatedUser == null)
            return BadRequest("User creation failed");
        
        return Ok(CreatedUser);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateUser([FromBody] UserUpdateDto user)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var CurrentUser = await _user.GetCurrentUser(userId);
        if(CurrentUser == null)
            return NotFound("User Not Found");

        if(CurrentUser.Id != userId)
            return Unauthorized();
        var toUpdate = CurrentUser with
        {
            Name = user.Name ?? CurrentUser.Name,
            About = user.About ?? CurrentUser.About,
            Email = user.Email ?? CurrentUser.Email,
            ProfileUrl = user.ProfileUrl ?? CurrentUser.ProfileUrl
        };

        var updatedUser = _user.UpdateUser(toUpdate);
        return NoContent();
    }

    [HttpDelete]

    public async Task<ActionResult> DeleteUser()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var CurrentUser = await _user.GetCurrentUser(userId);
        if(CurrentUser == null)
            return NotFound("User Not Found");
        if(CurrentUser.Id != userId)
            return Unauthorized();

        var deleteUser = await _user.DeleteUser(CurrentUser.Id);

        if(!deleteUser)
            return BadRequest("User Deletion Failed");
        return NoContent();
    }

}