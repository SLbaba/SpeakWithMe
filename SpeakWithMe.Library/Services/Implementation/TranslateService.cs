using Azure;
using Azure.AI.Translation.Text;
using SpeakWithMe.Library.Services.IServices;

namespace SpeakWithMe.Library.Services.Implementation;

/// <summary>
/// 翻译服务
/// </summary>
public class TranslateService(ILogService _logService) : ITranslateService {
    // ******** 公开变量

    // ******** 私有变量

    /// <summary>
    /// 目标语言-简体中文
    /// </summary>
    private const string TargetLanguage = "zh-Hans";

    /// <summary>
    /// 订阅密钥
    /// </summary>
    private const string Key = "ce4790191f94468abe8b785684f999cd";

    /// <summary>
    /// 文本翻译客户端
    /// </summary>
    private TextTranslationClient _client =
        new(new AzureKeyCredential(Key), "japanwest");

    // ******** 继承方法

    /// <summary>
    /// 翻译,从输入文本到中文
    /// </summary>
    /// <param name="inputText">输入文本</param>
    /// <returns>输入文本对应的中文</returns>
    public async Task<string?> TranslateAsync(string inputText) {
        try {
            Response<IReadOnlyList<TranslatedTextItem>> response =
                await _client.TranslateAsync(TargetLanguage, inputText)
                    .ConfigureAwait(false);
            IReadOnlyList<TranslatedTextItem> translations = response.Value;
            TranslatedTextItem translation = translations.FirstOrDefault();

            return translation?.Translations?.FirstOrDefault()?.Text;
        } catch (RequestFailedException e) {
            _logService.WriteLog(nameof(TranslateService), LogType.Error,
                $"{nameof(TranslateService)}.{nameof(TranslateAsync)}",
                $"翻译文本:'{inputText}'失败,Error Code:{e.ErrorCode},Message: {e.Message}");
            return null;
        }
    }

    // ******** 公开方法

    // ******** 私有方法
}