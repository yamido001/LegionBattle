
namespace LBCSCommon
{
    /// <summary>
    /// 服务器返回的事件类型
    /// </summary>
    public enum RpId : byte
    {
        CreateAccountResult,
        LoginResult,
        PlayerLogin,
        CreateRoom,
        EnterRoom,
        LeaveRoom,
        RoomAccountInfo,
        AllMemberReady,
        EnterScene,
        BattleInstruction,
    }
}
