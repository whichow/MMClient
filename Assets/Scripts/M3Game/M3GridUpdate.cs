
namespace Game.Match3
{
    public class M3GridUpdate : Singleton<M3GridUpdate>
    {

        public void RunUpdate()
        {
            if (M3GridManager.Instance.dropLock)
                return;

            // 检测掉落口元素为空，生成新元素
            M3Grid grid;
            for (int i = M3Config.GridHeight - 1; i >= 0; i--)
            {
                for (int j = 0; j < M3Config.GridWidth; j++)
                {
                    grid = M3GridManager.Instance.gridCells[i, j];
                    if (grid == null)
                        continue;
                    if (grid.gridInfo.spawnPointType == DropPointType.SpawnPoint)
                    {
                        if (grid.GetItem() == null)
                        {
                            M3DropManager.CreatePieceFromPort(grid);
                        }
                        else if (!grid.GetItem().isCrushing)
                        {
                            grid.portDropSpeed = grid.GetItem().dropSpeed;
                        }
                    }
                }
            }
        }

    }
}