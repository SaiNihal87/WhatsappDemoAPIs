using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhatsappDemoAPIs.DTOs;
using WhatsappDemoAPIs.Methods;
using WhatsappDemoAPIs.Models;
using WhatsappDemoAPIs.Repositories;

namespace WhatsappDemoAPIs.Controllers;

[ApiController]
[Route("api/chat")]

public class ChatController : ControllerBase
{
    private readonly IChatRepository _chat;
    public ChatController(IChatRepository chat)
    {
        _chat = chat;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetChat()
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var chat = await _chat.GetChatByUserId(userId);
        return Ok(chat);
    }


    [HttpGet("{id}")]
    [Authorize]

    public async Task<ActionResult> GetChatById([FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var chat = await _chat.GetChatById(id);
        if(chat == null)
            return NotFound("Chat Not Found");
        if(chat.SenderId != userId)
            return Unauthorized();
        return Ok(chat);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateChat([FromBody] ChatCreateDto chat)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var toCreate = new Chat
        {
            SenderId = userId,
            RecieverId = chat.RecieverId,
            Text = chat.Text,
            IsGroupChat = chat.IsGroupChat,
            GroupId = chat.GroupId
        };

        var CreatedChat = await _chat.CreateChat(toCreate);
        if(CreatedChat == null)
            return BadRequest("Chat creation failed");
        
        return Ok(CreatedChat);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateChat([FromBody] ChatUpdateDto chat, [FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var CurrentChat = await _chat.GetChatById(id);
        if(CurrentChat == null)
            return NotFound("Chat Not Found");

        if(CurrentChat.SenderId != userId)
            return Unauthorized();
            
        var toUpdate = CurrentChat with
        {
            Text = chat.Text ?? CurrentChat.Text,
            IsGroupChat = chat.IsGroupChat,
            GroupId = chat.GroupId
        };

        var updatedChat = _chat.UpdateChat(toUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]

    public async Task<ActionResult> DeleteChat([FromRoute] long id)
    {
        var userId = UserUtils.GetUserId(HttpContext);
        var CurrentChat = await _chat.GetChatById(id);
        if(CurrentChat == null)
            return NotFound("Chat Not Found");

        if(CurrentChat.SenderId != userId)
            return Unauthorized();

        var deleteChat = await _chat.DeleteChat(CurrentChat.Id);

        if(!deleteChat)
            return BadRequest("Chat Deletion Failed");
        return NoContent();
    }
}