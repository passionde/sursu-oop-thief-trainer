using Microsoft.Win32.SafeHandles;

namespace EngineLibrary
{
    public class EngineSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public EngineSafeHandle(IntPtr handle, bool ownsHandle) : this(ownsHandle) { SetHandle(handle); }

        public EngineSafeHandle() : this(true) { }

        private EngineSafeHandle(bool ownsHandle) : base(ownsHandle){ }

        override protected bool ReleaseHandle() { return EngineHelper.destroyEngine(handle); }
    }
}
