using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace highwayns
{
    public class Message
    {
        public string name;
        public string cmdid;
        public List<string> fields = new List<string>();
        public List<string> fields_type = new List<string>();
        public List<string> fields_require = new List<string>();
        public List<string> fields_value = new List<string>();
    }
}
