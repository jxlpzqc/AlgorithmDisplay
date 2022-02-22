void dfs(int i, int tw, int tv, int op[])
{
	//初始调用时rw为所有物品的重量和 
	if (i > n)   //找到一个叶子结点
	{
		if (tv > maxv)  //找到一个最优解
		{
			maxv = tv;
			for (int j = 1; j <= n; j++)  //复制最优解 
				x[j] = op[j];
		}
	}
	else   //未找完所有物品 
	{
		if (tw + rw > W)   //剪枝 
			dfs(i + 1, tw, tv, op);

		if (tw + w[i] <= W) //剪枝 
		{
			op[i] = 1;  //选取第i个物品
			dfs(i + 1, tw + w[i], tv + v[i], rw - w[i], op);
		}
		op[i] = 0;   //不选取物品i，回溯
	}
}