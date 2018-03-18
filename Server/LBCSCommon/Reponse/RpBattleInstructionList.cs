
using System.Collections.Generic;

namespace LBCSCommon
{
    /// <summary>
    /// 服务器返回每一帧的所有指令
    /// 注意，一个消息并不一定能返回全部指令
    /// </summary>
    public class RpBattleInstructionList : ResponseBase
    {
        public const int MaxInstructionCount = 100;
        public int FrameCount
        {
            get;
            private set;
        }

        public List<BattleInstructionBase> BattleInstructionList
        {
            get;
            private set;
        }

        public bool IsFrameFinish
        {
            get;
            private set;
        }

        public RpBattleInstructionList(int frameCount, List<BattleInstructionBase> battleInstructionList, bool isFrameFinish)
        {
            FrameCount = frameCount;
            BattleInstructionList = battleInstructionList;
            IsFrameFinish = isFrameFinish;
        }

        public static Dictionary<byte, object> Serialization(int frameCount, List<BattleInstructionBase> battleInstructionList, bool isFrameFinish)
        {
            int totalByteLength = 4 + 2 + 1;
            for(int i = 0; i < battleInstructionList.Count; ++i)
            {
                totalByteLength += battleInstructionList[i].GetSerializationByteLength();
            }
            byte[] byteArray = new byte[totalByteLength];
            int index = 0;
            SerializeUtils.WriteInt(byteArray, ref index, frameCount);
            SerializeUtils.WriteShort(byteArray, ref index, (short)battleInstructionList.Count);
            SerializeUtils.WriteBool(byteArray, ref index, isFrameFinish);
            for(int i = 0; i < battleInstructionList.Count; ++i)
            {
                battleInstructionList[i].Serialization(byteArray, ref index);
            }

            Dictionary<byte, object> retDic = new Dictionary<byte, object>();
            retDic[1] = byteArray;
            return retDic;
        }

        public static RpBattleInstructionList Deserialization(Dictionary<byte, object> parameters)
        {
            byte[] byteArray = parameters[1] as byte[];
            if (byteArray == null || byteArray.Length < 7)
                return null;

            List<BattleInstructionBase> instructionList = new List<BattleInstructionBase>();

            int index = 0;
            int frameCount = SerializeUtils.ReadInt(byteArray, ref index);
            short instructionCount = SerializeUtils.ReadShort(byteArray, ref index);
            bool isFrameFinish = SerializeUtils.ReadBool(byteArray, ref index);

            RpBattleInstructionList retBattleInstruction = new RpBattleInstructionList(frameCount, instructionList, isFrameFinish);

            for (int i = 0; i < instructionCount; ++i)
            {
                BattleInstructionBase instruction = BattleInstructionBase.Deserializetion(byteArray, ref index);
                instructionList.Add(instruction);
            }
            return retBattleInstruction;
        }
    }
}
