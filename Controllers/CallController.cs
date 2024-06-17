using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Methods;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/call")]

public class CallController : ControllerBase
{
    private readonly ICallRepository _call;
    public CallController(ICallRepository call)
    {
        _call = call;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetCalls()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var calls = await _call.GetCalls(userId);
        return Ok(calls);
    }


    [HttpGet("{id}")]
    [Authorize]

    public async Task<ActionResult> GetCallById([FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var call = await _call.GetCallById(id);
        if(call == null)
            return NotFound("Call Not Found");
        if(userId != call.CallerId && userId != call.RecieverId)
            return Unauthorized();
        return Ok(call);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateCall([FromBody] CallCreateDto call)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        
        var toCreate = new Call
        {
            CallerId = userId,
            RecieverId = call.RecieverId
        };

        var CreatedCall = await _call.CreateCall(toCreate);
        if(CreatedCall == null)
            return BadRequest("Call creation failed");
        
        return Ok(CreatedCall);
    }

    [HttpDelete("{id}")]
    [Authorize]

    public async Task<ActionResult> DeleteCall([FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);

        var currentCall = await _call.GetCallById(id);
        if(currentCall == null)
            return NotFound("Call Not Found");

        if(userId != currentCall.CallerId && userId != currentCall.RecieverId)
            return Unauthorized();

        var deleteCall = await _call.DeleteCall(currentCall.Id);

        if(!deleteCall)
            return BadRequest("Call Deletion Failed");
        return NoContent();
    }
}