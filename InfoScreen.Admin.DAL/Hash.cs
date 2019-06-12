using System;
using System.Linq;
using System.Security.Cryptography;

namespace InfoScreen.Admin.Logic
{
    public static class Hash
    {
        public const int SaltSize = 128 / 8;
        public const int HashSize = 256 / 8 + SaltSize + 13;

        private const int Iterations = 100000;
        private const int HashLength = 256 / 8;

        public static (byte[] hash, byte[] salt) HashPassword(string password)
        {
            var salt = GenerateSalt();
            var hash = HashString(password, salt);

            var output = new byte[13 + salt.Length + hash.Length];
            output[0] = 0x01; // format marker


            WriteNetworkByteOrder(output, 1, 1);
            WriteNetworkByteOrder(output, 5, Iterations);
            WriteNetworkByteOrder(output, 9, SaltSize);

            Buffer.BlockCopy(salt, 0, output, 13, salt.Length);
            Buffer.BlockCopy(hash, 0, output, 13 + salt.Length, hash.Length);

            return (output, salt);
        }

        public static bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            // check version
            if (hash[0] != 0x01)
                return false;

            // read header
            var prfUnused = ReadNetworkByteOrder(hash, 1);
            var iterations = (int) ReadNetworkByteOrder(hash, 5);
            var saltLength = (int) ReadNetworkByteOrder(hash, 9);

            if (saltLength < SaltSize)
                return false;

            var keyLength = hash.Length - 13 - salt.Length;
            if (keyLength < HashLength)
                return false;

            var expected = new byte[HashLength];
            Buffer.BlockCopy(hash, 13 + SaltSize, expected, 0, HashLength);

            var actual = HashString(password, salt, iterations);
            return actual.SequenceEqual(expected);
        }

        private static byte[] HashString(string value, byte[] salt, int iterations = Iterations)
        {
            return new Rfc2898DeriveBytes(value, salt, iterations, HashAlgorithmName.SHA256)
                .GetBytes(HashLength);
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte) (value >> 24);
            buffer[offset + 1] = (byte) (value >> 16);
            buffer[offset + 2] = (byte) (value >> 8);
            buffer[offset + 3] = (byte) (value >> 0);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint) buffer[offset + 0] << 24)
                   | ((uint) buffer[offset + 1] << 16)
                   | ((uint) buffer[offset + 2] << 8)
                   | buffer[offset + 3];
        }

        public static byte[] GenerateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);
            return salt;
        }

        /*private static byte[] Decode(byte[] packet)
        {
            var i = packet.Length - 1;
            while (packet[i] == 0)
            {
                --i;
            }

            byte[] temp = new byte[i + 1];
            Array.Copy(packet, temp, i + 1);
            return temp;
        }*/
    }
}