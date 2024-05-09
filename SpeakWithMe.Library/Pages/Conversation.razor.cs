using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SpeakWithMe.Library.Models;
using SpeakWithMe.Library.Services.Implementation;

#pragma warning disable CA1416

namespace SpeakWithMe.Library.Pages;

public partial class Conversation {
    [Parameter]
    public required string SceneKey { get; set; }

    private const string Title = "认识新朋友";

    private bool _showVirtualMessage;

    private bool _showVirtualLanguageTips = true;

    private bool _showRecordRegion;

    public ElementReference _recordRegion;

    private readonly List<SceneMessage> _sceneMessages = new();

    protected override async Task OnInitializedAsync() {
        var scene = (Scene)_parcelBoxService.Get(SceneKey);

        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage(scene.SceneSystemMessage);
        _sceneMessages.Add(new() {
            Message = new() {
                Role = ChatHistory.Role.System, Content = scene.SceneDescription
            }
        });
        //StateHasChanged();

        var blockingCollection =
            await _chatGptService.SendMessagesAsync(chatHistory);
        var message = string.Empty;
        while (!blockingCollection.IsCompleted) {
            try {
                message += blockingCollection.Take();
            } catch (Exception e) { }
        }

        chatHistory.AddAssistant(message);
        _sceneMessages.Add(new() {
            Message = new() {
                Role = ChatHistory.Role.Assistant,
                Content = message
            }
        });
        //StateHasChanged();

        _sceneMessages.Last().Translation = await _translateService.TranslateAsync(message) ?? "翻译失败";
        _sceneMessages.Last().HasTranslated = true;
        _sceneMessages.Last().ShowTranslation = true;
        StateHasChanged();

        await _speechService.SpeakTextAsync(message);
        _showRecordRegion = true;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (!firstRender) 
            await _jsRuntime.InvokeVoidAsync("scrollToBottom", _recordRegion);
    }

    private async Task StartRecording() {
        _showRecordRegion = false;
        StateHasChanged();
        var text = await _speechService.RecognizeOnceAsync();
        var chatHistory = new ChatHistory(_sceneMessages.Select(sm => sm.Message).ToList());
        chatHistory.AddUserMessage(text ??= "识别失败");
        if (text == "识别失败") {
            _showRecordRegion = true;
            return;
        }
        _sceneMessages.Add(new() {
            Message = new() {
                Role = ChatHistory.Role.User, Content = text
            }
        });
        StateHasChanged();
        
        var blockingCollection =
            await _chatGptService.SendMessagesAsync(chatHistory);
        var message = string.Empty;
        while (!blockingCollection.IsCompleted) {
            try {
                message += blockingCollection.Take();
            } catch (Exception e) { }
        }

        _sceneMessages.Add(new() {
            Message = new() {
                Role = ChatHistory.Role.Assistant,
                Content = message
            }
        });
        //StateHasChanged();

        _sceneMessages.Last().Translation = await _translateService.TranslateAsync(message) ?? "翻译失败";
        _sceneMessages.Last().HasTranslated = true;
        _sceneMessages.Last().ShowTranslation = true;
        StateHasChanged();

        await _speechService.SpeakTextAsync(message);
        _showRecordRegion = true;
    }

    // private async void Click() {
    //     //测试ChatGPT服务
    //     var chatHistory = new ChatHistory();
    //     chatHistory.AddUserMessage("请你告诉我什么是微服务");
    //     var client = new ChatGptService();
    //     var blockingCollection = await client.SendMessagesAsync(chatHistory);
    //     while (!blockingCollection.IsCompleted) {
    //         _output += blockingCollection.Take();
    //         StateHasChanged();
    //     }
    // }
}