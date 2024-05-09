using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using SpeakWithMe.Library.Services.IServices;

namespace SpeakWithMe.Library.Services.Implementation;

/// <summary>
/// 语音服务
/// </summary>
public class SpeechService : ISpeechService {
    // ******** 公开变量

    // ******** 私有变量

    /// <summary>
    /// 声音
    /// </summary>
    private string _voice = DefaultVoice;

    /// <summary>
    /// 默认声音
    /// </summary>
    private const string
        DefaultVoice = "en-US-JennyNeural"; //"zh-CN-XiaoxiaoNeural";

    /// <summary>
    /// 语音配置
    /// </summary>
    private readonly SpeechConfig _speechConfig =
        SpeechConfig.FromSubscription("85f782ecdd074a7186b3c29a89cd3167",
            "japanwest");

    /// <summary>
    /// 声音合成器,负责文本转语音
    /// </summary>
    private readonly SpeechSynthesizer _speechSynthesizer;

    /// <summary>
    /// 声音识别器,负责语音转文本
    /// </summary>
    private readonly SpeechRecognizer _speechRecognizer;

    /// <summary>
    /// 日志服务
    /// </summary>
    private readonly ILogService _logService;

    // ******** 继承方法

    /// <summary>
    /// 声音
    /// </summary>
    public string Voice {
        get => _voice;
        set {
            _speechConfig.SpeechSynthesisVoiceName = value;
            _voice = value;
        }
    }

    /// <summary>
    /// 获取可用的声音
    /// </summary>
    /// <returns>可用的声音列表</returns>
    public async Task<List<string>> GetVoicesAsync() {
        using var speechSynthesizer = new SpeechSynthesizer(_speechConfig);
        return (await speechSynthesizer.GetVoicesAsync()).Voices.Select(v =>
            v.Name.Substring(v.Name.IndexOf('(')).Replace("(", string.Empty)
                .Replace(")", string.Empty).Replace(", ", "-")).ToList();
    }

    /// <summary>
    /// 文本转语音,Text To Speech(TTS)
    /// </summary>
    /// <param name="text">文本</param>
    /// <returns>执行是否成功</returns>
    public async Task<bool> SpeakTextAsync(string text) {
        try {
            var speechSynthesisResult =
                await _speechSynthesizer.SpeakTextAsync(text);
            switch (speechSynthesisResult.Reason) {
                case ResultReason.SynthesizingAudioCompleted:
                    return true;
                case ResultReason.Canceled:
                    var cancellation =
                        SpeechSynthesisCancellationDetails.FromResult(
                            speechSynthesisResult);
                    throw new Exception(
                        $"Reason={cancellation.Reason}{(cancellation.Reason == CancellationReason.Error ? $",ErrorCode={cancellation.ErrorCode},ErrorDetails=[{cancellation.ErrorDetails}]" : string.Empty)}");
                default:
                    throw new Exception("Default Enum Exception!");
            }
        } catch (Exception e) {
            _logService.WriteLog(nameof(SpeechService), LogType.Error,
                $"{nameof(SpeechService)}.{nameof(SpeakTextAsync)}",
                $"执行SpeakTextAsync方法时发生异常,异常信息={e.Message}");
            return false;
        }
    }

    /// <summary>
    /// 识别一次语音
    /// </summary>
    /// <returns>从语音中识别出来的文本内容</returns>
    public async Task<string?> RecognizeOnceAsync() {
        try {
            var speechRecognitionResult =
                await _speechRecognizer.RecognizeOnceAsync();
            switch (speechRecognitionResult.Reason) {
                case ResultReason.RecognizedSpeech:
                    return speechRecognitionResult.Text;
                case ResultReason.NoMatch:
                    throw new Exception(
                        $"NOMATCH: Speech could not be recognized.");
                case ResultReason.Canceled:
                    var cancellation =
                        CancellationDetails.FromResult(speechRecognitionResult);
                    throw new Exception(
                        $"Reason={cancellation.Reason}{(cancellation.Reason == CancellationReason.Error ? $",ErrorCode={cancellation.ErrorCode},ErrorDetails=[{cancellation.ErrorDetails}]" : string.Empty)}");
                default:
                    throw new Exception("Default Enum Exception!");
            }
        } catch (Exception e) {
            _logService.WriteLog(nameof(SpeechService), LogType.Error,
                $"{nameof(SpeechService)}.{nameof(RecognizeOnceAsync)}",
                $"执行RecognizeOnceAsync方法时发生异常,异常信息={e.Message}");
            return null;
        }
    }

    // ******** 公开方法

    /// <summary>
    /// 语音服务
    /// </summary>
    /// <param name="logService">日志服务</param>
    public SpeechService(ILogService logService) {
        _logService = logService;

        //文本转语音
        _speechConfig.SpeechSynthesisVoiceName = Voice;
        _speechSynthesizer = new SpeechSynthesizer(_speechConfig);

        //语音转文本
        _speechConfig.SpeechRecognitionLanguage = "en-US";
        _speechRecognizer = new SpeechRecognizer(_speechConfig,
            AudioConfig.FromDefaultMicrophoneInput());
    }

    // ******** 私有方法
}