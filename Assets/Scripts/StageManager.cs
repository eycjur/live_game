using System.Collections.Generic;
using System.IO;
using UnityEngine;

// ステージの設定を管理するクラス
public class StageManager
{
    private List<StageConfig> stageConfigs;
    private StageConfig selectedStage;
    private float previousTime;

    public StageManager(string jsonPath)
    {
        LoadStageConfigs(jsonPath);
    }

    // JSONファイルからステージの設定を読み込む
    private void LoadStageConfigs(string jsonPath)
    {
        string jsonContents = File.ReadAllText(jsonPath);
        StageConfigWrapper stageConfigWrapper = JsonUtility.FromJson<StageConfigWrapper>(jsonContents);
        Debug.Log("success to load stage configs: " + jsonPath);
        this.stageConfigs = stageConfigWrapper.stages;
    }

    // ステージを設定する
    public void SetStage(int stageIndex, CellGrid cellGrid)
    {
        this.previousTime = Time.time;
        this.selectedStage = this.stageConfigs[stageIndex];

        // 全てのセルを死亡状態で初期化する
        cellGrid.InitializeCells(Cell.State.Dead);

        // 生存セルの状態を設定する
        foreach (CellConfig cellConfig in this.selectedStage.cells)
        {
            cellGrid.SetInitialCellState(cellConfig.x, cellConfig.y, Cell.State.Alive);
        }
    }

    // 状態を更新する
    public void UpdateState(CellGrid cellGrid)
    {
        // 指定された時間が経過していない場合は何もしない
        if (Time.time - this.previousTime < this.selectedStage.updateTimeInterval)
        {
            return;
        }

        this.previousTime = Time.time;
        Debug.Log("update state");

        cellGrid.CalculateNextCells(this.selectedStage);
    }

    // ステージの数を取得する
    public int StageCount
    {
        get { return this.stageConfigs.Count; }
    }

    // JSONの内容を読み込むためのクラス
    [System.Serializable]
    private class StageConfigWrapper
    {
        public List<StageConfig> stages;
    }

    [System.Serializable]
    public class StageConfig
    {
        public string name;
        public float updateTimeInterval;
        public List<int> aliveConditions;
        public List<int> birthConditions;
        public List<CellConfig> cells;
    }

    [System.Serializable]
    public class CellConfig
    {
        public int x;
        public int y;
    }
}
