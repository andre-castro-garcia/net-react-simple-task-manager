using OpenAI.Chat;

namespace SimpleTaskManagerProject.Infrastructure.Services;

public class OpenAIChatService(ChatClient client) : IChatService
{
    public async Task<string> SummarizeAsync(string instructions, CancellationToken cancellationToken)
    {
        var completion = await client.CompleteChatAsync(
            [ new UserChatMessage(instructions) ],
            cancellationToken: cancellationToken);
        return completion.Value.Content[0].Text;
    }
}
