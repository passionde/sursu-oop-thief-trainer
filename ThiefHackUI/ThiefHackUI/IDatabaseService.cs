using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThiefHackUI
{
    public interface IDatabaseService
    {
        public void EditOffsetByType (uint type, uint offset);
        public uint GetOffsetByType(uint type);
    }
}
