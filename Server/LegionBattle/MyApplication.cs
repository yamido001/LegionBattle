using Photon.SocketServer;
using LegionBattle.Message;
using LBCSCommon;
using ExitGames.Concurrency.Fibers;
using LegionBattle.ServerCommon;
using LegionBattle.SceneServer;

namespace LegionBattle
{
    public class MyApplication : ApplicationBase
    {
        IFiber mHeartbeatFiber;
        long mLastTick;

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new MyPeer(initRequest);   
        }

        protected override void Setup()
        {
            LBLogger.InitLogger(this.ApplicationRootPath, this.BinaryPath);
            RegisterPeerHandler();
            RegisterProtocolsSerialize();
            mHeartbeatFiber = new ThreadFiber();
            mHeartbeatFiber.ScheduleOnInterval(UpdateTick, 0, 1);
            mHeartbeatFiber.Start();
            mLastTick = TimeManager.Instance.CurrentTimeMs;
        }

        protected override void TearDown()
        {
            LBLogger.Info("MyApplication", "游戏服务器关闭");
            mHeartbeatFiber.Dispose();
            mHeartbeatFiber = null;
        }

        void UpdateTick()
        {
            long curTickMs = TimeManager.Instance.CurrentTimeMs;
            LBSceneManager.Instance.UpdatTick(curTickMs - mLastTick);
            mLastTick = curTickMs;
        }

        protected void RegisterPeerHandler()
        {
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.Login, RqLoginHandler.OnOperateRequest);
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.CreateAccount, RqCreateAccountHandler.OnOperateRequest);
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.CreateRoom, RqCreateRoomHandler.OnOperateRequest);
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.EnterRoom, RqEnterRoomHandler.OnOperateRequest);
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.LeaveRoom, RqLeaveRoomHandler.OnOperateRequest);
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.RoomReadyPlay, RqRoomReadyPlayHandler.OnOperateRequest);
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.LoadFinish, RqLoadFinishHandler.OnOperateRequest);
            MyPeer.RegisterHandlerFunc(LBCSCommon.RqId.BattleInstruction, RqBattleInstructionHandler.OnOperateRequest);
        }

        protected void RegisterProtocolsSerialize()
        {
            Protocol.TryRegisterCustomType(typeof(RpPlayerData), (byte)ProtocolSerializeType.AccountData, RpPlayerData.Serialize, RpPlayerData.Deserialize);
        }
    }
}
