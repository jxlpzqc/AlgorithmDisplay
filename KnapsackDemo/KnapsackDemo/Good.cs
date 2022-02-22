namespace KnapsackDemo
{
    /// <summary>
    /// 代表一个物品
    /// </summary>
    public class Good
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 物品价值
        /// </summary>
        public double Value { get; set; }


        /// <summary>
        /// 物品重量
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// 单位重量价格
        /// </summary>
        public double ValuePerWeight => Value / Weight;

    }
}