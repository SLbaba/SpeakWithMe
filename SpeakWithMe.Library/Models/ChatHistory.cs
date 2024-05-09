using Newtonsoft.Json;

namespace SpeakWithMe.Library.Models;

/// <summary>
/// 聊天记录
/// </summary>
public class ChatHistory {
    public ChatHistory() { }
    public ChatHistory(List<Message> messages) => Messages = messages;

    /// <summary>
    /// 聊天记录中所有的消息
    /// </summary>
    public IList<Message> Messages { get; } = new List<Message>();

    /// <summary>
    /// 添加用户消息
    /// </summary>
    /// <param name="message">消息</param>
    public void AddUserMessage(string message) =>
        Messages.Add(new Message { Role = "user", Content = message });

    /// <summary>
    /// 添加助手消息
    /// </summary>
    /// <param name="message">消息</param>
    public void AddAssistant(string message) =>
        Messages.Add(new Message { Role = "assistant", Content = message });

    /// <summary>
    /// 添加系统消息
    /// </summary>
    /// <param name="message">消息</param>
    public void AddSystemMessage(string message) =>
        Messages.Add(new Message { Role = "system", Content = message });

    /// <summary>
    /// 角色
    /// </summary>
    public static class Role {
        public const string User = "user";
        public const string Assistant = "assistant";
        public const string System = "system";
    }
}

/// <summary>
/// 消息
/// </summary>
public class Message {
    [JsonProperty("role")]
    public string Role { get; set; } = string.Empty;

    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;
}