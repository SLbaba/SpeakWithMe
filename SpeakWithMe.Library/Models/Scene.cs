namespace SpeakWithMe.Library.Models;

/// <summary>
/// AI畅聊-情景
/// </summary>
public class Scene {
    /// <summary>
    /// 情景名称,固定情景非空,用户自定义情景为空
    /// </summary>
    public string? SceneName { get; set; }

    /// <summary>
    /// 我的角色
    /// </summary>
    public required string MyRole { get; set; }

    /// <summary>
    /// AI的角色
    /// </summary>
    public required string AIRole { get; set; }

    /// <summary>
    /// 场景描述
    /// </summary>

    public required string SceneDescription { get; set; }

    /// <summary>
    /// 剧本图片,固定情景非空,用户自定义情景为空
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// 该情景构成的要发送给ChatGPT的系统消息
    /// </summary>
    public string SceneSystemMessage => $"I am {MyRole} and you are {AIRole}. Scenario description: {SceneDescription} Please guide me from your character's point of view. Please don't use extremely long sentences.";
}