using UnityEngine;

// 現在のステージのインデックスを管理するクラス
public class StageIndexManager : MonoBehaviour
{
    private int currentStageIndex = 0;
    private int totalStages;

    // monoBehaviourはインスタンス化できない
    public void Initialize(int totalStages, int initialStageIndex)
    {
        this.totalStages = totalStages;
        SetCurrentStageIndex(initialStageIndex);
    }

    public void SetCurrentStageIndex(int index)
    {
		Debug.Log("SetCurrentStageIndex: " + index);
        this.currentStageIndex = Mathf.Clamp(index, 0, totalStages - 1);
    }

    public int GetCurrentStageIndex()
    {
        return this.currentStageIndex;
    }

    public bool CanMoveToNextStage()
    {
        return this.currentStageIndex < totalStages - 1;
    }

    public bool CanMoveToPreviousStage()
    {
        return this.currentStageIndex > 0;
    }

    public void MoveToNextStage()
    {
        if (CanMoveToNextStage())
        {
			Debug.Log("MoveToNextStage: " + this.currentStageIndex);
            this.currentStageIndex++;
        }
		else
		{
			Debug.Log("Fail to MoveToNextStage: " + this.currentStageIndex);
		}
    }

    public void MoveToPreviousStage()
    {
        if (CanMoveToPreviousStage())
        {
			Debug.Log("MoveToPreviousStage: " + this.currentStageIndex);
            this.currentStageIndex--;
        }
		else
		{
			Debug.Log("Fail to MoveToPreviousStage: " + this.currentStageIndex);
		}
    }
}
