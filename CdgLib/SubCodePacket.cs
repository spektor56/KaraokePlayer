namespace CdgLib
{
    public class SubCodePacket
    {
        public byte[] Command = new byte[1];
        public byte[] Data = new byte[16];
        public byte[] Instruction = new byte[1];
        public byte[] ParityP = new byte[4];
        public byte[] ParityQ = new byte[2];
    }
}