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
    /// Interaction logic for GoodView.xaml
    /// </summary>
    public partial class GoodView : UserControl
    {
        public GoodView(string name,double value,double weight)
        {
            InitializeComponent();
            this.name.Content = name;
            this.value.Content = value;
            this.weight.Content = weight;
        }

        public GoodView(Good good)
        {
            InitializeComponent();
            this.name.Content = good.Name;
            this.value.Content = good.Value;
            this.weight.Content = good.Weight;
        }

        public void Select()
        {
            this.Background = Brushes.LightBlue;
        }

        public void DeSelect()
        {
            this.Background = Brushes.Transparent;
        }
    }
}
