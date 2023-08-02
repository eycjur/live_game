using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// public class StageConfig
// {
//     public string name;
//     public List<CellConfig> cells;
// }

// public class CellConfig
// {
//     public int x;
//     public int y;
// }

// life game用のクラス
public class Game : MonoBehaviour
{
    static readonly int X_SIZE = 30;
    static readonly int Y_SIZE = 30;

    static readonly int ALIVE = 1;
    static readonly int DEAD = 0;

    static int[,] cells = new int[X_SIZE, Y_SIZE];

    string stageName;
    float privious_time;
    List<StageConfig> stageConfigs;
    StageConfig selectedStage;

    // セルの実態
    GameObject[,] _cellsObject = new GameObject[X_SIZE, Y_SIZE];

    [SerializeField]
    GameObject _liveCellPrefab;
    
    [SerializeField]
    GameObject _deadCellPrefab;

    [System.Serializable]
    private class StageConfigWrapper
    {
        public List<StageConfig> stages;
    }

    [System.Serializable]
    private class StageConfig
    {
        public string name;
        public float updateTimeInterval;
        public List<int> aliveConditions;
        public List<int> birthConditions;
        public List<CellConfig> cells;
    }

    [System.Serializable]
    private class CellConfig
    {
        public int x;
        public int y;
    }

    // Start is called before the first frame update
    void Start()
    {
        string stageName = "Stage1";

        string jsonPath = "Assets/Configs/stage_config.json";
        string jsonContents = File.ReadAllText(jsonPath);
        StageConfigWrapper stageConfigWrapper = JsonUtility.FromJson<StageConfigWrapper>(jsonContents);
        stageConfigs = stageConfigWrapper.stages;

        SetStage(stageName);
    }

    void SetStage(string stageName)
    {
        float privious_time = Time.time;

        // ステージの設定
        selectedStage = stageConfigs.Find(stage => stage.name == stageName);
        if (selectedStage == null)
        {
            Debug.LogError($"Stage with name '{stageName}' not found in the JSON file.");
            return;
        }

        // 初期化
        for (int x = 0; x < X_SIZE; x++)
        {
            for (int y = 0; y < Y_SIZE; y++)
            {
                cells[x, y] = DEAD;
            }
        }

        // 初期状態
        foreach (CellConfig cellConfig in selectedStage.cells)
        {
            int x = cellConfig.x;
            int y = cellConfig.y;
            if (x >= 0 && x < X_SIZE && y >= 0 && y < Y_SIZE)
            {
                cells[x, y] = ALIVE;
            }
        }
        
        // セルの生成
        drawCells();
    }

    // Update is called once per frame
    void Update()
    {
        // 一定間隔で更新
        if (Time.time - privious_time < selectedStage.updateTimeInterval)
        {
            return;
        }
        privious_time = Time.time;

        // セルの状態を更新
        cells = CalculateNextCells(cells);

        // セルの生成
        drawCells();
    }

    // セルの状態を更新するメソッド
    public int[,] CalculateNextCells(int[,] cells)
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
                            aliveCount++;
                        }
                    }
                }
                if (cells[x, y] == ALIVE)
                {
                    // 生存
                    if (selectedStage.aliveConditions.Contains(aliveCount))
                    {
                        nextCells[x, y] = ALIVE;
                    }
                    // 過疎 or 過密
                    else
                    {
                        nextCells[x, y] = DEAD;
                    }
                }
                else
                {
                    // 誕生
                    if (selectedStage.birthConditions.Contains(aliveCount))
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
                _cellsObject[x, y].transform.localPosition = new Vector3(x - X_SIZE / 2, y - Y_SIZE / 2, 0);
            }
        }
    }
}
