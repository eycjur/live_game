using UnityEngine;
using UnityEngine.UI;

// ボタンの表示や挙動を管理するクラス
public class StageButtonController
{
    private StageIndexManager stageIndexManager;
    private Button previousButton;
    private Button nextButton;

    public StageButtonController(StageIndexManager stageIndexManager, Button previousButton, Button nextButton)
    {
        this.stageIndexManager = stageIndexManager;
        this.previousButton = previousButton;
        this.nextButton = nextButton;
        UpdateButtonVisibility();
    }
    
    public void NextStage()
    {
        Debug.Log("NextStage");
        this.stageIndexManager.MoveToNextStage();
        UpdateButtonVisibility();
    }

    public void PreviousStage()
    {
        Debug.Log("PreviousStage");
        this.stageIndexManager.MoveToPreviousStage();
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        this.previousButton.gameObject.SetActive(this.stageIndexManager.CanMoveToPreviousStage());
        this.nextButton.gameObject.SetActive(this.stageIndexManager.CanMoveToNextStage());
    }
}