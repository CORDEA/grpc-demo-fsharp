using System;

namespace Grpcdemo
{
    public class GrpcDemoUtils
    {
        private static Random random = new Random();

        public static string getNonce()
        {
            string s = "";
            for (int i = 0; i < 5; i++)
            {
                s += (char)('a' + random.Next(0, 26));
            }
            return s;
        }
    }
}
