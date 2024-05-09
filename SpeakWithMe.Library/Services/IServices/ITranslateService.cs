namespace SpeakWithMe.Library.Services.IServices;

/// <summary>
/// 翻译服务接口
/// </summary>
public interface ITranslateService {
    /// <summary>
    /// 翻译,从输入文本到中文
    /// </summary>
    /// <param name="inputText">输入文本</param>
    /// <returns>输入文本对应的中文</returns>
    Task<string?> TranslateAsync(string inputText);
}