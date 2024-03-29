﻿using System.Text;

namespace SecureVault.Services
{
    public class AesEncryption
    {
        private static byte[] invSBox = {
            0x52, 0x09, 0x6A, 0xD5, 0x30, 0x36, 0xA5, 0x38, 0xBF, 0x40, 0xA3, 0x9E, 0x81, 0xF3, 0xD7, 0xFB,
            0x7C, 0xE3, 0x39, 0x82, 0x9B, 0x2F, 0xFF, 0x87, 0x34, 0x8E, 0x43, 0x44, 0xC4, 0xDE, 0xE9, 0xCB,
            0x54, 0x7B, 0x94, 0x32, 0xA6, 0xC2, 0x23, 0x3D, 0xEE, 0x4C, 0x95, 0x0B, 0x42, 0xFA, 0xC3, 0x4E,
            0x08, 0x2E, 0xA1, 0x66, 0x28, 0xD9, 0x24, 0xB2, 0x76, 0x5B, 0xA2, 0x49, 0x6D, 0x8B, 0xD1, 0x25,
            0x72, 0xF8, 0xF6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xD4, 0xA4, 0x5C, 0xCC, 0x5D, 0x65, 0xB6, 0x92,
            0x6C, 0x70, 0x48, 0x50, 0xFD, 0xED, 0xB9, 0xDA, 0x5E, 0x15, 0x46, 0x57, 0xA7, 0x8D, 0x9D, 0x84,
            0x90, 0xD8, 0xAB, 0x00, 0x8C, 0xBC, 0xD3, 0x0A, 0xF7, 0xE4, 0x58, 0x05, 0xB8, 0xB3, 0x45, 0x06,
            0xD0, 0x2C, 0x1E, 0x8F, 0xCA, 0x3F, 0x0F, 0x02, 0xC1, 0xAF, 0xBD, 0x03, 0x01, 0x13, 0x8A, 0x6B,
            0x3A, 0x91, 0x11, 0x41, 0x4F, 0x67, 0xDC, 0xEA, 0x97, 0xF2, 0xCF, 0xCE, 0xF0, 0xB4, 0xE6, 0x73,
            0x96, 0xAC, 0x74, 0x22, 0xE7, 0xAD, 0x35, 0x85, 0xE2, 0xF9, 0x37, 0xE8, 0x1C, 0x75, 0xDF, 0x6E,
            0x47, 0xF1, 0x1A, 0x71, 0x1D, 0x29, 0xC5, 0x89, 0x6F, 0xB7, 0x62, 0x0E, 0xAA, 0x18, 0xBE, 0x1B,
            0xFC, 0x56, 0x3E, 0x4B, 0xC6, 0xD2, 0x79, 0x20, 0x9A, 0xDB, 0xC0, 0xFE, 0x78, 0xCD, 0x5A, 0xF4,
            0x1F, 0xDD, 0xA8, 0x33, 0x88, 0x07, 0xC7, 0x31, 0xB1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xEC, 0x5F,
            0x60, 0x51, 0x7F, 0xA9, 0x19, 0xB5, 0x4A, 0x0D, 0x2D, 0xE5, 0x7A, 0x9F, 0x93, 0xC9, 0x9C, 0xEF,
            0xA0, 0xE0, 0x3B, 0x4D, 0xAE, 0x2A, 0xF5, 0xB0, 0xC8, 0xEB, 0xBB, 0x3C, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2B, 0x04, 0x7E, 0xBA, 0x77, 0xD6, 0x26, 0xE1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0C, 0x7D
        };

