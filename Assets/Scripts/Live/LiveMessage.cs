using System;
public class LiveMessage : MessageBase
{
    public string Message { get; set; }
    public LiveMessage(string text)
    {
        var split = text.Split('\0');
        try
        {
            this.UID = int.Parse(split[0]);
            this.Name = split[1];
            this.Message = split[2];
        }
        catch (Exception ex)
        {
            throw new Exception($"消息解析失败: {ex.Message}");
        }
    }
}
