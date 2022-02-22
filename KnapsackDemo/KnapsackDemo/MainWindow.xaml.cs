using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KnapsackDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDemoViewer
    {
        private AlgorithmController controller;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCode();
            controller = new AlgorithmController(this);
            this.console.Text += "\n";
            code.Focus();
        }

        private void InitializeCode()
        {
            var lines = File.ReadAllLines("Code.cxx");
            code.ItemsSource = lines;
        }

        public void DrawSolutionTree(int num,string[] names)
        {
            solutionTree.DrawTree(num+1, names);

        }

        public void Error(string message)
        {
            Log("【错误】 " + message);
        }

        public void HighLightNode(int[] nodeid)
        {
            solutionTree.Painter.HighlightNode(nodeid);
        }

        public void LocateLine(int lineNumber)
        {
            code.Focus();
            code.SelectedIndex = lineNumber - 1;
            code.ScrollIntoView(code.Items[lineNumber - 1]);
        }

        public void Log(string message)
        {
            Log(message, Colors.White);
        }

        public void Log(string message, Color color)
        {
            this.console.Text += message;
            this.console.Text += "\n";
            this.console.ScrollToEnd();
        }

        public void LogWithoutLineBreak(string message)
        {
            this.console.Text += message;
            this.console.ScrollToEnd();

        }

        public void SetGoodList(List<Good> goods)
        {
            goodList.Items.Clear();
            foreach (var good in goods)
            {
                goodList.Items.Add(new GoodView(good));
            }
        }

        public void UpdateList(int[] used, double weightUsed, double totalValue)
        {
            foreach (GoodView item in goodList.Items)
            {
                item.DeSelect();
            }

            for (int i = 0; i < used.Length; i++)
            {
                if (used[i] == 1)
                {
                    (goodList.Items[i] as GoodView).Select();
                }

            }


            totalWeight.Text = weightUsed.ToString() + "/" + controller.Capacity;
            this.totalValue.Text = totalValue.ToString();



        }

        public void UpdateMax(double weight, int[] x)
        {
            this.maxValue.Text = weight + $"( {string.Join(",", x.Select(u => u.ToString()).ToArray())} )";
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "选择文件"
            };

            if (dialog.ShowDialog() == true)
            {

                controller.ReadData(dialog.FileName);
            }

        }

        private void AutoRunButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int a = int.Parse(speed.Text);

                controller.AutoRun(a);
                speed.IsEnabled = false;
            }
            catch
            {
                MessageBox.Show("执行速度不是整数！检查后重试");
            }
        }

        private void StopAutoRunButton_Click(object sender, RoutedEventArgs e)
        {

            speed.IsEnabled = true;
            controller.StopAutoRun();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var result =  controller.Next();
            if (result == false)
            {
                var re = MessageBox.Show("程序已经执行完毕，是否重新开始执行？", "询问", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(re == MessageBoxResult.Yes)
                {
                    controller.ReStart();
                }
            }
        }

        public void CutBranch(int[] noteid)
        {
            solutionTree.Painter.CutBranch(noteid);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            this.console.Text = "";
        }
    }
}
