// 一つ一つのセルの状態を管理するクラス
public class Cell
{
    public enum State { Dead, Alive }

    // セルの状態
    public State CurrentState { get; private set; }

    // コンストラクタ
    public Cell(State initialState)
    {
        this.CurrentState = initialState;
    }

    public void SetState(State state)
    {
        this.CurrentState = state;
    }
}
