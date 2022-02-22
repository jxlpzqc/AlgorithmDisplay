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

namespace HuffmanDemo
{
    /// <summary>
    /// Interaction logic for TreeNode.xaml
    /// </summary>
    public partial class TreeNode : DemoComponent
    {


        public int Weight
        {
            get { return (int)GetValue(WeightProperty); }
            set
            {
                SetValue(WeightProperty, value);
                weightTp.Text = "节点权重： " + value;
            }
        }

        // Using a DependencyProperty as the backing store for Weight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register("Weight", typeof(int), typeof(TreeNode), new PropertyMetadata(0));




        public string Symbol
        {
            get { return (string)GetValue(SymbolProperty); }
            set 
            {
                SetValue(SymbolProperty, value);
                symbolTp.Text = string.IsNullOrEmpty(value) ? "" : "节点标识： " + value;
            }
        }

        // Using a DependencyProperty as the backing store for Symbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbolProperty =
            DependencyProperty.Register("Symbol", typeof(string), typeof(TreeNode), new PropertyMetadata(""));


        public void SetRed()
        {

            el.Fill = new SolidColorBrush(Colors.Red);
        }

        public void SetDefault()
        {
            el.Fill = new SolidColorBrush(Color.FromRgb(0xf1, 0xc5, 0xa3));
        }

        public TreeNode LeftChild { get; set; }

        public TreeNode RightChild { get; set; }


        public TreeNode()
        {
            InitializeComponent();
        }
    }
}
