using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplyOffsetRegidentPlugin
{
    public class Args
    {
        public DxMath.Vector3 Position { get; set; } = new DxMath.Vector3();
        public bool IsLocalL { get; set; } = false;

        public DxMath.Vector3 Rotation { get; set; } = new DxMath.Vector3();
        public bool IsLocalR { get; set; } = false;
    }
}