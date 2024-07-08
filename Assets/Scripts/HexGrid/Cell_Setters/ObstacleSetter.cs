using UnityEngine;

public class ObstacleSetter : MonoBehaviour, ISetCell
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
        LevelController.Instance.SetObstacle(cell);
    }
}
