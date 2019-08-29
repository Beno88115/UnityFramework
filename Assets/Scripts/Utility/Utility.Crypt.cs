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
                                            
        static uint[] SB1 = new uint[64] {
            0x01010400, 0x00000000, 0x00010000, 0x01010404,
            0x01010004, 0x00010404, 0x00000004, 0x00010000,
            0x00000400, 0x01010400, 0x01010404, 0x00000400,
            0x01000404, 0x01010004, 0x01000000, 0x00000004,
            0x00000404, 0x01000400, 0x01000400, 0x00010400,
            0x00010400, 0x01010000, 0x01010000, 0x01000404,
            0x00010004, 0x01000004, 0x01000004, 0x00010004,
            0x00000000, 0x00000404, 0x00010404, 0x01000000,
            0x00010000, 0x01010404, 0x00000004, 0x01010000,
            0x01010400, 0x01000000, 0x01000000, 0x00000400,
            0x01010004, 0x00010000, 0x00010400, 0x01000004,
            0x00000400, 0x00000004, 0x01000404, 0x00010404,
            0x01010404, 0x00010004, 0x01010000, 0x01000404,
            0x01000004, 0x00000404, 0x00010404, 0x01010400,
            0x00000404, 0x01000400, 0x01000400, 0x00000000,
            0x00010004, 0x00010400, 0x00000000, 0x01010004
        };

        static uint[] SB2 = new uint[64] {
            0x80108020, 0x80008000, 0x00008000, 0x00108020,
            0x00100000, 0x00000020, 0x80100020, 0x80008020,
            0x80000020, 0x80108020, 0x80108000, 0x80000000,
            0x80008000, 0x00100000, 0x00000020, 0x80100020,
            0x00108000, 0x00100020, 0x80008020, 0x00000000,
            0x80000000, 0x00008000, 0x00108020, 0x80100000,
            0x00100020, 0x80000020, 0x00000000, 0x00108000,
            0x00008020, 0x80108000, 0x80100000, 0x00008020,
            0x00000000, 0x00108020, 0x80100020, 0x00100000,
            0x80008020, 0x80100000, 0x80108000, 0x00008000,
            0x80100000, 0x80008000, 0x00000020, 0x80108020,
            0x00108020, 0x00000020, 0x00008000, 0x80000000,
            0x00008020, 0x80108000, 0x00100000, 0x80000020,
            0x00100020, 0x80008020, 0x80000020, 0x00100020,
            0x00108000, 0x00000000, 0x80008000, 0x00008020,
            0x80000000, 0x80100020, 0x80108020, 0x00108000
        };

        static uint[] SB3 = new uint[64] {
            0x00000208, 0x08020200, 0x00000000, 0x08020008,
            0x08000200, 0x00000000, 0x00020208, 0x08000200,
            0x00020008, 0x08000008, 0x08000008, 0x00020000,
            0x08020208, 0x00020008, 0x08020000, 0x00000208,
            0x08000000, 0x00000008, 0x08020200, 0x00000200,
            0x00020200, 0x08020000, 0x08020008, 0x00020208,
            0x08000208, 0x00020200, 0x00020000, 0x08000208,
            0x00000008, 0x08020208, 0x00000200, 0x08000000,
            0x08020200, 0x08000000, 0x00020008, 0x00000208,
            0x00020000, 0x08020200, 0x08000200, 0x00000000,
            0x00000200, 0x00020008, 0x08020208, 0x08000200,
            0x08000008, 0x00000200, 0x00000000, 0x08020008,
            0x08000208, 0x00020000, 0x08000000, 0x08020208,
            0x00000008, 0x00020208, 0x00020200, 0x08000008,
            0x08020000, 0x08000208, 0x00000208, 0x08020000,
            0x00020208, 0x00000008, 0x08020008, 0x00020200
        };

        static uint[] SB4 = new uint[64] {
            0x00802001, 0x00002081, 0x00002081, 0x00000080,
            0x00802080, 0x00800081, 0x00800001, 0x00002001,
            0x00000000, 0x00802000, 0x00802000, 0x00802081,
            0x00000081, 0x00000000, 0x00800080, 0x00800001,
            0x00000001, 0x00002000, 0x00800000, 0x00802001,
            0x00000080, 0x00800000, 0x00002001, 0x00002080,
            0x00800081, 0x00000001, 0x00002080, 0x00800080,
            0x00002000, 0x00802080, 0x00802081, 0x00000081,
            0x00800080, 0x00800001, 0x00802000, 0x00802081,
            0x00000081, 0x00000000, 0x00000000, 0x00802000,
            0x00002080, 0x00800080, 0x00800081, 0x00000001,
            0x00802001, 0x00002081, 0x00002081, 0x00000080,
            0x00802081, 0x00000081, 0x00000001, 0x00002000,
            0x00800001, 0x00002001, 0x00802080, 0x00800081,
            0x00002001, 0x00002080, 0x00800000, 0x00802001,
            0x00000080, 0x00800000, 0x00002000, 0x00802080
        };

        static uint[] SB5 = new uint[64] {
            0x00000100, 0x02080100, 0x02080000, 0x42000100,
            0x00080000, 0x00000100, 0x40000000, 0x02080000,
            0x40080100, 0x00080000, 0x02000100, 0x40080100,
            0x42000100, 0x42080000, 0x00080100, 0x40000000,
            0x02000000, 0x40080000, 0x40080000, 0x00000000,
            0x40000100, 0x42080100, 0x42080100, 0x02000100,
            0x42080000, 0x40000100, 0x00000000, 0x42000000,
            0x02080100, 0x02000000, 0x42000000, 0x00080100,
            0x00080000, 0x42000100, 0x00000100, 0x02000000,
            0x40000000, 0x02080000, 0x42000100, 0x40080100,
            0x02000100, 0x40000000, 0x42080000, 0x02080100,
            0x40080100, 0x00000100, 0x02000000, 0x42080000,
            0x42080100, 0x00080100, 0x42000000, 0x42080100,
            0x02080000, 0x00000000, 0x40080000, 0x42000000,
            0x00080100, 0x02000100, 0x40000100, 0x00080000,
            0x00000000, 0x40080000, 0x02080100, 0x40000100
        };

        static uint[] SB6 = new uint[64] {
            0x20000010, 0x20400000, 0x00004000, 0x20404010,
            0x20400000, 0x00000010, 0x20404010, 0x00400000,
            0x20004000, 0x00404010, 0x00400000, 0x20000010,
            0x00400010, 0x20004000, 0x20000000, 0x00004010,
            0x00000000, 0x00400010, 0x20004010, 0x00004000,
            0x00404000, 0x20004010, 0x00000010, 0x20400010,
            0x20400010, 0x00000000, 0x00404010, 0x20404000,
            0x00004010, 0x00404000, 0x20404000, 0x20000000,
            0x20004000, 0x00000010, 0x20400010, 0x00404000,
            0x20404010, 0x00400000, 0x00004010, 0x20000010,
            0x00400000, 0x20004000, 0x20000000, 0x00004010,
            0x20000010, 0x20404010, 0x00404000, 0x20400000,
            0x00404010, 0x20404000, 0x00000000, 0x20400010,
            0x00000010, 0x00004000, 0x20400000, 0x00404010,
            0x00004000, 0x00400010, 0x20004010, 0x00000000,
            0x20404000, 0x20000000, 0x00400010, 0x20004010
        };

        static uint[] SB7 = new uint[64] {
            0x00200000, 0x04200002, 0x04000802, 0x00000000,
            0x00000800, 0x04000802, 0x00200802, 0x04200800,
            0x04200802, 0x00200000, 0x00000000, 0x04000002,
            0x00000002, 0x04000000, 0x04200002, 0x00000802,
            0x04000800, 0x00200802, 0x00200002, 0x04000800,
            0x04000002, 0x04200000, 0x04200800, 0x00200002,
            0x04200000, 0x00000800, 0x00000802, 0x04200802,
            0x00200800, 0x00000002, 0x04000000, 0x00200800,
            0x04000000, 0x00200800, 0x00200000, 0x04000802,
            0x04000802, 0x04200002, 0x04200002, 0x00000002,
            0x00200002, 0x04000000, 0x04000800, 0x00200000,
            0x04200800, 0x00000802, 0x00200802, 0x04200800,
            0x00000802, 0x04000002, 0x04200802, 0x04200000,
            0x00200800, 0x00000000, 0x00000002, 0x04200802,
            0x00000000, 0x00200802, 0x04200000, 0x00000800,
            0x04000002, 0x04000800, 0x00000800, 0x00200002
        };
                                        
        static uint[] SB8 = new uint[64] {
            0x10001040, 0x00001000, 0x00040000, 0x10041040,
            0x10000000, 0x10001040, 0x00000040, 0x10000000,
            0x00040040, 0x10040000, 0x10041040, 0x00041000,
            0x10041000, 0x00041040, 0x00001000, 0x00000040,
            0x10040000, 0x10000040, 0x10001000, 0x00001040,
            0x00041000, 0x00040040, 0x10040040, 0x10041000,
            0x00001040, 0x00000000, 0x00000000, 0x10040040,
            0x10000040, 0x10001000, 0x00041040, 0x00040000,
            0x00041040, 0x00040000, 0x10041000, 0x00001000,
            0x00000040, 0x10040040, 0x00001000, 0x00041040,
            0x10001000, 0x00000040, 0x10000040, 0x10040000,
            0x10040040, 0x10000000, 0x00040000, 0x10001040,
            0x00000000, 0x10041040, 0x00040040, 0x10000040,
            0x10040000, 0x10001000, 0x10001040, 0x00000000,
            0x10041040, 0x00041000, 0x00041000, 0x00001040,
            0x00001040, 0x00040040, 0x10000000, 0x10041000
        };

        static uint[] LHs = new uint[16] {
            0x00000000, 0x00000001, 0x00000100, 0x00000101,
            0x00010000, 0x00010001, 0x00010100, 0x00010101,
            0x01000000, 0x01000001, 0x01000100, 0x01000101,
            0x01010000, 0x01010001, 0x01010100, 0x01010101
        };

        static uint[] RHs = new uint[16] {
            0x00000000, 0x01000000, 0x00010000, 0x01010000,
            0x00000100, 0x01000100, 0x00010100, 0x01010100,
            0x00000001, 0x01000001, 0x00010001, 0x01010001,
            0x00000101, 0x01000101, 0x00010101, 0x01010101,
        };

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

        /**
            static int
            ldesencode(lua_State *L) {
                uint32_t SK[32];
                des_key(L, SK);

                size_t textsz = 0;
                const uint8_t * text = (const uint8_t *)luaL_checklstring(L, 2, &textsz);
                size_t chunksz = (textsz + 8) & ~7;
                uint8_t tmp[SMALL_CHUNK];
                uint8_t *buffer = tmp;
                if (chunksz > SMALL_CHUNK) {
                    buffer = lua_newuserdata(L, chunksz);
                }
                int i;
                for (i=0;i<(int)textsz-7;i+=8) {
                    des_crypt(SK, text+i, buffer+i);
                }
                int bytes = textsz - i;
                uint8_t tail[8];
                int j;
                for (j=0;j<8;j++) {
                    if (j < bytes) {
                        tail[j] = text[i+j];
                    } else if (j==bytes) {
                        tail[j] = 0x80;
                    } else {
                        tail[j] = 0;
                    }
                }
                des_crypt(SK, tail, buffer+i);
                lua_pushlstring(L, (const char *)buffer, chunksz);

                return 1;
            }

            static void 
            des_crypt( const uint32_t SK[32], const uint8_t input[8], uint8_t output[8] ) {
                uint32_t X, Y, T;

                GET_UINT32( X, input, 0 );
                GET_UINT32( Y, input, 4 );

                DES_IP( X, Y );

                DES_ROUND( Y, X );  DES_ROUND( X, Y );
                DES_ROUND( Y, X );  DES_ROUND( X, Y );
                DES_ROUND( Y, X );  DES_ROUND( X, Y );
                DES_ROUND( Y, X );  DES_ROUND( X, Y );
                DES_ROUND( Y, X );  DES_ROUND( X, Y );
                DES_ROUND( Y, X );  DES_ROUND( X, Y );
                DES_ROUND( Y, X );  DES_ROUND( X, Y );
                DES_ROUND( Y, X );  DES_ROUND( X, Y );

                DES_FP( Y, X );

                PUT_UINT32( Y, output, 0 );
                PUT_UINT32( X, output, 4 );
            }

            #define GET_UINT32(n,b,i)					   \
            {											   \
                (n) = ( (uint32_t) (b)[(i)	] << 24 )	   \
                    | ( (uint32_t) (b)[(i) + 1] << 16 )	   \
                    | ( (uint32_t) (b)[(i) + 2] <<  8 )	   \
                    | ( (uint32_t) (b)[(i) + 3]	   );	  \
            }

            #define DES_ROUND(X,Y)						  \
            {											   \
                T = *SK++ ^ X;							  \
                Y ^= SB8[ (T	  ) & 0x3F ] ^			  \
                    SB6[ (T >>  8) & 0x3F ] ^			  \
                    SB4[ (T >> 16) & 0x3F ] ^			  \
                    SB2[ (T >> 24) & 0x3F ];			   \
                                                            \
                T = *SK++ ^ ((X << 28) | (X >> 4));		 \
                Y ^= SB7[ (T	  ) & 0x3F ] ^			  \
                    SB5[ (T >>  8) & 0x3F ] ^			  \
                    SB3[ (T >> 16) & 0x3F ] ^			  \
                    SB1[ (T >> 24) & 0x3F ];			   \
            }

            #define DES_FP(X,Y)											 \
            {															   \
                X = ((X << 31) | (X >> 1)) & 0xFFFFFFFF;					\
                T = (X ^ Y) & 0xAAAAAAAA; X ^= T; Y ^= T;				   \
                Y = ((Y << 31) | (Y >> 1)) & 0xFFFFFFFF;					\
                T = ((Y >>  8) ^ X) & 0x00FF00FF; X ^= T; Y ^= (T <<  8);   \
                T = ((Y >>  2) ^ X) & 0x33333333; X ^= T; Y ^= (T <<  2);   \
                T = ((X >> 16) ^ Y) & 0x0000FFFF; Y ^= T; X ^= (T << 16);   \
                T = ((X >>  4) ^ Y) & 0x0F0F0F0F; Y ^= T; X ^= (T <<  4);   \
            }

            #define PUT_UINT32(n,b,i)					   \
            {											   \
                (b)[(i)	] = (uint8_t) ( (n) >> 24 );	   \
                (b)[(i) + 1] = (uint8_t) ( (n) >> 16 );	   \
                (b)[(i) + 2] = (uint8_t) ( (n) >>  8 );	   \
                (b)[(i) + 3] = (uint8_t) ( (n)	   );	   \
            }

            #define DES_IP(X,Y)											 \
            {															   \
                T = ((X >>  4) ^ Y) & 0x0F0F0F0F; Y ^= T; X ^= (T <<  4);   \
                T = ((X >> 16) ^ Y) & 0x0000FFFF; Y ^= T; X ^= (T << 16);   \
                T = ((Y >>  2) ^ X) & 0x33333333; X ^= T; Y ^= (T <<  2);   \
                T = ((Y >>  8) ^ X) & 0x00FF00FF; X ^= T; Y ^= (T <<  8);   \
                Y = ((Y << 1) | (Y >> 31)) & 0xFFFFFFFF;					\
                T = (X ^ Y) & 0xAAAAAAAA; Y ^= T; X ^= T;				   \
                X = ((X << 1) | (X >> 31)) & 0xFFFFFFFF;					\
            }
         */

        private static void PUT_UINT32(uint n, byte[] b, int i)	
        {										
            b[i] = (byte)(n >> 24);	
            b[i + 1] = (byte)(n >> 16);	
            b[i + 2] = (byte)(n >> 8);
            b[i + 3] = (byte)n;
        }

        private static void DES_FP(ref uint X, ref uint Y, ref uint T)
        {														
            X = ((X << 31) | (X >> 1)) & 0xFFFFFFFF;			
            T = (X ^ Y) & 0xAAAAAAAA; X ^= T; Y ^= T;			
            Y = ((Y << 31) | (Y >> 1)) & 0xFFFFFFFF;			
            T = ((Y >>  8) ^ X) & 0x00FF00FF; X ^= T; Y ^= (T <<  8); 
            T = ((Y >>  2) ^ X) & 0x33333333; X ^= T; Y ^= (T <<  2);
            T = ((X >> 16) ^ Y) & 0x0000FFFF; Y ^= T; X ^= (T << 16);
            T = ((X >>  4) ^ Y) & 0x0F0F0F0F; Y ^= T; X ^= (T <<  4);  
        }

        private static uint GET_UINT32(byte[] b, int i)
        {										
            return ((uint)b[i] << 24) | ((uint)b[i + 1] << 16) | ((uint)b[i + 2] << 8) | ((uint)b[i + 3]);
        }

        private static void DES_IP(ref uint X, ref uint Y, ref uint T)
        {
            T = ((X >>  4) ^ Y) & 0x0F0F0F0F; Y ^= T; X ^= (T <<  4);
            T = ((X >> 16) ^ Y) & 0x0000FFFF; Y ^= T; X ^= (T << 16); 
            T = ((Y >>  2) ^ X) & 0x33333333; X ^= T; Y ^= (T <<  2); 
            T = ((Y >>  8) ^ X) & 0x00FF00FF; X ^= T; Y ^= (T <<  8);
            Y = ((Y << 1) | (Y >> 31)) & 0xFFFFFFFF;
            T = (X ^ Y) & 0xAAAAAAAA; Y ^= T; X ^= T;
            X = ((X << 1) | (X >> 31)) & 0xFFFFFFFF;
        }

        private static void DES_ROUND(uint[] SK, int index, ref uint X, ref uint Y, ref uint T)
        {		
            T = SK[index] ^ X;
            Y ^= SB8[ (T	  ) & 0x3F ] ^	
                SB6[ (T >>  8) & 0x3F ] ^	
                SB4[ (T >> 16) & 0x3F ] ^	
                SB2[ (T >> 24) & 0x3F ];	
                                                
            T = SK[index + 1] ^ ((X << 28) | (X >> 4));	
            Y ^= SB7[ (T	  ) & 0x3F ] ^		
                SB5[ (T >>  8) & 0x3F ] ^
                SB3[ (T >> 16) & 0x3F ] ^	
                SB1[ (T >> 24) & 0x3F ];
        }
        
        private static void des_crypt(uint[] SK, byte[] input, byte[] output) 
        {
            uint X, Y, T;

            X = GET_UINT32(input, 0);
            Y = GET_UINT32(input, 4);
            T = 0;

            DES_IP(ref X, ref Y, ref T);

            DES_ROUND(SK, 0, ref Y, ref X, ref T);  DES_ROUND(SK, 2, ref X, ref Y, ref T);
            DES_ROUND(SK, 4, ref Y, ref X, ref T);  DES_ROUND(SK, 6, ref X, ref Y, ref T);
            DES_ROUND(SK, 8, ref Y, ref X, ref T);  DES_ROUND(SK, 10, ref X, ref Y, ref T);
            DES_ROUND(SK, 12, ref Y, ref X, ref T); DES_ROUND(SK, 14, ref X, ref Y, ref T);
            DES_ROUND(SK, 16, ref Y, ref X, ref T); DES_ROUND(SK, 18, ref X, ref Y, ref T);
            DES_ROUND(SK, 20, ref Y, ref X, ref T);  DES_ROUND(SK, 22, ref X, ref Y, ref T);
            DES_ROUND(SK, 24, ref Y, ref X, ref T);  DES_ROUND(SK, 26, ref X, ref Y, ref T);
            DES_ROUND(SK, 28, ref Y, ref X, ref T);  DES_ROUND(SK, 30, ref X, ref Y, ref T);

            DES_FP(ref Y, ref X, ref T);

            PUT_UINT32(Y, output, 0);
            PUT_UINT32(X, output, 4);
        }

        public static byte[] DesEncode(ulong secret, string token) 
        {
            uint[] SK = new uint[32];
            des_key(secret, SK);

            int textsz = token.Length;
            int chunksz = (textsz + 8) & ~7;
            byte[] buffer = null;
            if (chunksz > SMALL_CHUNK) {
                buffer = new byte[chunksz];
            }
            else {
                buffer = new byte[SMALL_CHUNK];
            }

            int i = 0;
            for (i=0;i<(int)textsz-7;i+=8) {
                string tt = token.Substring(i);
                byte[] tb = new byte[buffer.Length - i];
                des_crypt(SK, System.Text.Encoding.UTF8.GetBytes(tt), tb);//buffer+i);

                for (int idx = 0; idx < tb.Length; ++idx) {
                    buffer[i + idx] = tb[idx];
                }
            }
            int bytes = textsz - i;
            byte[] tail = new byte[8];
            int j;
            for (j=0;j<8;j++) {
                if (j < bytes) {
                    tail[j] = (byte)token[i+j];
                } else if (j==bytes) {
                    tail[j] = 0x80;
                } else {
                    tail[j] = 0;
                }
            }

            byte[] tbb = new byte[buffer.Length - i];
            des_crypt(SK, tail, tbb);//buffer+i);
            for (int idx = 0; idx < tbb.Length; ++idx) {
                buffer[i + idx] = tbb[idx];
            }

            byte[] result = new byte[chunksz];
            for (int ii = 0; ii < chunksz; ++ii) {
                result[ii] = buffer[ii];
            }
            return result;
        }

        private static void des_key(ulong secret, uint[] SK) 
        {
            des_main_ks(SK, System.BitConverter.GetBytes(secret));
        }

        private static void des_main_ks(uint[] SK, byte[] key) 
        {
            int i;
            uint X, Y, T;

            X = GET_UINT32( key, 0 );
            Y = GET_UINT32( key, 4 );
            T = 0;

            /* Permuted Choice 1 */

            T =  ((Y >>  4) ^ X) & 0x0F0F0F0F;  X ^= T; Y ^= (T <<  4);
            T =  ((Y	  ) ^ X) & 0x10101010;  X ^= T; Y ^= (T	  );

            X =   (LHs[ (X	  ) & 0xF] << 3) | (LHs[ (X >>  8) & 0xF ] << 2)
                | (LHs[ (X >> 16) & 0xF] << 1) | (LHs[ (X >> 24) & 0xF ]	 )
                | (LHs[ (X >>  5) & 0xF] << 7) | (LHs[ (X >> 13) & 0xF ] << 6)
                | (LHs[ (X >> 21) & 0xF] << 5) | (LHs[ (X >> 29) & 0xF ] << 4);

            Y =   (RHs[ (Y >>  1) & 0xF] << 3) | (RHs[ (Y >>  9) & 0xF ] << 2)
                | (RHs[ (Y >> 17) & 0xF] << 1) | (RHs[ (Y >> 25) & 0xF ]	 )
                | (RHs[ (Y >>  4) & 0xF] << 7) | (RHs[ (Y >> 12) & 0xF ] << 6)
                | (RHs[ (Y >> 20) & 0xF] << 5) | (RHs[ (Y >> 28) & 0xF ] << 4);

            X &= 0x0FFFFFFF;
            Y &= 0x0FFFFFFF;

            /* calculate subkeys */

            for( i = 0; i < 16; i++ )
            {
                if( i < 2 || i == 8 || i == 15 )
                {
                    X = ((X <<  1) | (X >> 27)) & 0x0FFFFFFF;
                    Y = ((Y <<  1) | (Y >> 27)) & 0x0FFFFFFF;
                }
                else
                {
                    X = ((X <<  2) | (X >> 26)) & 0x0FFFFFFF;
                    Y = ((Y <<  2) | (Y >> 26)) & 0x0FFFFFFF;
                }

                SK[i * 2] =   ((X <<  4) & 0x24000000) | ((X << 28) & 0x10000000)
                        | ((X << 14) & 0x08000000) | ((X << 18) & 0x02080000)
                        | ((X <<  6) & 0x01000000) | ((X <<  9) & 0x00200000)
                        | ((X >>  1) & 0x00100000) | ((X << 10) & 0x00040000)
                        | ((X <<  2) & 0x00020000) | ((X >> 10) & 0x00010000)
                        | ((Y >> 13) & 0x00002000) | ((Y >>  4) & 0x00001000)
                        | ((Y <<  6) & 0x00000800) | ((Y >>  1) & 0x00000400)
                        | ((Y >> 14) & 0x00000200) | ((Y	  ) & 0x00000100)
                        | ((Y >>  5) & 0x00000020) | ((Y >> 10) & 0x00000010)
                        | ((Y >>  3) & 0x00000008) | ((Y >> 18) & 0x00000004)
                        | ((Y >> 26) & 0x00000002) | ((Y >> 24) & 0x00000001);

                SK[i * 2 + 1] =   ((X << 15) & 0x20000000) | ((X << 17) & 0x10000000)
                        | ((X << 10) & 0x08000000) | ((X << 22) & 0x04000000)
                        | ((X >>  2) & 0x02000000) | ((X <<  1) & 0x01000000)
                        | ((X << 16) & 0x00200000) | ((X << 11) & 0x00100000)
                        | ((X <<  3) & 0x00080000) | ((X >>  6) & 0x00040000)
                        | ((X << 15) & 0x00020000) | ((X >>  4) & 0x00010000)
                        | ((Y >>  2) & 0x00002000) | ((Y <<  8) & 0x00001000)
                        | ((Y >> 14) & 0x00000808) | ((Y >>  9) & 0x00000400)
                        | ((Y	  ) & 0x00000200) | ((Y <<  7) & 0x00000100)
                        | ((Y >>  7) & 0x00000020) | ((Y >>  3) & 0x00000011)
                        | ((Y <<  2) & 0x00000004) | ((Y >> 21) & 0x00000002);
            }
        }
    }
}
