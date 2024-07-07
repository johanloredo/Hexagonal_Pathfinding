using UnityEngine;

public class EndSetter : MonoBehaviour, ISetCell
{
    private void OnEnable()
    {
        LevelController.Instance.OnSettingCell += LevelController_OnSettingCell;
    }

    private void OnDisable()
    {
        LevelController.Instance.OnSettingCell -= LevelController_OnSettingCell;
    }

    private void LevelController_OnSettingCell(object sender, LevelController.SetCellArgs e)
    {
        SetCell(e.TogglingCell);
    }

    public void SetCell(ICell cell)
    {
        LevelController.Instance.SetCellEnd(cell);
    }
}
