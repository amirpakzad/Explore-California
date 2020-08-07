using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalR_App.Models;

namespace SignalR_App.Services
{
    public interface IChatRoomService
    {
        Task<Guid> CreateRoom(string connectionId);
        Task<Guid> GetRoomForConnectionId(string connectionId);
        Task SetRoomName(Guid roomId, string name);
        Task AddMessage(Guid roomId, ChatMessage message);
        Task<List<ChatMessage>> GetMessageHistory(Guid roomId);
    }
}
