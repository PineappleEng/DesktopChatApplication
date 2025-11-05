using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network
{
    public enum NetworkMessageType
    {
        Login, 
        Signup, 
        Logout,

        ChatMessage, //TODO
        CreateChat,
        AddMember, //TODO
        ListUsers, 
        ListChats,

        Error
    }
}
