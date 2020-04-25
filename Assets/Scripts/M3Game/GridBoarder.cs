/** 
*FileName:     GridBoarder.cs 
*Author:       Hejunjie 
*Version:      1.0 
*UnityVersion：5.6.2f1
*Date:         2017-11-29 
*Description:    
*History: 
*/
using UnityEngine;

namespace Game.Match3
{
    public class GridBoarder
    {
        private string[] pic_rotate = new string[]
        {
            "border_t04",
            "border_t03",
            "border_t02",
            "border_t01"
        };

        private string[] pic_straight = new string[]
        {
            "board_edge7",
            "board_edge8",
            "board_edge6",
            "board_edge5",
            "board_edge4",
            "board_edge3",
            "board_edge2",
            "board_edge1",
        };

        private int[] outer_rotate_offset_x = new int[]
        {
            -1,
            1,
            1,
            -1
        };

        private int[] outer_rotate_offset_y = new int[]
        {
            1,
            1,
            -1,
            -1
        };

        private int[] outer_straight_offset_x = new int[]
        {
            -1,
            -1,
            -1,
            1,
            1,
            1,
            1,
            -1
        };

        private int[] outer_straight_offset_y = new int[]
        {
            -1,
            1,
            1,
            1,
            1,
            -1,
            -1,
            -1
        };


        public void InitBorders()
        {
            for (int i = 0; i < M3Config.GridWidth; i++)
            {
                for (int j = 0; j < M3Config.GridHeight; j++)
                {
                    bool[] array = new bool[4];
                    for (int k = 0; k < 4; k++)
                    {
                        string rotateForPositionAndDirection = this.GetRotateForPositionAndDirection(j, i, k);
                        this.CreateBoarder(rotateForPositionAndDirection, M3Supporter.Instance.GetItemPositionByGrid(j, i));
                        array[k] = (rotateForPositionAndDirection != string.Empty);
                    }
                    for (int l = 0; l < 8; l++)
                    {
                        if (!array[(l != 0) ? ((l - 1) / 2) : 3])
                        {
                            string straightForPositionAndDirection = this.GetStraightForPositionAndDirection(j, i, l);
                            this.CreateBoarder(straightForPositionAndDirection, M3Supporter.Instance.GetItemPositionByGrid(j, i));
                        }
                    }
                }
            }
        }

        private string GetRotateForPositionAndDirection(int x, int y, int direciton)
        {
            string result = string.Empty;
            if (this.IsBlankForPosition(x, y))
            {
                result = this.CheckInnerRotate(x, y, direciton);
            }
            else
            {
                result = this.CheckOuterRotate(x, y, direciton);
            }
            return result;
        }

        private string GetStraightForPositionAndDirection(int x, int y, int direciton)
        {
            string result = string.Empty;
            if (!this.IsBlankForPosition(x, y))
            {
                result = this.CheckOuterStraight(x, y, direciton);
            }
            return result;
        }

        private string CheckOuterRotate(int x, int y, int direction)
        {
            if (this.IsBlankForPositionAndDirection(x, y, direction) && this.IsBlankForPositionAndDirection(x, y, (direction != 3) ? (direction + 1) : 0) && this.IsBlankForPosition(x + this.outer_rotate_offset_x[direction], y + this.outer_rotate_offset_y[direction]))
            {
                return this.pic_rotate[direction] + "_o";
            }
            return string.Empty;
        }

        private string CheckInnerRotate(int x, int y, int direction)
        {
            if (!this.IsBlankForPositionAndDirection(x, y, direction) && !this.IsBlankForPositionAndDirection(x, y, (direction != 3) ? (direction + 1) : 0))
            {
                return this.pic_rotate[direction] + "_i";
            }
            return string.Empty;
        }

        private string CheckOuterStraight(int x, int y, int direction)
        {
            int direction2 = direction / 2;
            if (this.IsBlankForPositionAndDirection(x, y, direction2) && this.IsBlankForPosition(x + this.outer_straight_offset_x[direction], y + this.outer_straight_offset_y[direction]))
            {
                return this.pic_straight[direction] + "_o";
            }
            return string.Empty;
        }

        private bool IsBlankForPositionAndDirection(int x, int y, int direction)
        {
            x = ((direction % 2 != 1) ? (x + direction - 1) : x);
            y = ((direction % 2 != 0) ? (y + 2 - direction) : y);
            return this.IsBlankForPosition(x, y);
        }

        private bool IsBlankForPosition(int x, int y)
        {
            return x < 0 || x > M3Config.GridHeight - 1 || y < 0 || y > M3Config.GridWidth - 1 || (M3GridManager.Instance.gridCells[x, y] == null);
        }

        private void CreateBoarder(string name, Vector3 position)
        {
            if (name == string.Empty)
            {
                return;
            }
            GameObject gameObject;
            if (KAssetManager.Instance.TryGetMatchPrefab(name, out gameObject))
            {
                gameObject = GameObject.Instantiate(gameObject);
                gameObject.transform.SetParent(M3GridManager.Instance.borderParent.transform);
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = position;
            }
        }

    }
}