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
    /// 表明画布中一颗树控件
    /// 
    /// Interaction logic for Tree.xaml
    /// </summary>
    public class Tree : DemoCanvas
    {
        private int leftUsed = 0;

        public int RWidth => leftUsed * 120;

        public override void Repaint()
        {
            ClearCanvas();
            DrawTree();
        }


        public void DrawTree()
        {
            // 使用后序遍历绘制树
            leftUsed = 0;
            DrawNodeAndLine(Root, 0);

        }

        public void ClearTree()
        {
            for (int i = Canvas.Children.Count-1; i >= 0 ; i--)
            {
                if(Canvas.Children[i] is DemoComponent p)
                    RemoveComponent(p);
                this.RemoveLogicalChild(Canvas.Children[i]);
                this.RemoveVisualChild(Canvas.Children[i]);

            }
        }

        public void DrawNodeAndLine(TreeNode node, int level)
        {
            if (node.LeftChild != null)
                DrawNodeAndLine(node.LeftChild, level + 1);

            if (node.RightChild != null)
                DrawNodeAndLine(node.RightChild, level + 1);


            node.RealY = (level) * 120;


            if (node.LeftChild != null && node.RightChild != null)
            {
                node.RealX = (node.LeftChild.RealX + node.RightChild.RealX) / 2;
            }
            else if (node.LeftChild != null)
            {
                node.RealX = (node.LeftChild.RealX + 50);
            }
            else if (node.RightChild != null)
            {
                node.RealX = (node.RightChild.RealX + 10);
            }
            else
            {
                node.RealX = (leftUsed++) * 120;
            }

            if (node.LeftChild != null)
            {
                var line = new Line()
                {
                    X1 = ToCanvasSize(node.LeftChild.RealX + 40),
                    Y1 = ToCanvasSize(node.LeftChild.RealY + 40),
                    X2 = ToCanvasSize(node.RealX + 40),
                    Y2 = ToCanvasSize(node.RealY + 40),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 3
                };
                line.SetValue(Panel.ZIndexProperty, -1);
                this.Canvas.Children.Add(line);
            }

            if (node.RightChild != null)
            {
                var line = new Line()
                {
                    X1 = ToCanvasSize(node.RightChild.RealX + 40),
                    Y1 = ToCanvasSize(node.RightChild.RealY + 40),
                    X2 = ToCanvasSize(node.RealX + 40),
                    Y2 = ToCanvasSize(node.RealY + 40),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 3,
                };
                line.SetValue(Panel.ZIndexProperty, -1);

                this.Canvas.Children.Add(line);
            }

            this.AddComponent(node);



        }


        public TreeNode Root { get; set; }
    }
}