        private static byte[] sBox = {
            0x63,0x7C,0x77,0x7B,0xF2,0x6B,0x6F,0xC5,0x30,0x01,0x67,0x2B,0xFE,0xD7,0xAB,0x76,
            0xCA,0x82,0xC9,0x7D,0xFA,0x59,0x47,0xF0,0xAD,0xD4,0xA2,0xAF,0x9C,0xA4,0x72,0xC0,
            0xB7,0xFD,0x93,0x26,0x36,0x3F,0xF7,0xCC,0x34,0xA5,0xE5,0xF1,0x71,0xD8,0x31,0x15,
            0x04,0xC7,0x23,0xC3,0x18,0x96,0x05,0x9A,0x07,0x12,0x80,0xE2,0xEB,0x27,0xB2,0x75,
            0x09,0x83,0x2C,0x1A,0x1B,0x6E,0x5A,0xA0,0x52,0x3B,0xD6,0xB3,0x29,0xE3,0x2F,0x84,
            0x53,0xD1,0x00,0xED,0x20,0xFC,0xB1,0x5B,0x6A,0xCB,0xBE,0x39,0x4A,0x4C,0x58,0xCF,
            0xD0,0xEF,0xAA,0xFB,0x43,0x4D,0x33,0x85,0x45,0xF9,0x02,0x7F,0x50,0x3C,0x9F,0xA8,
            0x51,0xA3,0x40,0x8F,0x92,0x9D,0x38,0xF5,0xBC,0xB6,0xDA,0x21,0x10,0xFF,0xF3,0xD2,
            0xCD,0x0C,0x13,0xEC,0x5F,0x97,0x44,0x17,0xC4,0xA7,0x7E,0x3D,0x64,0x5D,0x19,0x73,
            0x60,0x81,0x4F,0xDC,0x22,0x2A,0x90,0x88,0x46,0xEE,0xB8,0x14,0xDE,0x5E,0x0B,0xDB,
            0xE0,0x32,0x3A,0x0A,0x49,0x06,0x24,0x5C,0xC2,0xD3,0xAC,0x62,0x91,0x95,0xE4,0x79,
            0xE7,0xC8,0x37,0x6D,0x8D,0xD5,0x4E,0xA9,0x6C,0x56,0xF4,0xEA,0x65,0x7A,0xAE,0x08,
            0xBA,0x78,0x25,0x2E,0x1C,0xA6,0xB4,0xC6,0xE8,0xDD,0x74,0x1F,0x4B,0xBD,0x8B,0x8A,
            0x70,0x3E,0xB5,0x66,0x48,0x03,0xF6,0x0E,0x61,0x35,0x57,0xB9,0x86,0xC1,0x1D,0x9E,
            0xE1,0xF8,0x98,0x11,0x69,0xD9,0x8E,0x94,0x9B,0x1E,0x87,0xE9,0xCE,0x55,0x28,0xDF,
            0x8C,0xA1,0x89,0x0D,0xBF,0xE6,0x42,0x68,0x41,0x99,0x2D,0x0F,0xB0,0x54,0xBB,0x16
        };

        private static byte[] rCon = { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36 };

        private static byte GaloisMultiplication(byte a, byte b)
        {
            byte result = 0;
            while (b != 0)
            {
                if ((b & 1) != 0)
                {
                    result ^= a;
                }
                bool carry = (a & 0x80) != 0;
                a <<= 1;
                if (carry)
                {
                    a ^= 0x1B;
                }
                b >>= 1;
            }
            return result;
        }

        private static List<byte[,]> getKeysBlocks(byte[] expandedKey)
        {
            List<byte[,]> result = new List<byte[,]>();
            for (int i = 0; i < expandedKey.Length; i += 16)
            {
                byte[,] block = new byte[4, 4];
                for (int k = 0; k < 4; k++)
                    for (int l = 0; l < 4; l++)
                        block[l, k] = expandedKey[i + l + (k * 4)];
                result.Add(block);
            }
            return result;
        }

