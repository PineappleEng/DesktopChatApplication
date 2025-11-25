namespace Common.Network
{
    public enum NetworkMessageType
    {
        Login,
        LoginResponse,
        Signup,
        SignupResponse,
        Logout,
        LogoutResponse,

        ChatMessage,
        ChatMessageResponse,
        CreateChat,
        CreateChatResponse,
        AddMember,
        AddMemberResponse,
        GetUsers, 
        GetUsersResponse,
        GetChats,
        GetChatsResponse,
        GetMessages,
        GetMessagesResponse,

        Error
    }
}
