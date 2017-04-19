using System;

namespace Grpcdemo
{
    public class GrpcDemoUtils
    {
        private static Random Random { get; } = new Random();

        public static string Nonce
        {
            get
            {
                var s = string.Empty;
                for (var i = 0; i < 5; i++)
                {
                    s += (char) ('a' + Random.Next(0, 26));
                }
                return s;
            }
        }
    }
}