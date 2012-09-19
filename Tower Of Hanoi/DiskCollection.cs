using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
namespace Tower_Of_Hanoi
{
    public class DiskCollection : ObservableCollection<DiskInfo>
    {
        public DiskCollection() : base()
        {

        }
    }
}
