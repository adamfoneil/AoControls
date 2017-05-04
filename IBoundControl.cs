using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AdamOneilSoftware
{
    public interface IBoundControl
    {
        event EventHandler ValueChanged;

        [Browsable(false)]
        object BoundValue { get; set; }        

        void Clear();
    }
}
