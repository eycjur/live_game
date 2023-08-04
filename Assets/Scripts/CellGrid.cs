using UnityEngine;

// セルのグリッドを管理するクラス
public class CellGrid
{
    private float botom_left_x = 0; // 左下のx座標
    private float botom_left_y = 350; // 左下のy座標

    // セルのスケール
    private float cellScale;

    public int Rows { get; }
    public int Columns { get; }
    public Cell[,] Cells { get; private set; }

    // コンストラクタ
    public CellGrid(int rows, int columns)
    {
        this.Rows = rows;
        this.Columns = columns;
        this.Cells = new Cell[rows, columns];
        // 画面上の横幅をセルの数で割る
        // 横幅はカメラサイズ * 9 /16 * 2
        this.cellScale = Screen.width / Columns;
        InitializeCells(Cell.State.Dead);
    }

    public void SetCellPosition(GameObject cellObject, int x, int y)
    {
        // セルの位置を計算して設定
        Vector3 coordinate = CoordinateToPosition(x, y);
        cellObject.transform.localPosition = new Vector3(coordinate.x, coordinate.y, 0);
        cellObject.transform.localScale = new Vector3(this.cellScale, this.cellScale, 1);
    }

    // 全てのセルを初期化する
    public void InitializeCells(Cell.State initialState)
    {
        for (int x = 0; x < this.Rows; x++)
        {
            for (int y = 0; y < this.Columns; y++)
            {
                this.Cells[x, y] = new Cell(initialState);
            }
        }
    }

    // 指定された座標のセルの状態を設定する
    public void SetInitialCellState(int x, int y, Cell.State state)
    {
        if (x >= 0 && x < this.Rows && y >= 0 && y < this.Columns)
        {
            Cells[x, y].SetState(state);
        }
    }

    // 指定された座標のセルの次の状態を計算する
    public void CalculateNextCells(StageManager.StageConfig stageConfig)
    {
        Cell[,] nextCells = new Cell[this.Rows, this.Columns];
        for (int x = 0; x < this.Rows; x++)
        {
            for (int y = 0; y < this.Columns; y++)
            {
                int aliveCount = CountAliveNeighbors(x, y);
                Cell.State nextState = DetermineNextCellState(Cells[x, y].CurrentState, aliveCount, stageConfig);
                nextCells[x, y] = new Cell(nextState);
            }
        }
        this.Cells = nextCells;
    }

    // 指定された座標のセルの周囲の生きているセルの数を数える
    private int CountAliveNeighbors(int x, int y)
    {
        int aliveCount = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            int nx = x + dx;
            if (nx < 0 || nx >= this.Rows) continue;

            for (int dy = -1; dy <= 1; dy++)
            {
                int ny = y + dy;
                if (ny < 0 || ny >= this.Columns || (dx == 0 && dy == 0)) continue;
                if (this.Cells[nx, ny].CurrentState == Cell.State.Alive) aliveCount++;
            }
        }
        return aliveCount;
    }

    // 指定されたセルの次の状態を決定する
    private Cell.State DetermineNextCellState(Cell.State currentCell, int aliveCount, StageManager.StageConfig stageConfig)
    {
        if (currentCell == Cell.State.Alive)
        {
            return stageConfig.aliveConditions.Contains(aliveCount) ? Cell.State.Alive : Cell.State.Dead;
        }
        else
        {
            return stageConfig.birthConditions.Contains(aliveCount) ? Cell.State.Alive : Cell.State.Dead;
        }
    }

    // セルの格子座標から位置を計算する
    public Vector3 CoordinateToPosition(int x, int y)
    {
        // localPositionは中心なので、半分ずらす
        float x_position = x * cellScale + botom_left_x + cellScale / 2;
        float y_position = y * cellScale + botom_left_y + cellScale / 2;
        return new Vector3(x_position, y_position, 0);
    }

    // 位置からセルの格子座標を計算する
    public (int, int) PositionToCoordinate(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x - botom_left_x) / cellScale);
        int y = Mathf.FloorToInt((position.y - botom_left_y) / cellScale);
        return (x, y);
    }
}
