using System;


namespace LBCSCommon
{
    public class SerializeUtils
    {
        static byte[] byte4 = new byte[4];
        public static float ReadFloat(byte[] bytes, ref int index)
        {
            float ret;
            if (BitConverter.IsLittleEndian)
            {
                ret = BitConverter.ToSingle(bytes, index);
            }
            else
            {
                byte4[3] = bytes[index + 0];
                byte4[2] = bytes[index + 1];
                byte4[1] = bytes[index + 2];
                byte4[0] = bytes[index + 3];
                ret = BitConverter.ToSingle(byte4, 0);
            }
            index += 4;
            return ret;
        }

        public static void WriteFloat(byte[] bytes, ref int index, float value)
        {
            byte[] floatBytes = BitConverter.GetBytes(value);
            bytes[0] = floatBytes[index + 0];
            bytes[1] = floatBytes[index + 1];
            bytes[2] = floatBytes[index + 2];
            bytes[3] = floatBytes[index + 3];
            index += 4;
        }

        public static short ReadShort(byte[] bytes, ref int index)
        {
            short ret = 0;
            if (BitConverter.IsLittleEndian)
            {
                ret |= (short)(bytes[index] & 0xFF);
                ret |= (short)((bytes[index + 1] << 8) & 0xFF00);
            }
            else
            {
                ret |= (short)(bytes[index + 1] & 0xFF);
                ret |= (short)((bytes[index] << 8) & 0xFF00);
            }
            index += 2;
            return ret;
        }

        public static void WriteShort(byte[] bytes, ref int index, short value)
        {
            if (BitConverter.IsLittleEndian)
            {
                bytes[index] = (byte)(value & 0xFF);
                bytes[index + 1] = (byte)((value >> 8) & 0xFF);
            }
            else
            {
                bytes[index + 1] = (byte)(value & 0xFF);
                bytes[index] = (byte)((value >> 8) & 0xFF);
            }
            index += 2;
        }

        public static byte ReadByte(byte[] bytes, ref int index)
        {
            return bytes[index++];
        }

        public static void WriteByte(byte[] bytes, ref int index, byte value)
        {
            bytes[index++] = value;
        }

        public static int ReadInt(byte[] bytes, ref int index)
        {
            int ret = 0;
            if (BitConverter.IsLittleEndian)
            {
                ret |= (int)(bytes[index] & 0xFF);
                ret |= (int)((bytes[index + 1] << 8) & 0xFF00);
                ret |= (int)((bytes[index + 2] << 16) & 0xFF0000);
                ret |= (int)((bytes[index + 3] << 24) & 0xFF000000);
            }
            else
            {
                ret |= (int)((bytes[index] << 24) & 0xFF000000);
                ret |= (int)((bytes[index + 1] << 16) & 0xFF0000);
                ret |= (int)((bytes[index + 2] << 8 ) & 0xFF00);
                ret |= (int)((bytes[index + 3] << 0 ) & 0xFF);
            }
            index += 4;
            return ret;
        }

        public static void WriteInt(byte[] bytes, ref int index, int value)
        {
            if (BitConverter.IsLittleEndian)
            {
                bytes[index + 0] = (byte)(value & 0xFF);
                bytes[index + 1] = (byte)((value >> 8) & 0xFF);
                bytes[index + 2] = (byte)((value >> 16) & 0xFF);
                bytes[index + 3] = (byte)((value >> 24) & 0xFF);
            }
            else
            {
                bytes[index + 3] = (byte)(value & 0xFF);
                bytes[index + 2] = (byte)((value >> 8) & 0xFF);
                bytes[index + 1] = (byte)((value >> 16) & 0xFF);
                bytes[index + 0] = (byte)((value >> 24) & 0xFF);
            }
            index += 4;
        }

        public static string ReadString(byte[] bytes, ref int index)
        {
            short strLength = ReadShort(bytes, ref index);
            string ret = System.Text.Encoding.UTF8.GetString(bytes, index, strLength);
            index += strLength;
            return ret;
        }

        public static void WriteString(byte[] bytes, ref int index, string value)
        {
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(value);
            WriteShort(bytes, ref index, (short)strBytes.Length);
            Buffer.BlockCopy(strBytes, 0, bytes, index, strBytes.Length);
            index += strBytes.Length;
        }

        public static bool ReadBool(byte[] bytes, ref int index)
        {
            bool ret = false;
            if (bytes[index++] == 1)
                ret = true;
            return ret;
        }

        public static void WriteBool(byte[] bytes, ref int index, bool value)
        {
            byte writeValue = value ? (byte)1 : (byte)0;
            bytes[index++] = writeValue;
        }
    }
}
