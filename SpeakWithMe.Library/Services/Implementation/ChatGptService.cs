using SpeakWithMe.Library.Models;
using SpeakWithMe.Library.Services.IServices;
using Newtonsoft.Json;
using System.Collections.Concurrent;
#pragma warning disable CS4014 
#pragma warning disable CA1416

namespace SpeakWithMe.Library.Services.Implementation;

/// <summary>
/// ChatGPT服务
/// </summary>
public class ChatGptService : IChatGptService
{
    // ******** 公开变量

    // ******** 私有变量

    /// <summary>
    /// 服务端点
    /// </summary>
    private const string Endpoint = "http://43.155.166.6:4000/openai-chat";

    // ******** 继承方法

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="chatHistory">聊天记录</param>
    /// <returns>响应内容</returns>
    public async Task<BlockingCollection<string>> SendMessagesAsync(
        ChatHistory chatHistory)
    {
        // 检查是否有用户消息
        if (chatHistory.Messages.All(c => c.Role != ChatHistory.Role.User) && chatHistory.Messages.FirstOrDefault()?.Role!=ChatHistory.Role.System)
            throw new Exception(
                "you should add user message first,and then send it.");

        var json = JsonConvert.SerializeObject(new
        {
            model = "gpt-3.5-turbo",
            messages = chatHistory.Messages,
            stream = true
        });
        using var httpClient = new HttpClient();
        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Post, Endpoint)
                {
                    Content = new StringContent(json)
                }, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            //TODO 未来用Serilog记录日志
            Console.WriteLine(e);
            throw;
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var blockingCollection = new BlockingCollection<string>();
        Task.Run(async () =>
        {
            var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var myJsonResponse = await reader.ReadLineAsync();
                if (myJsonResponse?.Length > 6)
                {
                    myJsonResponse = myJsonResponse.Substring(6);
                    if (myJsonResponse == "[DONE]")
                        break;
                    try
                    {
                        var responseJsonObject =
                            JsonConvert.DeserializeObject<ResponseJsonObject>(
                                myJsonResponse);
                        var answer = responseJsonObject?.choices[0].delta
                            .content;
                        string words = answer!;
                        blockingCollection.Add(words);
                    }
                    catch (Exception e)
                    {
                        var error = e.Message;
                        throw new Exception(error);
                    }
                }
            }

            blockingCollection.CompleteAdding();
        });
        return blockingCollection;
    }

    // ******** 公开方法

    // ******** 私有方法
}