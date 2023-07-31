using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// life game用のクラス
public class Game : MonoBehaviour
{
    static readonly int X_SIZE = 100;
    static readonly int Y_SIZE = 100;

    static readonly int ALIVE = 1;
    static readonly int DEAD = 0;

    static int[,] cells = new int[X_SIZE, Y_SIZE];

    // セルの実態
    GameObject[,] _cellsObject = new GameObject[X_SIZE, Y_SIZE];

    [SerializeField]
    int UPDATE_INTERVAL = 1;

    [SerializeField]
    GameObject _liveCellPrefab;
    
    [SerializeField]
    GameObject _deadCellPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        for (int x = 0; x < X_SIZE; x++)
        {
            for (int y = 0; y < Y_SIZE; y++)
            {
                cells[x, y] = DEAD;
            }
        }

        // 初期状態
        cells[52, 55] = ALIVE;
        cells[53, 53] = ALIVE;
        cells[53, 55] = ALIVE;
        cells[55, 54] = ALIVE;
        cells[56, 55] = ALIVE;
        cells[57, 55] = ALIVE;
        cells[58, 55] = ALIVE;

        // セルの生成
        drawCells();
    }

    // Update is called once per frame
    void Update()
    {
        // 一定間隔で更新
        if (Time.frameCount % UPDATE_INTERVAL != 0)
        {
            return;
        }

        // セルの状態を更新
        cells = CalculateNextCells(cells);

        // セルの生成
        drawCells();
    }

    // セルの状態を更新するメソッド
    public static int[,] CalculateNextCells(int[,] cells)
    {
        // セルの状態を更新
        int[,] nextCells = new int[X_SIZE, Y_SIZE];
        for (int x = 0; x < X_SIZE; x++)
        {
            for (int y = 0; y < Y_SIZE; y++)
            {
                // 周囲の生存セルを数える
                int aliveCount = 0;
                for (int dx = -1; dx <= 1; dx++)
                {
                    int nx = x + dx;
                    if (nx < 0 || nx >= X_SIZE)
                    {
                        continue;
                    }
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        int ny = y + dy;
                        if (ny < 0 || ny >= Y_SIZE)
                        {
                            continue;
                        }
                        // 自分自身は除く
                        if (dx == 0 && dy == 0)
                        {
                            continue;
                        }
                        if (cells[nx, ny] == ALIVE)
                        {
                            Debug.Log("生存セル: " + dx + ", " + dy);
                            aliveCount++;
                        }
                    }
                }
                if (cells[x, y] == ALIVE)
                {
                    // 生存
                    if (aliveCount == 2 || aliveCount == 3)
                    {
                        Debug.Log("生存: " + aliveCount);
                        nextCells[x, y] = ALIVE;
                    }
                    // 過疎 or 過密
                    else
                    {
                        Debug.Log("過疎 or 過密: " + aliveCount);
                        nextCells[x, y] = DEAD;
                    }
                }
                else
                {
                    // 誕生
                    if (aliveCount == 3)
                    {
                        nextCells[x, y] = ALIVE;
                    }
                    else
                    {
                        nextCells[x, y] = DEAD;
                    }
                }
            }
        }
        return nextCells;
    }

    // セルの状態を描画
    void drawCells()
    {
        // セルの生成
        for (int x = 0; x < X_SIZE; x++)
        {
            for (int y = 0; y < Y_SIZE; y++)
            {
                Destroy(_cellsObject[x, y]);
                _cellsObject[x, y] = Instantiate(cells[x, y] == ALIVE ? _liveCellPrefab : _deadCellPrefab);
                _cellsObject[x, y].transform.localPosition = new Vector3(x, y, 0);
            }
        }
    }
}
