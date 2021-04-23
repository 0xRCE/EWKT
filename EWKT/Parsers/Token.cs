using System;
using System.Collections.Generic;
using System.Text;

namespace EWKT.Parsers
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string RawValue { get; set; }
        public int Linenumber { get; set; }
        public int Column { get; set; }
    }
}
