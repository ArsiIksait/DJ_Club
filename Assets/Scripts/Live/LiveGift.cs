using System;
/// <summary>
/// 礼物数据
/// </summary>
public class LiveGift : MessageBase
{
    /// <summary>
    /// 礼物名称
    /// </summary>
    public string GiftName { get; set; }
    /// <summary>
    /// 礼物个数
    /// </summary>
    public int GiftNum { get; set; }
    /// <summary>
    /// 礼物单价
    /// </summary>
    public int GiftPrice { get; set; }
    /// <summary>
    /// 礼物货币类型
    /// </summary>
    public string GiftCoinType { get; set; }
    /// <summary>
    /// 时间戳(Unix/Linux)
    /// </summary>
    public int TimeStamp { get; set; }
    /// <summary>
    /// 礼物总价格
    /// </summary>
    public double TotalPrice { get; set; }

    public LiveGift(string text)
    {
        var split = text.Split('\0');
        try
         {
            UID = int.Parse(split[0]);
            Name = split[1];
            GiftName = split[2];
            GiftNum = int.Parse(split[3]);
            GiftPrice = int.Parse(split[4]);
            GiftCoinType = split[5];
            TimeStamp = int.Parse(split[6]);

            if (GiftCoinType == "gold")
            {
                TotalPrice = GiftNum * GiftPrice;
            }
            else if (GiftCoinType == "silver")
            {
                TotalPrice = GiftNum * GiftPrice * 0.02f;
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"礼物解析失败: {ex.Message}");
        }
    }
}
