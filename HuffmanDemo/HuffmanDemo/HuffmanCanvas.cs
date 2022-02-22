using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace HuffmanDemo
{
    /// <summary>
    /// Huffman 树绘制的容器，是程序的最外层容器，用来处理huffman森林变化的逻辑，LineChange 方法是执行过程
    /// </summary>
    class HuffmanCanvas : DemoCanvas
    {
        List<Tree> Trees = new List<Tree>();

        public HuffmanCanvas()
        {
            Logger.NeedUpdateLine += Logger_NeedUpdateLine;
        }

        private void Logger_NeedUpdateLine(object sender, int e)
        {
            LineChange();
        }

        private string Passage = "";

        public void ReStart()
        {
            ReadText(Passage);
            Logger.IsFinish = false;
        }

        public void ReadText(string passage)
        {
            Passage = passage;
            ClearCanvas();
            this.Trees.Clear();
            Logger.Clear();
            Logger.WriteLine("正在统计词频...");
            var dict = passage.GroupBy(u => u).OrderBy(u=>u.Count()).ToDictionary(u => u.Key);

            Logger.WriteLine("统计结果：");
            int x = 0;
            foreach (var item in dict)
            {
                var k = item.Key;
                var c = item.Value.Count();
                Logger.Write($"{k} - {c}\t");
                if ((++x) % 4 == 0) Logger.WriteLine("");
                var tree = new Tree
                {
                    Root = new TreeNode
                    {
                        Weight = c,
                        Symbol = k.ToString()
                    },
                    RealX = (x-1) * 100
                };

                tree.DrawTree();
                Trees.Add(tree);
            }
            DrawForeset();
        }

        public void DrawForeset()
        {
            ClearCanvas();
            int w = 0;
            foreach (var tree in Trees)
            {
                tree.RealX = w;
                w += (int)Math.Max(100, tree.RWidth);
                AddComponent(tree);
            }
        }

        /// <summary>
        /// 整个算法的核心处理程序
        /// 使用行号判断将代码框中执行的位置
        /// 和C#代码执行的位置相绑定
        /// 
        /// 这只是一个非常初级的模拟实现一个调试器的方法
        /// </summary>
        public void LineChange()
        {
            if (Trees.Count() <= 1)
            {
                // 按右侧程序不会执行到这一步，因为最后做的加法已经没有必要，但是为了程序展示加上
                Trees[0].Root.Weight = Trees[0].Root.LeftChild.Weight + Trees[0].Root.RightChild.Weight;
                Logger.IsFinish = true;
                PrintHuffmanCode();
                return;
            }

            if (Logger.CurrentLine == 1 && Logger.CurrentLine == 2) Logger.NextLine();
            else if (Logger.CurrentLine == 4) 
            {
                Trees.Sort((u, p) => (u.Root.Weight - p.Root.Weight));
                DrawForeset();
                Trees[0].Root.SetRed();
                Trees[1].Root.SetRed();

            }
            else if (Logger.CurrentLine == 5)
            {
                Trees[0].Root.SetDefault();
                Trees[1].Root.SetDefault();
            }
            else if (Logger.CurrentLine == 8)
            {
                var newNode = new Tree
                {
                    Root = new TreeNode
                    {
                        Weight = 0,
                        Symbol = "",
                        LeftChild = Trees[0].Root,
                        RightChild = Trees[1].Root,
                        RealX = 0
                    }

                };
                Trees[0].ClearCanvas();
                Trees[1].ClearCanvas();
                newNode.DrawTree();
                Trees.RemoveRange(0, 2);
                Trees.Insert(0, newNode);
                DrawForeset();
            }
            else if (Logger.CurrentLine == 9)
            {
                Trees[0].Root.Weight = Trees[0].Root.LeftChild.Weight + Trees[0].Root.RightChild.Weight; 
            }

        }


        /// <summary>
        /// 打印Huffman编码，DFS搜索这颗树，如果查询到叶子节点，就输出编码
        /// </summary>
        private void PrintHuffmanCode()
        {
            Logger.WriteLine("");
            Logger.WriteLine("哈夫曼编码已经生成，现在开始打印");
            HuffmanCodeDfs(new int[] { }, Trees[0].Root);
        }

        /// <summary>
        /// 打印Huffman编码的核心DFS程序
        /// </summary>
        private void HuffmanCodeDfs(int[] code,TreeNode node)
        {
            // 递归终止条件
            if (node.LeftChild == null && node.RightChild == null)
            {
                Logger.WriteLine(node.Symbol +"\t" + string.Join("", code));
            }
            else
            {
                if (node.LeftChild != null)
                {
                    HuffmanCodeDfs(code.Concat(new int[] { 0 }).ToArray(),node.LeftChild);
                }

                if (node.RightChild != null)
                {
                    HuffmanCodeDfs(code.Concat(new int[] { 1 }).ToArray(), node.RightChild);
                }

            }


        }

        /* 这是一个测试方法，测试树和森林绘制效果时时使用，正式程序删除
        public void AddTree()
        {
            //var c = new Tree()
            //{
            //    Root = new TreeNode
            //    {
            //        Weight = 20,
            //        LeftChild = new TreeNode
            //        {
            //            Weight = 10
            //        },
            //        RightChild = new TreeNode
            //        {
            //            Weight = 30
            //        }
            //    }
            //};
            //AddComponent(c);
            //c.DrawTree();


        }
        */
    }
}
