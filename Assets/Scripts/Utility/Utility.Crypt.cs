using System;

public static partial class Utility
{
    public static class Crypt
    {
        static readonly ulong P = 0xffffffffffffffc5u;
        static readonly ulong G = 5;
        static readonly int SMALL_CHUNK = 256;

        // Constants are the integer part of the sines of integers (in radians) * 2^32.
        static readonly uint[] k = new uint[64] {
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee ,
            0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501 ,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be ,
            0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821 ,
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa ,
            0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8 ,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed ,
            0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a ,
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c ,
            0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70 ,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05 ,
            0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665 ,
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039 ,
            0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1 ,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1 ,
            0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391 };

        // r specifies the per-round shift amounts
        static readonly uint[] r = new uint[] { 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
					                          5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20,
					                          4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
					                          6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21 };

        public static byte[] RandomKey()
        {
            byte[] tmp = new byte[8];
            byte x = 0;
            for (int i = 0; i < 8; ++i) {
                tmp[i] = (byte)(new System.Random().Next() & 0xff);
                x ^= tmp[i];
            }
            if (x == 0) {
                tmp[0] |= 1;
            }
            return tmp;
        }

        public static string Base64Encode(string value)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
        }

        public static string Base64Encode(int value)
        {
            return System.Convert.ToBase64String(System.BitConverter.GetBytes(value));
        }

        public static string Base64Encode(ulong value)
        {
            return System.Convert.ToBase64String(System.BitConverter.GetBytes(value));
        }

