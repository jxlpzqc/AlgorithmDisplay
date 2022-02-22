using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace KnapsackDemo
{
    /// <summary>
    /// 展示面板接口，定义所有算法控制器调用的前台界面的方法，
    /// 需要被算法控制器控制的一切视图应该实现这个接口
    /// </summary>
    interface IDemoViewer
    {
        #region 日志相关
        void Log(string message);
        void LogWithoutLineBreak(string message);
        void Log(string message, Color color);
        void Error(string message);
        #endregion


        #region 图形树相关
        /// <summary>
        /// 初始化并绘制解空间树
        /// </summary>
        /// <param name="num">物品的个数</param>
        void DrawSolutionTree(int num,string[] names);

        /// <summary>
        /// 高亮某个解空间节点
        /// </summary>
        /// <param name="nodeid">解id，为0和1的数组，代表选择或者不选择该物品</param>
        void HighLightNode(int[] nodeid);

        void CutBranch(int[] noteid);

        #endregion


        #region 背包栏相关

        void UpdateList(int[] used, double weightUsed, double totalValue);
        void UpdateMax(double weight, int[] x);
        void SetGoodList(List<Good> goods);

        #endregion

        #region 代码相关
        /// <summary>
        /// 代码区定位到某一行
        /// </summary>
        /// <param name="lineNumber"></param>
        void LocateLine(int lineNumber);
        #endregion

    }
}
