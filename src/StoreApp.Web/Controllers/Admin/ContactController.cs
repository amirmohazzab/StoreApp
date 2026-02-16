using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Dtos.Admin.AdminContactMessage;
using StoreApp.Application.Features.Admin.AdminContactMessage.Command;
using StoreApp.Application.Features.Admin.AdminContactMessage.Query;
using StoreApp.Application.Features.ContactMessageFeature.Command;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Enums;
using System.Threading;

namespace StoreApp.Web.Controllers.Admin
{
    public class ContactController : AdminApiBaseController
    {
        [HttpGet("contact-messages")]
        public async Task<IActionResult> GetContactMessages([FromQuery] ContactMessageFilterDto filter)
        {
            var result = await Mediator.Send(new GetContactMessagesQuery(filter));
            return Ok(result);
        }

        [HttpPut("contact-messages/{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromQuery] bool isRead)
        {
            return Ok(await Mediator.Send(new ChangeContactMessageStatusCommand(id, isRead)));
        }

        [HttpGet("admin-unread-message-count")]
        public async Task<IActionResult> GetAdminUnreadCount()
        {
            return Ok(await Mediator.Send(new GetAdminCountUnreadMessageQuery()));
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            await Mediator.Send(new ChangeContactMessageStatusCommand(id, true));
            return Ok();
        }

        [HttpPut("{id}/unread")]
        public async Task<IActionResult> MarkUnread(int id)
        {
            await Mediator.Send(new ChangeContactMessageStatusCommand(id, false));
            return Ok();
        }

        [HttpPost("conversations/{id}/reply")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Reply(int id, [FromForm] AdminReplyDto dto)
        {
            await Mediator.Send(new AdminReplyContactCommand(id, dto));
            return Ok();
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations([FromQuery] ContactMessageFilterDto filter)
        {
            var result = await Mediator.Send(new GetContactMessagesQuery(filter));
            return Ok(result);
        }

        [HttpGet("conversations/{id}/messages")]
        public async Task<IActionResult> GetConversationMessages(int id)
        {
            var result = await Mediator.Send(new GetAdminConversationMessagesQuery(id));
            return Ok(result);
        }

        [HttpDelete("conversations/{id}")]
        public async Task<IActionResult> DeleteContactMessage(int id)
        {
            await Mediator.Send(new DeleteConversationCommand(id));
            return Ok();
        }
    }
}
