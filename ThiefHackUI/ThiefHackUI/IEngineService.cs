using EngineLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThiefHackUI
{
    public interface IEngineService
    {
        bool SetParameterByType(uint type, bool value);
        void SetEngineSettings(Setting[] engineSettings);
    }
}