        public static string Base64Encode(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes);
        }

        public static byte[] Base64Decode(string code)
        {
            return System.Convert.FromBase64String(code);
        }

        public static ulong DHExchange(byte[] x) 
        {
            if (x.Length != 8) {
                return 0;
            }
            uint[] xx = new uint[2];
            xx[0] = (uint)(x[0] | x[1]<<8 | x[2]<<16 | x[3]<<24);
            xx[1] = (uint)(x[4] | x[5]<<8 | x[6]<<16 | x[7]<<24);

            ulong x64 = (ulong)xx[0] | (ulong)xx[1]<<32;
            if (x64 == 0)
                return 0;

            return powmodp(G, x64);
        }

        private static ulong powmodp(ulong a, ulong b)
        {
            if (a > P)
                a%=P;
            return pow_mod_p(a,b);
        }

        private static ulong pow_mod_p(ulong a, ulong b)
        {
            if (b==1) {
                return a;
            }
            ulong t = pow_mod_p(a, b >> 1);
            t = mul_mod_p(t,t);
            if (b % 2 != 0) {
                t = mul_mod_p(t, a);
            }
            return t;
        }

        private static ulong mul_mod_p(ulong a, ulong b)
        {
            ulong m = 0;
            while(b != 0) {
                if ((b & 1) != 0) {
                    ulong t = P - a;
                    if ( m >= t) {
                        m -= t;
                    } else {
                        m += a;
                    }
                }
                if (a >= P - a) {
                    a = a * 2 - P;
                } else {
                    a = a * 2;
                }
                b>>=1;
            }
            return m;
        }

        public static ulong DHSecret(byte[] a, byte[] b) 
        {
            ulong xx = System.BitConverter.ToUInt64(a, 0);
            ulong yy = System.BitConverter.ToUInt64(b, 0);
            if (xx == 0 || yy == 0)
                return 0;
            return powmodp(xx, yy);
        }

        private static void Hash(string str, byte[] key) 
        {
            uint djb_hash = 5381;
            uint js_hash = 1315423911;

            for (int i = 0; i < str.Length; i++) {
                byte c = (byte)str[i];
                djb_hash += (djb_hash << 5) + c;
                js_hash ^= ((js_hash << 5) + c + (js_hash >> 2));
            }

            key[0] = (byte)(djb_hash & 0xff);
            key[1] = (byte)((djb_hash >> 8) & 0xff);
            key[2] = (byte)((djb_hash >> 16) & 0xff);
            key[3] = (byte)((djb_hash >> 24) & 0xff);

            key[4] = (byte)(js_hash & 0xff);
            key[5] = (byte)((js_hash >> 8) & 0xff);
            key[6] = (byte)((js_hash >> 16) & 0xff);
            key[7] = (byte)((js_hash >> 24) & 0xff);
        }

        public static ulong HashKey(string key)
        {
            byte[] realkey = new byte[8];
            Hash(key, realkey);
            return System.BitConverter.ToUInt64(realkey, 0);
        }

        public static string HexEncode(ulong x)
        {
            return HexEncode(System.BitConverter.GetBytes(x));
        }

        public static string HexEncode(byte[] bytes) 
        {
            string hex = "0123456789abcdef";

            char[] buffer = null;
            if (bytes.Length > SMALL_CHUNK / 2) {
                buffer = new char[bytes.Length * 2];
            }
            else {
                buffer = new char[SMALL_CHUNK];
            }
            for (int i = 0; i < bytes.Length; i++) {
                buffer[i*2] = hex[bytes[i] >> 4];
                buffer[i*2+1] = hex[bytes[i] & 0xf];
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(buffer);
            return sb.ToString();
        }

        public static ulong HMac64(string x, ulong y)
        {
            return HMac64(System.Text.Encoding.UTF8.GetBytes(x), System.BitConverter.GetBytes(y));
        }

        public static ulong HMac64(byte[] x, ulong y)
        {
            return HMac64(x, System.BitConverter.GetBytes(y));
        }

        public static ulong HMac64(ulong x, ulong y)
        {
            return HMac64(System.BitConverter.GetBytes(x), System.BitConverter.GetBytes(y));
        }

        public static ulong HMac64(byte[] xx, byte[] yy) 
        {
            uint[] x = new uint[2];
            uint[] y = new uint[2];
            read64(xx, yy, x, y);
            uint[] result = new uint[2];
            hmac(x,y,result);
            return (ulong)result[0] | (ulong)result[1] << 32;
        }

        private static void read64(byte[] x, byte[] y, uint[] xx, uint[] yy) 
        {
            if (x.Length != 8) {
                return;
            }
            if (y.Length != 8) {
                return;
            }
            xx[0] = (uint)(x[0] | x[1]<<8 | x[2]<<16 | x[3]<<24);
            xx[1] = (uint)(x[4] | x[5]<<8 | x[6]<<16 | x[7]<<24);
            yy[0] = (uint)(y[0] | y[1]<<8 | y[2]<<16 | y[3]<<24);
            yy[1] = (uint)(y[4] | y[5]<<8 | y[6]<<16 | y[7]<<24);
        }

        private static void hmac(uint[] x, uint[] y, uint[] result) 
        {
            uint[] w = new uint[16];
            uint[] r = new uint[4];
            for (int i=0;i<16;i+=4) {
                w[i] = x[1];
                w[i+1] = x[0];
                w[i+2] = y[1];
                w[i+3] = y[0];
            }

            digest_md5(w, r);

            result[0] = r[2]^r[3];
            result[1] = r[0]^r[1];
        }

        private static uint LEFTROTATE(uint x, uint c)
        {
            return (x << (int)c) | (x >> (32 - (int)c));
        }

        private static void digest_md5(uint[] w, uint[] result) 
        {
            uint a, b, c, d, f, g, temp;
        
            a = 0x67452301u;
            b = 0xefcdab89u;
            c = 0x98badcfeu;
            d = 0x10325476u;

            for(uint i = 0; i<64; i++) {
                if (i < 16) {
                    f = (b & c) | ((~b) & d);
                    g = i;
                } else if (i < 32) {
                    f = (d & b) | ((~d) & c);
                    g = (5*i + 1) % 16;
                } else if (i < 48) {
                    f = b ^ c ^ d;
                    g = (3*i + 5) % 16; 
                } else {
                    f = c ^ (b | (~d));
                    g = (7*i) % 16;
                }

                temp = d;
                d = c;
                c = b;
                b = b + LEFTROTATE((a + f + k[i] + w[g]), r[i]);
                a = temp;
            }

            result[0] = a;
            result[1] = b;
            result[2] = c;
            result[3] = d;
        }
    }
}
