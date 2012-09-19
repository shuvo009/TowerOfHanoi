using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
namespace Tower_Of_Hanoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DiskCollection DiskFirstRow { get; set; }
        public DiskCollection DiskSecondRow { get; set; }
        public DiskCollection DiskThirdRow { get; set; }
        private const string projectName = "Tower Of Hanoi";
        private BackgroundWorker myWorker = new BackgroundWorker();
        private int diskMovment = 0;
        private TimeSpan movementSpreed = new TimeSpan(0,0,0,0,0);
        public MainWindow()
        {
            DiskFirstRow = new DiskCollection();
            DiskSecondRow = new DiskCollection();
            DiskThirdRow = new DiskCollection();
            InitializeComponent();
            myWorker.WorkerReportsProgress = true;
            myWorker.DoWork += new DoWorkEventHandler(myWorker_DoWork);
            myWorker.ProgressChanged += new ProgressChangedEventHandler(myWorker_ProgressChanged);
            myWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(myWorker_RunWorkerCompleted);
            this.TextBoxDiskSize.Focus();
        }

        void myWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Mouse.OverrideCursor = null;
            this.ButtonStart.IsEnabled = true;
            MessageBox.Show("Complete !");
        }

        void myWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string[] valus = e.UserState.ToString().Split('/');
            this.addRemoveCollection(Convert.ToInt16(valus[0]),Convert.ToInt16(valus[1]));
        }

        void myWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker work=sender as BackgroundWorker;
            this.hanoi(1, 3, 2, DiskFirstRow.Count(), work);
        }

        private void ButtonAddClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ButtonAdd.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait; ;
            try
            {
                if (this.TextBoxDiskSize.Text!=string.Empty && !String.IsNullOrWhiteSpace(this.TextBoxDiskSize.Text))
                {
                    int diskSize=Convert.ToInt16(this.TextBoxDiskSize.Text);
                    var sequenceCheck=DiskFirstRow.LastOrDefault(smallValue=>smallValue.DiskSize<=diskSize);
                    if(sequenceCheck==null)
                    {
                        DiskFirstRow.Add(new DiskInfo { DiskSize=diskSize  });
                    }
                    else
                    {
                        throw new Exception(string.Format("Enter a Smaller Disk Size Than {0}", DiskFirstRow.LastOrDefault().DiskSize));
                    }
                }
                else
                {
                    throw new Exception("Please Enter a Disk Size Between 50 to 90 ");
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(error.Message,projectName,MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.ButtonAdd.IsEnabled = true;
                this.TextBoxDiskSize.Text = string.Empty;
            }
        }

        private void ButtonStartClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ButtonStart.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait; ;
            try
            {
                int totalDisks=DiskFirstRow.Count();
                if ( totalDisks> 0)
                {
                    this.textDisk.Text = totalDisks.ToString();
                    this.textMovementCal.Text = (Math.Pow(2,totalDisks) - 1).ToString();
                    movementSpreed = new TimeSpan(0, 0, 0, 0, (int)slider.Value);
                    myWorker.RunWorkerAsync();
                }
                else
                {
                    throw new Exception("There is no Disk !");
                }
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(error.Message, projectName, MessageBoxButton.OK, MessageBoxImage.Error);
                this.ButtonStart.IsEnabled = true;
            }
        }

        private void ButtonStartOverClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ButtonStartOver.IsEnabled = false;
            Mouse.OverrideCursor = Cursors.Wait; ;
            try
            {
                this.DiskFirstRow.Clear();
                this.DiskSecondRow.Clear();
                this.DiskThirdRow.Clear();
                this.textMovement.Text = "0";
                this.textDisk.Text="0";
                this.textMovementCal.Text = "0";
                this.diskMovment = 0;
            }
            catch (Exception error)
            {
                Mouse.OverrideCursor = null;
                MessageBox.Show(error.Message, projectName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                this.ButtonStartOver.IsEnabled = true;
            }
        }

        private void TextBoxDiskSizeKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Return))
            {
                this.ButtonAddClick(null, null);
            }
        }

        private void hanoi(int from, int to,int midd, int num,BackgroundWorker worker)
        {

            if (num.Equals(1))
            {
                //this.addRemoveCollection(from, to);
                worker.ReportProgress(0, string.Format("{0}/{1}", from, to));
                System.Threading.Thread.Sleep(movementSpreed);
            }
            else
            {
                hanoi(from, midd,to, num - 1,worker);
               // this.addRemoveCollection(from, to); 
                worker.ReportProgress(0, string.Format("{0}/{1}", from, to));
                System.Threading.Thread.Sleep(movementSpreed);
                hanoi(midd, to,from ,num - 1,worker);
            }
        }

        private void addRemoveCollection(int from, int to)
        {
            DiskInfo tempdiskInfo = new DiskInfo();
           // tempdiskInfo = null;
            if (from.Equals(1))
            {
                tempdiskInfo = DiskFirstRow.LastOrDefault();
            }
            else if(from.Equals(2))
            {
                tempdiskInfo=DiskSecondRow.LastOrDefault();
            }
            else
            {
                tempdiskInfo = DiskThirdRow.LastOrDefault();
            }

            if (to.Equals(1))
            {
                 DiskFirstRow.Add(tempdiskInfo);
            }
            else if(to.Equals(2))
            {
                DiskSecondRow.Add(tempdiskInfo);
            }
            else
            {
                DiskThirdRow.Add(tempdiskInfo);
            }

            if (from.Equals(1))
            {
                DiskFirstRow.Remove(tempdiskInfo);
            }
            else if (from.Equals(2))
            {
               DiskSecondRow.Remove(tempdiskInfo);
            }
            else
            {
                DiskThirdRow.Remove(tempdiskInfo);
            }
            this.textMovement.Text = (++diskMovment).ToString();
        }

    }
}
