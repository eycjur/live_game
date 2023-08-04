using UnityEngine;
using UnityEngine.UI;

// ゲームの進行を管理する
public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject liveCellPrefab;

    [SerializeField]
    private GameObject deadCellPrefab;

    [SerializeField]
    private Button previousButton;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private StageIndexManager stageIndexManager;

    [SerializeField]
    private StageButtonController stageButtonController;
    
    private StageManager stageManager;
    private CellGrid cellGrid;
    private CellDrawer cellDrawer;

    void Start()
    {
        this.stageManager = new StageManager("stage_config.json");
        this.cellGrid = new CellGrid(30, 30);
        this.cellDrawer = new CellDrawer(this.cellGrid, this.liveCellPrefab, this.deadCellPrefab, Destroy, Instantiate);

        // monoBehaviourの初期化（インスタンス化はできないため、クラス変数のまま利用）
        stageIndexManager.Initialize(this.stageManager.StageCount, 0);
        this.stageButtonController = new StageButtonController(stageIndexManager, previousButton, nextButton);
        this.stageManager.SetStage(stageIndexManager.GetCurrentStageIndex(), cellGrid);
        this.cellDrawer.DrawCells(this.cellGrid);

        // Add listeners for the button click events
        previousButton.onClick.AddListener(PreviousButtonClick);
        nextButton.onClick.AddListener(NextButtonClick);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックを検知
        {
            Debug.Log("Left Click");
            ToggleCellState();
        }

        this.stageManager.UpdateState(this.cellGrid);
        this.cellDrawer.DrawCells(this.cellGrid);
    }

    void PreviousButtonClick()
    {
        Debug.Log("PreviousButtonClick");
        this.stageButtonController.PreviousStage();
        this.stageManager.SetStage(stageIndexManager.GetCurrentStageIndex(), this.cellGrid);
        this.cellDrawer.DrawCells(this.cellGrid);
    }

    void NextButtonClick()
    {
        Debug.Log("NextButtonClick");
        this.stageButtonController.NextStage();
        this.stageManager.SetStage(stageIndexManager.GetCurrentStageIndex(), this.cellGrid);
        this.cellDrawer.DrawCells(this.cellGrid);
    }

    // セルの状態を反転させる
    private void ToggleCellState()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        (int x, int y) = this.cellGrid.PositionToCoordinate(mouseWorldPosition);

        // グリッド内のクリックを確認
        if (x >= 0 && x < this.cellGrid.Rows && y >= 0 && y < this.cellGrid.Columns)
        {
            Cell cell = this.cellGrid.Cells[x, y];
            cell.SetState(cell.CurrentState == Cell.State.Alive ? Cell.State.Dead : Cell.State.Alive);
        }
    }
}
