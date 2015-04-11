using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSMP.Model
{
    public class ComboxItemInfo
    {
        public string Text = "";

        public int Value = 0;
        public ComboxItemInfo(string _Text, int _Value)
        {
            Text = _Text;
            Value = _Value;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
