using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    [Serializable]
    public class TestClass
    {
        public string aString;
        public int anInt;

        public TestClass()
        {
            aString = "Hello";
            anInt = 10;
        }

    }
}
