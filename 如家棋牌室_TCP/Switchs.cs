using System;

namespace 如家棋牌室_TCP
{
    class Switchs
    {
        private static byte[] HexStringToByteArray(string s)
        {
            if (s == string.Empty)
            {
                return null;
            }
            else
            {
                s = s.Replace(" ", "");

                byte[] buffer = new byte[s.Length / 2];

                for (int i = 0; i < s.Length; i += 2)
                {
                    buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
                }

                return buffer;
            }
        }

        private static string ByteArrayToHexString(byte[] bytes)
        {
            string returnStr = string.Empty;

            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");

                    returnStr += " ";
                }
            }

            return returnStr.Trim();
        }

        public static byte[] Return_Byte(int Num, bool Bit)
        {
            string str = string.Empty;

            switch (Num)
            {
                case 1:
                    if (Bit)
                        str = "01 05 05 02 FF 00 2D 36";
                    else
                        str = "01 05 05 02 00 00 6C C6";
                    break;
                case 8:
                    if (Bit)
                        str = "01 05 05 03 FF 00 7C F6";
                    else
                        str = "01 05 05 03 00 00 3D 06";
                    break;
                case 2:
                    if (Bit)
                        str = "01 05 05 04 FF 00 CD 37";
                    else
                        str = "01 05 05 04 00 00 8C C7";
                    break;
                case 7:
                    if (Bit)
                        str = "01 05 05 05 FF 00 9C F7";
                    else
                        str = "01 05 05 05 00 00 DD 07";
                    break;
                case 4:
                    if (Bit)
                        str = "01 05 05 06 FF 00 6C F7";
                    else
                        str = "01 05 05 06 00 00 2D 07";
                    break;
                case 6:
                    if (Bit)
                        str = "01 05 05 07 FF 00 3D 37";
                    else
                        str = "01 05 05 07 00 00 7C C7";
                    break;
                case 3:
                    if (Bit)
                        str = "01 05 05 08 FF 00 0D 34";
                    else
                        str = "01 05 05 08 00 00 4C C4";
                    break;
                case 5:
                    if (Bit)
                        str = "01 05 05 09 FF 00 5C F4";
                    else
                        str = "01 05 05 09 00 00 1D 04";
                    break;
                case 10:
                    if (Bit)
                        str = "02 05 05 02 FF 00 2D 05";
                    else
                        str = "02 05 05 02 00 00 6C F5";
                    break;
                case 11:
                    if (Bit)
                        str = "02 05 05 03 FF 00 7C C5";
                    else
                        str = "02 05 05 03 00 00 3D 35";
                    break;
                case 14:
                    if (Bit)
                        str = "02 05 05 04 FF 00 CD 04";
                    else
                        str = "02 05 05 04 00 00 8C F4";
                    break;
                case 12:
                    if (Bit)
                        str = "02 05 05 05 FF 00 9C C4";
                    else
                        str = "02 05 05 05 00 00 DD 34";
                    break;
                case 16:
                    if (Bit)
                        str = "02 05 05 06 FF 00 6C C4";
                    else
                        str = "02 05 05 06 00 00 2D 34";
                    break;
                case 9:
                    if (Bit)
                        str = "02 05 05 07 FF 00 3D 04";
                    else
                        str = "02 05 05 07 00 00 7C F4";
                    break;
                case 15:
                    if (Bit)
                        str = "02 05 05 08 FF 00 0D 07";
                    else
                        str = "02 05 05 08 00 00 4C F7";
                    break;
                case 13:
                    if (Bit)
                        str = "02 05 05 09 FF 00 5C C7";
                    else
                        str = "02 05 05 09 00 00 1D 37";
                    break;
            }

            return HexStringToByteArray(str);
        }
    }
}
