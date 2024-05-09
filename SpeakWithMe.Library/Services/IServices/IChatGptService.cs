using System.Collections.Concurrent;
using SpeakWithMe.Library.Models;

namespace SpeakWithMe.Library.Services.IServices;

/// <summary>
/// ChatGPT服务接口
/// </summary>
public interface IChatGptService {
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="chatHistory">聊天记录</param>
    /// <returns>响应内容</returns>
    Task<BlockingCollection<string>> SendMessagesAsync(ChatHistory chatHistory);
}

#region 解析响应用到的类

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Choice {
    public Delta delta { get; set; }
    public int index { get; set; }
    public object finish_reason { get; set; }
}

public class Delta {
    public string content { get; set; } = string.Empty;
}

public class ResponseJsonObject {
    public string id { get; set; }
    public string @object { get; set; }
    public int created { get; set; }
    public string model { get; set; }
    public List<Choice> choices { get; set; }
}

#endregion