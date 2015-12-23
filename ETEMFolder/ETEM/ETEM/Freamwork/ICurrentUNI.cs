using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETEM.Freamwork
{
    interface ICurrentUNI
    {
        string CurrentUNIID { get; set; }
        void UserControlLoad();
    }
}
