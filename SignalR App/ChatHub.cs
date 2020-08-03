using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalR_App.Models;

namespace SignalR_App
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string name, string text)
        {
            var message = new ChatMessage
            {
                SenderName = name,
                Text = text,
                SentAt = DateTimeOffset.Now
            };
            //Broadcast to all clients
            await Clients
                .All
                .SendAsync("ReceiveMessage",
                    message.SenderName,
                    message.SentAt,
                    message.Text);
        }
    }
}
