﻿// This code is originally from UnityStudio, adapted here to suit Valkyrie

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Unity_Studio
{
    public enum EndianType
    {
        BigEndian,
        LittleEndian
    }

    public class EndianStream : BinaryReader
    {
        public EndianType endian;
        private byte[] a16 = new byte[2];
        private byte[] a32 = new byte[4];
        private byte[] a64 = new byte[8];

        public EndianStream(Stream stream, EndianType endian) : base(stream) { }

        ~EndianStream()
        {
            Dispose(true);
        }

        public long Position { get { return base.BaseStream.Position; } set { base.BaseStream.Position = value; } }

        public new void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override bool ReadBoolean()
        {
            return base.ReadBoolean();
        }

        public override byte ReadByte()
        {
            try
            {
                return base.ReadByte();
            }
            catch
            {
                return 0;
            }
        }

        public override char ReadChar()
        {
            return base.ReadChar();
        }
 
        public override Int16 ReadInt16()
        {
            if (endian == EndianType.BigEndian)
            {
                a16 = base.ReadBytes(2);
                Array.Reverse(a16);
                return BitConverter.ToInt16(a16, 0);
            }
            else return base.ReadInt16();
        }
 
        public override int ReadInt32()
        {
            if (endian == EndianType.BigEndian)
            {
                a32 = base.ReadBytes(4);
                Array.Reverse(a32);
                return BitConverter.ToInt32(a32, 0);
            }
            else return base.ReadInt32();
        }
 
        public override Int64 ReadInt64()
        {
            if (endian == EndianType.BigEndian)
            {
                a64 = base.ReadBytes(8);
                Array.Reverse(a64);
                return BitConverter.ToInt64(a64, 0);
            }
            else return base.ReadInt64();
        }
 
        public override UInt16 ReadUInt16()
        {
            if (endian == EndianType.BigEndian)
            {
                a16 = base.ReadBytes(2);
                Array.Reverse(a16);
                return BitConverter.ToUInt16(a16, 0);
            }
            else return base.ReadUInt16();
        }
 
        public override UInt32 ReadUInt32()
        {
            if (endian == EndianType.BigEndian)
            {
                a32 = base.ReadBytes(4);
                Array.Reverse(a32);
                return BitConverter.ToUInt32(a32, 0);
            }
            else return base.ReadUInt32();
        }

        public override UInt64 ReadUInt64()
        {
            if (endian == EndianType.BigEndian)
            {
                a64 = base.ReadBytes(8);
                Array.Reverse(a64);
                return BitConverter.ToUInt64(a64, 0);
            }
            else return base.ReadUInt64();
        }
 
        public override Single ReadSingle()
        {
            if (endian == EndianType.BigEndian)
            {
                a32 = base.ReadBytes(4);
                Array.Reverse(a32);
                return BitConverter.ToSingle(a32, 0);
            }
            else return base.ReadSingle();
        }
 
        public override Double ReadDouble()
        {
            if (endian == EndianType.BigEndian)
            {
                a64 = base.ReadBytes(8);
                Array.Reverse(a64);
                return BitConverter.ToUInt64(a64, 0);
            }
            else return base.ReadDouble();
        }

        public override string ReadString()
        {
            return base.ReadString();
        }

        public string ReadASCII(int length)
        {
            return ASCIIEncoding.ASCII.GetString(base.ReadBytes(length));
        }

        public void AlignStream(int alignment)
        {
            long pos = base.BaseStream.Position;
            //long padding = alignment - pos + (pos / alignment) * alignment;
            //if (padding != alignment) { base.BaseStream.Position += padding; }
            if ((pos % alignment) != 0) { base.BaseStream.Position += alignment - (pos % alignment); }
        }

        public string ReadAlignedString(int length)
        {
            if (length > 0 && length < (base.BaseStream.Length - base.BaseStream.Position))//crude failsafe
            {
                byte[] stringData = new byte[length];
                base.Read(stringData, 0, length);
                var result = System.Text.Encoding.UTF8.GetString(stringData); //must verify strange characters in PS3

                /*string result = "";
                char c;
                for (int i = 0; i < length; i++)
                {
                    c = (char)base.ReadByte();
                    result += c.ToString();
                }*/

                AlignStream(4);
                return result;
            }
            else { return ""; }
        }
 
        public string ReadStringToNull()
        {
            string result = "";
            char c;
            for (int i = 0; i < base.BaseStream.Length; i++)
            {
                if ((c = (char)base.ReadByte()) == 0)
                {
                    break;
                }
                result += c.ToString();
            }
            return result;
        }
    }
}
