namespace SpeakWithMe.Library.Models;

/// <summary>
/// 消息
/// </summary>
public class SceneMessage {
    /// <summary>
    /// 聊天记录中的一个消息
    /// </summary>
    public required Message Message { get; set; }       

    /// <summary>
    /// 译文
    /// </summary>
    public string Translation { get; set; } = string.Empty;

    /// <summary>
    /// 完成翻译
    /// </summary>
    public bool HasTranslated { get; set; }

    /// <summary>
    /// 显示译文
    /// </summary>
    public bool ShowTranslation { get; set; }
}