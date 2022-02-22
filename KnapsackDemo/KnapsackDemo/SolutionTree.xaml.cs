using System;
using System.Collections.Generic;
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
    /// 解空间树用户控件
    /// Interaction logic for SolutionTree.xaml
    /// </summary>
    public partial class SolutionTree : UserControl
    {
        public SolutionTree()
        {
            InitializeComponent();            
        }

        internal SolutionTreePainter Painter { get; set; }

        public void DrawTree(int num,string[] names)
        {
            Painter = new SolutionTreePainter(canvas, num);
            Painter.names = names;
            Painter.Paint();
        }
    }
}
