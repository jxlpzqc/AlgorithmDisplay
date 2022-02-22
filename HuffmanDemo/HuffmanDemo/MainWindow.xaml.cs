using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HuffmanDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DemoCanvas.Scale = 1.2;
            //测试代码
            //mainCanvas.AddTree();
            Logger.NeedLog += Logger_NeedLog;
            Logger.NeedClear += Logger_NeedClear;
            Logger.NeedUpdateLine += Logger_NeedUpdateLine;

            timer = new Timer(200);
            timer.Elapsed += Timer_Elapsed;
        }

        private void Logger_NeedUpdateLine(object sender, int e)
        {
            this.code.Focus();
            this.code.SelectedIndex = e - 1;
        }

        private void Logger_NeedClear(object sender, EventArgs e)
        {
            this.console.Text = "";
        }

        private void Logger_NeedLog(object sender, string e)
        {
            this.console.Text += e;
            this.console.ScrollToEnd();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            DemoCanvas.Scale = 10.6 - e.NewValue;
            mainCanvas.Repaint();
        }

        private void ReadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog  = new OpenFileDialog()
            {
                Title = "选择文件"
            };

            if (dialog.ShowDialog() == true)
            {
                var str = File.ReadAllText(dialog.FileName);
                mainCanvas.ReadText(str);

            }

        }


        #region 画布移动控制

        bool IsDrag = false;
        private double currentLeft = 0;
        private double currentTop = 0;
        private bool canvasLock = false;


        /// <summary>
        /// 鼠标移动的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dragPane_MouseMove(object sender, MouseEventArgs e)
        {
            if ((!canvasLock) && IsDrag)
            {
                // 如果正在拖动，改变Top和Left
                this.mainCanvas.Margin = new Thickness
                {
                    Top = (e.GetPosition(dragPane).Y - currentTop),
                    Left = (e.GetPosition(dragPane).X - currentLeft)
                };
            }
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dragPane_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //释放鼠标捕获
            this.dragPane.ReleaseMouseCapture();
            IsDrag = false;
            currentLeft = currentTop = 0;
        }

        /// <summary>
        /// 鼠标按下的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dragPane_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(this.dragPane);
            IsDrag = true;
            currentLeft = e.GetPosition(this.dragPane).X - this.mainCanvas.Margin.Left;
            currentTop = e.GetPosition(this.dragPane).Y - this.mainCanvas.Margin.Top;
        }


        #endregion


        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if (Logger.IsFinish)
            {
                var res = MessageBox.Show("是否重新开始", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res ==  MessageBoxResult.Yes)
                {
                    mainCanvas.ReStart();
                }
            }
            Logger.NextLine();
        }

        /// <summary>
        /// 定时器
        /// </summary>
        private Timer timer;

        private void AutoRunButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Interval = 1000 / speedSlider.Value;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (Logger.IsFinish) timer.Stop();
                Logger.NextLine();
            });
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void StopAutoButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }


        private void BackToOriginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.mainCanvas.Margin = new Thickness
            {
                Top = 0,
                Left = 0
            };
        }


        private void lockMenu_Click(object sender, RoutedEventArgs e)
        {
            if (canvasLock)
            {
                canvasLock = false;
            }
            else
            {
                canvasLock = true;
            }

            lockMenu.IsChecked = canvasLock;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                Logger.NextLine();
            }
        }
    }
}
