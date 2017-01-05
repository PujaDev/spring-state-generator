using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringStateGenerator
{
    class PropertyComparer : IComparer<Property>
    {
        public int Compare(Property x, Property y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }
}
