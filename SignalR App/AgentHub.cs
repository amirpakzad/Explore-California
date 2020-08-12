using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR_App.Models;
using SignalR_App.Services;

namespace SignalR_App
{
    [Authorize]
    public class AgentHub :Hub
    {
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IChatRoomService _chatRoomService;

        public AgentHub(IChatRoomService chatRoomService,IHubContext<ChatHub> chatHub)
        {
            _chatRoomService = chatRoomService;
            _chatHub = chatHub;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync(
                "ActiveRooms",
                await _chatRoomService.GetAllRooms());
            await base.OnConnectedAsync();
        }
        public async Task SendAgentMessage(Guid roomId, string text)
        {
            var message = new ChatMessage
            {
                SenderName = Context.User.Identity.Name,
                Text = text,
                SentAt = DateTimeOffset.Now
            };
            await _chatRoomService.AddMessage(roomId, message);

            await _chatHub.Clients
                .Group(roomId.ToString())
                .SendAsync("ReceiveMessage",
                    message.SenderName,
                    message.SentAt,
                    message.Text);
        }

        public async Task LoadHistory(Guid roomId)
        {
            var history = await _chatRoomService.GetMessageHistory(roomId);

            await Clients.Caller.SendAsync("ReceiveMessage", history);
        }
    }
}
