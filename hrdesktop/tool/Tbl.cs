using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace highwayns
{
    public class Tbl
    {
        public string name;
        public string pk;
        public string charset;
        public string enqine;
        public List<string> fields = new List<string>();
        public List<string> fields_type = new List<string>();
        public List<string> fields_size = new List<string>();
        public List<string> fields_sign = new List<string>();
        public List<string> fields_null = new List<string>();
        public List<string> fields_default = new List<string>();
        public List<string> fields_increase = new List<string>();
        public List<string> keys = new List<string>();
    }
}
