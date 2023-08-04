using UnityEngine;
using System;

// セルを描画するクラス
public class CellDrawer
{
    private CellGrid cellGrid;
    private GameObject liveCellPrefab;
    private GameObject deadCellPrefab;
    private GameObject[,] cellObjects;
	private Action<GameObject> destroyAction;
	private Func<GameObject, GameObject> instantiateAction;

	// コンストラクタ
    public CellDrawer(CellGrid cellGrid, GameObject liveCellPrefab, GameObject deadCellPrefab, Action<GameObject> destroyAction, Func<GameObject, GameObject> instantiateAction)
    {
        this.cellGrid = cellGrid;
        this.liveCellPrefab = liveCellPrefab;
        this.deadCellPrefab = deadCellPrefab;
        this.cellObjects = new GameObject[cellGrid.Rows, cellGrid.Columns];
		this.destroyAction = destroyAction;
		this.instantiateAction = instantiateAction;
    }

	// セルを描画する
    public void DrawCells(CellGrid cellGrid)
    {
        for (int x = 0; x < cellGrid.Columns; x++)
        {
            for (int y = 0; y < cellGrid.Rows; y++)
            {
				if (this.cellObjects[x, y] != null)
				{
					this.destroyAction(this.cellObjects[x, y]);
				}
                cellObjects[x, y] = this.instantiateAction(this.cellGrid.Cells[x, y].CurrentState == Cell.State.Alive ? this.liveCellPrefab : this.deadCellPrefab);
                cellGrid.SetCellPosition(cellObjects[x, y], x, y);
            }
        }
    }
}
