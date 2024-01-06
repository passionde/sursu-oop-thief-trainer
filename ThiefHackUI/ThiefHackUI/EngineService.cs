using EngineLibrary;

namespace ThiefHackUI
{
    class EngineService : IEngineService
    {
        private Setting[] _engineSettings = [];
        public bool SetParameterByType(uint type, bool value)
        {
            EngineSafeHandle engine = EngineHelper.CreateEngine(_engineSettings);
            bool result = true;

            if (type == EngineHelper.AMMO_PARAMETR)
            {
                if (value) 
                { 
                    result = EngineHelper.HackAmmo(engine); 
                }
                else 
                {
                    result = EngineHelper.RestoreAmmo(engine); 
                }
            }
            if (type == EngineHelper.MONEY_PARAMETR)
            {
                if (value) 
                {
                    result = EngineHelper.HackMoney(engine); 
                }
                else {
                    result = EngineHelper.RestoreMoney(engine); 
                }
            }

            return result;
        }

        public void SetEngineSettings(Setting[] engineSettings)
        {
            _engineSettings = engineSettings;
        }
    }
}
