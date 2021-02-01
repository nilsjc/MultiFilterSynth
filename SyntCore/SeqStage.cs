using System;
using System.Collections.Generic;
using System.Text;

namespace SyntCore
{
    public class SeqStage
    {
        public bool Function { get; set; }  // 0 = normal 1 = counter
        public bool CarryNotSent { get; set; }
        public bool Q { get; set; }
    }
}
