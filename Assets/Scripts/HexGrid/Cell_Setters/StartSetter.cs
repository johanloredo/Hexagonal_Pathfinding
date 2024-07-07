using UnityEngine;

public class StartSetter : MonoBehaviour, ISetCell
{
    //private void Start()
    //{
    //    LevelController.Instance.OnSettingCell += LevelController_OnSettingCell;
    //}
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
        LevelController.Instance.SetCellStart(cell);
    }
}
