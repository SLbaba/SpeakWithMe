namespace SpeakWithMe.Library.Models;

/// <summary>
/// AI畅聊-角色扮演的角色与剧本
/// </summary>
public class CharacterScript {
    /// <summary>
    /// 剧本名称
    /// </summary>
    public string ScriptName { get; set; } = "剧本名称";

    /// <summary>
    /// 剧本图片
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// 自己的姓名
    /// </summary>
    public string OwnName { get; set; } = "所扮演角色的姓名";

    /// <summary>
    /// 剧本中其他人的姓名
    /// </summary>
    public List<string> OtherNames { get; set; } = ["剧本中其他人的姓名"];

    /// <summary>
    /// 谈话内容
    /// </summary>
    public List<OneSentence> Conversation { get; set; } = new();
}

/// <summary>
/// 一句话
/// </summary>
public class OneSentence {
    /// <summary>
    /// 说话者的姓名
    /// </summary>
    public string SpeakerName { get; set; } = "说话者的姓名";

    /// <summary>
    /// 说话内容
    /// </summary>
    public string Content { get; set; } = "说话内容";
}