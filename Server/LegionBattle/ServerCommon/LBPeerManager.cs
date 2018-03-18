using System.Collections.Generic;
using LegionBattle.Player;
using LegionBattle.Room;
using LBCSCommon;

namespace LegionBattle.ServerCommon
{
    class LBPeerManager : SingleInstance<LBPeerManager>
    {
        const string LogTag = "PeerManager";
        Dictionary<int, MyPeer> mPeerDic;

        public LBPeerManager()
        {
            mPeerDic = new Dictionary<int, MyPeer>(ServerCommonDefine.PeerManagerDicCapacity);
        }

        /// <summary>
        /// 添加连接
        /// </summary>
        /// <param name="peerConnectionId"></param>
        /// <param name="peer"></param>
        public void AddPeer(int peerConnectionId, MyPeer peer)
        {
            if(mPeerDic.ContainsKey(peerConnectionId))
            {
                LBLogger.Error(LogTag, "连接管理器，插入已经存在的连接 " + peerConnectionId.ToString());
                return;
            }
            mPeerDic[peerConnectionId] = peer;
        }

        /// <summary>
        /// 删除连接
        /// </summary>
        /// <param name="peerConnectionId"></param>
        public void RemovePeer(int peerConnectionId)
        {
            mPeerDic.Remove(peerConnectionId);
            LBPlayerManager.Instance.PlayerDisconnect(peerConnectionId);
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <param name="peerConnectionId"></param>
        /// <returns></returns>
        public MyPeer GetPeer(int peerConnectionId)
        {
            MyPeer ret = null;
            mPeerDic.TryGetValue(peerConnectionId, out ret);
            return ret;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="peerConnectionId"></param>
        public void KickoutPeer(int peerConnectionId)
        {
            MyPeer peer = GetPeer(peerConnectionId);
            if (null != peer)
            {
                peer.Disconnect();
                mPeerDic.Remove(peerConnectionId);
            }
        }

        public void SendMessage(int peerConnectionId, RpId rpId, Dictionary<byte, object> dicParameters)
        {
            MyPeer peer = GetPeer(peerConnectionId);
            if (null != peer)
            {
                peer.SendCustomEvent(rpId, dicParameters);
            }
        }
    }
}
