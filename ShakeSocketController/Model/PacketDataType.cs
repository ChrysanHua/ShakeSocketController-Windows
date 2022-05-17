namespace ShakeSocketController.Model
{
    /// <summary>
    /// 指示数据报文对象中实际数据的类型
    /// </summary>
    public enum PacketDataType
    {
        None = 0x0,
        Json = 0x1,
        Str = 0x2,
        Int = 0x4,
        Unknown = 0xF,
    }
}