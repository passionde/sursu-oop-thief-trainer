using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EngineLibrary
{
    public static class EngineHelper
    {
        public const uint AMMO_PARAMETR = 1;
        public const uint MONEY_PARAMETR = 2;

        public const uint AMMO_OFFSET = 0x21AAEA;
        public const uint MONEY_OFFSET = 0x269FF3;

        private const string DllPath = "./runtimes/win-x64/native/EngineCoreCpp.dll";

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern EngineSafeHandle createEngine(
            uint pid, 
            uint baseAddress, 
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] Setting[] settings,
            int size);

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool destroyEngine(nint engine);

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool hackAmmo(EngineSafeHandle engine);

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool restoreAmmo(EngineSafeHandle engine);

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool hackMoney(EngineSafeHandle engine);

        [DllImport(DllPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool restoreMoney(EngineSafeHandle engine);

        public static EngineSafeHandle CreateEngine()
        {
            return CreateEngine([]);
        }

        public static EngineSafeHandle CreateEngine(Setting[] settings)
        {
            Process? process = Process.GetProcessesByName("T3Main").FirstOrDefault();
            return CreateEngine(process, settings);
        }

        public static EngineSafeHandle CreateEngine(Process process)
        {
            return CreateEngine(process, []);
        }

        public static EngineSafeHandle CreateEngine(Process? process, Setting[] settings)
        {
            if (process is null)
            {
                throw new EngineException("The game process was not found, you must first launch the game");
            }
            uint pid = (uint)process.Id;
            uint baseAddress = (uint)process.MainModule.BaseAddress.ToInt32();

            EngineSafeHandle engine = createEngine(pid, baseAddress, settings, settings.Length);
            if (engine.IsInvalid)
            {
                throw new EngineException("Invalid pointer to engine");
            }
            return engine;
        }

        public static bool HackAmmo(EngineSafeHandle engine)
        {
            return hackAmmo(engine);
        }

        public static bool RestoreAmmo(EngineSafeHandle engine)
        {
            return restoreAmmo(engine);
        }

        public static bool HackMoney(EngineSafeHandle engine)
        {
            return hackMoney(engine);
        }

        public static bool RestoreMoney(EngineSafeHandle engine)
        {
            return restoreMoney(engine);
        }

        public static bool HackAll(EngineSafeHandle engine)
        {
            return HackMoney(engine) && HackAmmo(engine);
        }

        public static bool RestoreAll(EngineSafeHandle engine)
        {
            return restoreMoney(engine) && RestoreAmmo(engine);
        }
    }
}
