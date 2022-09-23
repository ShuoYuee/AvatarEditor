
using UnityEngine.EventSystems;

public class ListBankBruh : ListBankBase, IMoveHandler
{
    public ListPositionCtrl _circularList;

    private void Start()
    {
        _circularList.onCenteredBoxChanged += OnCenteredBoxChanged;
        _circularList.onCenteredBoxExit += OnCenteredBoxExit;
    }

    private void OnDestroy()
    {
        _circularList.onCenteredBoxChanged -= OnCenteredBoxChanged;
        _circularList.onCenteredBoxExit -= OnCenteredBoxExit;
    }

    public override object GetListContent(int index)
    {
        return null;
    }

    public override int GetListLength()
    {
        return _circularList.listBoxes.Length;
    }

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Right)
        {
            _circularList.MoveOneUnitDown();
        }
        else if (eventData.moveDir == MoveDirection.Left)
        {
            _circularList.MoveOneUnitUp();
        }
    }

    private void OnCenteredBoxChanged(ListBoxBase enterBox)
    {
        //((ListItem)enterBox).m_Highlight.enabled = true;
    }

    private void OnCenteredBoxExit(ListBoxBase exitBox)
    {
        //((ListItem)exitBox).m_Highlight.enabled = false;
    }
}