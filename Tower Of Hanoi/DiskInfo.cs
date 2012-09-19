using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace Tower_Of_Hanoi
{
    public class DiskInfo : INotifyPropertyChanged
    {
        private int _DiskSize;

        public int DiskSize
        {
            get { return this._DiskSize; }
            set
            {
                if (50<=value && value<=90)
                {
                    this._DiskSize = value;
                    this.OnPropertyChanged("DiskSize");
                }
                else
                {
                    throw new Exception("Out of Range");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
