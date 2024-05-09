namespace SpeakWithMe.Library.Services.IServices;

/// <summary>
/// 语音服务接口
/// </summary>
public interface ISpeechService {
    /// <summary>
    /// 声音
    /// </summary>
    string Voice { get; set; }

    /// <summary>
    /// 获取可用的声音
    /// </summary>
    /// <returns>可用的声音列表</returns>
    Task<List<string>> GetVoicesAsync();

    /// <summary>
    /// 文本转语音,Text To Speech(TTS)
    /// </summary>
    /// <param name="text">文本</param>
    /// <returns>执行是否成功</returns>
    Task<bool> SpeakTextAsync(string text);

    /// <summary>
    /// 识别一次语音
    /// </summary>
    /// <returns>从语音中识别出来的文本内容</returns>
    Task<string?> RecognizeOnceAsync();
}