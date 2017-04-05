using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace highwayns
{
    public class Row
    {
        public string[] cols = null;
    }
    public class Dat
    {
        public string name;
        public List<Row> rows = new List<Row>();
    }
}
