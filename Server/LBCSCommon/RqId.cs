

namespace LBCSCommon
{
    /// <summary>
    /// 客户端请求的事件类型
    /// </summary>
    public enum RqId : byte
    {
        Login,
        CreateAccount,
        CreateRoom,
        EnterRoom,
        LeaveRoom,
        RoomReadyPlay,
        LoadFinish,
        BattleInstruction,
    }
}