        private static List<byte[,]> arrayToBlocks(byte[] originalBytes, int blockSize = 16)
        {
            List<byte[,]> result = new List<byte[,]>();
            for (int i = 0; i < originalBytes.Length; i += blockSize)
            {
                byte[,] block = new byte[4, 4];
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 4; l++)
                    {
                        int index = i + l + (k * 4);
                        if (index >= originalBytes.Length)
                            block[l, k] = 0;
                        else
                            block[l, k] = originalBytes[index];
                    }
                }
                result.Add(block);
            }
            return result;
        }

        private static byte[] blocksToArray(List<byte[,]> blocks)
        {
            byte[] result = new byte[blocks.Count * blocks[0].Length];
            int index = 0;
            foreach (var block in blocks)
            {
                for (int i = 0; i < block.GetLength(0); i++)
                    for (int j = 0; j < block.GetLength(1); j++)
                        result[index++] = block[j, i];
            }
            return result;
        }

        public static byte[] getBytePassword(string password)
        {
            byte[] temp = Encoding.UTF8.GetBytes(password);
            if (temp.Length % 16 == 0)
                return temp;
            else
            {
                int size = 16 - temp.Length % 16;
                byte[] result = new byte[size + temp.Length];
                Console.WriteLine(result.Length);
                for (int i = 0; i < result.Length; i++)
                    if (i < temp.Length)
                        result[i] = temp[i];
                    else
                        result[i] = 0;
                return result;
            }
        }

        private static byte[] SubWord(byte[] word)
        {
            byte[] result = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                result[i] = sBox[word[i]];
            }
            return result;
        }

        private static byte[] RotWord(byte[] word)
        {
            byte temp = word[0];
            for (int i = 0; i < 3; i++)
            {
                word[i] = word[i + 1];
            }
            word[3] = temp;
            return word;
        }

        private static byte[,] subBytes(byte[,] state, bool inverse = false)
        {
            byte[] box = inverse ? invSBox : sBox;
            byte[,] result = new byte[4, 4];
            for (int i = 0; i < state.GetLength(0); i++)
            {
                for (int j = 0; j < state.GetLength(1); j++)
                {
                    byte row = (byte)(state[i, j] >> 4);
                    byte col = (byte)(state[i, j] & 0x0F);
                    result[i, j] = box[row * 16 + col];
                }
            }
            return result;
        }

        private static byte[,] shiftRows(byte[,] state, bool inverse = false)
        {
            byte[,] result = new byte[4, 4];
            if (inverse)
                for (int i = 0; i < state.GetLength(0); i++)
                    for (int j = 0; j < state.GetLength(1); j++)
                        result[i, j] = state[i, (j + (4 - i)) % 4];
            else
                for (int i = 0; i < state.GetLength(0); i++)
                    for (int j = 0; j < state.GetLength(1); j++)
                        result[i, j] = state[i, (j + i) % 4];
            return result;
        }

        private static byte[,] mixColumns(byte[,] state, bool inverse = false)
        {
            byte[,] result = new byte[4, 4];
            if (inverse)
                for (int col = 0; col < 4; col++)
                {
                    result[0, col] = (byte)(GaloisMultiplication(state[0, col], 0x0e) ^ GaloisMultiplication(state[1, col], 0x0b) ^ GaloisMultiplication(state[2, col], 0x0d) ^ GaloisMultiplication(state[3, col], 0x09));
                    result[1, col] = (byte)(GaloisMultiplication(state[0, col], 0x09) ^ GaloisMultiplication(state[1, col], 0x0e) ^ GaloisMultiplication(state[2, col], 0x0b) ^ GaloisMultiplication(state[3, col], 0x0d));
                    result[2, col] = (byte)(GaloisMultiplication(state[0, col], 0x0d) ^ GaloisMultiplication(state[1, col], 0x09) ^ GaloisMultiplication(state[2, col], 0x0e) ^ GaloisMultiplication(state[3, col], 0x0b));
                    result[3, col] = (byte)(GaloisMultiplication(state[0, col], 0x0b) ^ GaloisMultiplication(state[1, col], 0x0d) ^ GaloisMultiplication(state[2, col], 0x09) ^ GaloisMultiplication(state[3, col], 0x0e));
                }
            else
                for (int col = 0; col < 4; col++)
                {
                    result[0, col] = (byte)(GaloisMultiplication(state[0, col], 2) ^ GaloisMultiplication(state[1, col], 3) ^ state[2, col] ^ state[3, col]);
                    result[1, col] = (byte)(state[0, col] ^ GaloisMultiplication(state[1, col], 2) ^ GaloisMultiplication(state[2, col], 3) ^ state[3, col]);
                    result[2, col] = (byte)(state[0, col] ^ state[1, col] ^ GaloisMultiplication(state[2, col], 2) ^ GaloisMultiplication(state[3, col], 3));
                    result[3, col] = (byte)(GaloisMultiplication(state[0, col], 3) ^ state[1, col] ^ state[2, col] ^ GaloisMultiplication(state[3, col], 2));
                }
            return result;
        }

        private static List<byte[,]> keyExpansion(byte[] key)
        {
            int Nk = key.Length / 4;
            int Nr = Nk + 6;
            int Nb = 4;

            int currentSize = 0;
            int rconIteration = 0;
            int keySize = Nb * (Nr + 1) * 4;
            byte[] expandedKey = new byte[keySize];
            byte[] temp = new byte[4];

            for (int i = 0; i < Nk * 4; i++)
            {
                expandedKey[i] = key[i];
            }
            currentSize += Nk * 4;

            while (currentSize < keySize)
            {
                for (int i = 0; i < 4; i++)
                {
                    temp[i] = expandedKey[currentSize - 4 + i];
                }

                if (currentSize % (Nk * 4) == 0)
                {
                    temp = SubWord(RotWord(temp));
                    temp[0] ^= rCon[rconIteration];
                    rconIteration++;
                }

                for (int i = 0; i < 4; i++)
                {
                    expandedKey[currentSize] = (byte)(expandedKey[currentSize - Nk * 4] ^ temp[i]);
                    currentSize++;
                }
            }

            return getKeysBlocks(expandedKey);
        }

        private static byte[,] addRoundKey(byte[,] state, byte[,] roundKey)
        {
            byte[,] result = new byte[4, 4];
            for (int i = 0; i < state.GetLength(0); i++)
                for (int j = 0; j < state.GetLength(1); j++)
                    result[i, j] = (byte)(state[i, j] ^ roundKey[i, j]);
            return result;
        }

        public static byte[] encrypt(byte[] text, byte[] key)
        {
            var blocks = arrayToBlocks(text);
            var roundKeys = keyExpansion(key);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i] = addRoundKey(blocks[i], roundKeys[0]);
                for (int j = 1; j < 10; j++)
                {
                    blocks[i] = subBytes(blocks[i]);
                    blocks[i] = shiftRows(blocks[i]);
                    blocks[i] = mixColumns(blocks[i]);
                    blocks[i] = addRoundKey(blocks[i], roundKeys[j]);
                }

                blocks[i] = subBytes(blocks[i]);
                blocks[i] = shiftRows(blocks[i]);
                blocks[i] = addRoundKey(blocks[i], roundKeys[10]);
            }
            return blocksToArray(blocks);
        }

        public static byte[] decrypt(byte[] text, byte[] key)
        {
            var blocks = arrayToBlocks(text);
            var roundKeys = keyExpansion(key);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i] = addRoundKey(blocks[i], roundKeys[10]);
                for (int j = 1; j < 10; j++)
                {
                    blocks[i] = shiftRows(blocks[i], true);
                    blocks[i] = subBytes(blocks[i], true);
                    blocks[i] = addRoundKey(blocks[i], roundKeys[10 - j]);
                    blocks[i] = mixColumns(blocks[i], true);
                }

                blocks[i] = shiftRows(blocks[i], true);
                blocks[i] = subBytes(blocks[i], true);
                blocks[i] = addRoundKey(blocks[i], roundKeys[0]);
            }
            return blocksToArray(blocks);
        }
    }
}
