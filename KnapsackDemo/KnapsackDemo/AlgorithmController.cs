using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace KnapsackDemo
{
    /// <summary>
    /// 算法执行控制器
    /// </summary>
    class AlgorithmController
    {

        private IDemoViewer view;

        public AlgorithmController(IDemoViewer view)
        {

            timer.Elapsed += Timer_Elapsed;
            this.view = view;
        }


        double rw = 0;
        int n;
        double maxv = 0;
        int[] x;
        double W;

        public double Capacity => W;



        /// <summary>
        /// 算法执行核心算法，使用yield return模拟程序断点，
        /// 利用巧妙的方法省去了状态机的复杂实现，
        /// 每次返回的是执行到的行号
        /// </summary>
        public IEnumerable<int> Run(int i, double tw, double tv, double rw, int[] op)
        {
            yield return 1;
            view.HighLightNode(op.Take(i).ToArray());
            view.UpdateList(op, tw, tv);
            if (i == n)   //找到一个叶子结点
            {
                yield return 5;
                if (tv > maxv)  //找到一个最优解
                {
                    maxv = tv;
                    yield return 8;
                    for (int j = 0; j < n; j++)  //复制最优解 
                        x[j] = op[j];

                    view.UpdateMax(maxv, x);
                    yield return 11;
                }
            }
            else   //未找完所有物品 
            {
                yield return 15;
                if (tw + rw > W)   //剪枝 
                {
                    yield return 16;
                    foreach (var rr in Run(i + 1, tw, tv, rw - goods[i].Weight, op))
                    {
                        yield return rr;
                    } // 用foreach实现递归
                }
                // 剪枝左子树
                else
                {
                    view.CutBranch(op.Take(i).Concat(new int[] { 0 }).ToArray());
                }

                yield return 18;
                if (tw + goods[i].Weight <= W) //剪枝 
                {
                    yield return 20;
                    op[i] = 1;  //选取第i个物品

                    yield return 21;
                    foreach (var rr in Run(i + 1, tw + goods[i].Weight, tv + goods[i].Weight, rw - goods[i].Weight, op))
                    {
                        yield return rr;
                    }
                }
                else
                {
                    view.CutBranch(op.Take(i).Concat(new int[] { 1 }).ToArray());
                }

                yield return 23;
                op[i] = 0;   //不选取物品i，回溯
            }

        }

        /// <summary>
        /// 物品列表
        /// </summary>
        List<Good> goods;

        public void ReadData(string filename)
        {
            try
            {
                goods = new List<Good>();
                var list = File.ReadAllLines(filename).Where(u => !u.StartsWith("#")).ToList();
                W = double.Parse(list[0]);
                n = int.Parse(list[1]);
                if (n > 10) throw new Exception("物品数量大于10，回溯法解空间太大 { 最坏时间复杂度为O(2^n) }，无法实现图形展示。");

                for (int i = 2; i < list.Count; i++)
                {
                    var line = list[i];
                    var c = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    goods.Add(new Good
                    {
                        Weight = double.Parse(c[0]),
                        Value = double.Parse(c[1]),
                        Name = c.Length > 2 ? c[2] : $"第{i - 1}个物品"
                    });

                }
                view.SetGoodList(goods);
                view.DrawSolutionTree(n,goods.Select(u=>u.Name).ToArray());

            }
            catch (Exception ex)
            {
                MessageBox.Show("输入数据错误，读入文件失败\n错误原因：" + ex.Message);
            }

        }


        #region 算法执行控制相关

        IEnumerator<int> Result;

        public bool Next()
        {
            if (goods == null)
            {
                view.Error("没有数据！");
                return false;
            }

            if (Result == null)
            {
                ReStart();                
            }

            var p = Result.MoveNext();
            view.LocateLine(Result.Current);

            if (p == false)
            {
                view.Log($"执行完毕！\n最高价值为：{maxv} 解向量为："+ $"( {string.Join(",", x.Select(u => u.ToString()).ToArray())} )");
                view.Log("选择的物品为：");
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i] == 1)
                    {
                        view.Log("\t- " + goods[i].Name);
                    }

                }
            }

            return p;
        }

        public void ReStart()
        {
            view.DrawSolutionTree(n, goods.Select(u => u.Name).ToArray());
            if (goods == null)
            {
                view.Error("没有数据！");
                return;
            }
            if (Result != null) Result.Dispose();
            
            n = goods.Count;
            rw = goods.Sum(u => u.Weight);
            x = new int[goods.Count];

            Result = Run(0, 0, 0, rw, new int[n]).GetEnumerator();
        }

        Timer timer = new Timer(500);


        public void AutoRun(int interval = 500)
        {
            timer.Interval = interval;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                if (Next() == false)
                {
                    view.Log("自动执行已完成！");
                    timer.Stop();
                }

            });
        }

        public void StopAutoRun()
        {
            timer.Stop();
        }

        #endregion
    }
}
