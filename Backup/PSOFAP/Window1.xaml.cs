using System.Windows;
using PSOFAP.FAPPSO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;

namespace PSOFAP
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            Thread t1 = new Thread((ThreadStart)delegate
                {
                    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
                    algo1.PerformCostTest();
                    algo1.Initialize();
                    algo1.Start();
                });
            //Thread t2 = new Thread((ThreadStart)delegate
            //{
            //    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
            //    algo1.Initialize();
            //    algo1.Start();
            //});
            //Thread t3 = new Thread((ThreadStart)delegate
            //{
            //    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
            //    algo1.Initialize();
            //    algo1.Start();
            //});
            //Thread t4 = new Thread((ThreadStart)delegate
            //{
            //    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
            //    algo1.Initialize();
            //    algo1.Start();
            //});
            //Thread t5 = new Thread((ThreadStart)delegate
            //{
            //    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
            //    algo1.Initialize();
            //    algo1.Start();
            //});
            //Thread t6 = new Thread((ThreadStart)delegate
            //{
            //    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
            //    algo1.Initialize();
            //    algo1.Start();
            //});
            //Thread t7 = new Thread((ThreadStart)delegate
            //{
            //    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
            //    algo1.Initialize();
            //    algo1.Start();
            //});
            //Thread t8 = new Thread((ThreadStart)delegate
            //{
            //    FAPPSOAlgorithm algo1 = new FAPPSOAlgorithm(1000);
            //    algo1.Initialize();
            //    algo1.Start();
            //});
            t1.Start();
            //t2.Start();
            //t3.Start();
            //t4.Start();
            //t5.Start();
            //t6.Start();
            //t7.Start();
            //t8.Start();
        }

        private void Window_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Display information about this event
            this.Title = "Source = " + e.Source.GetType().Name + ", OriginalSource = " + e.OriginalSource.GetType().Name + " @ " + e.Timestamp;

            //In this example, all possible source derive from  Control
            Control source = e.Source as Control;
            //Toggle the border on the source control
            if(source.BorderThickness != new Thickness(5))
            {
                source.BorderThickness = new Thickness(5);
                source.BorderBrush = Brushes.Black;
            }
            else
                source.BorderThickness = new Thickness(0);
        }
    }
}
