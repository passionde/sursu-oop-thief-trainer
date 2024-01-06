using System.Runtime.InteropServices;

namespace EngineLibrary
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct Setting
    {
        public uint Parameter;
        public uint Offset;
    }
}
