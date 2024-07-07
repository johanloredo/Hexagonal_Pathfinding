using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIController : Singleton<UIController>
{
    [Header("Debuggers")]
    //[SerializeField]
    //private TMP_Text startCellText;
    //[SerializeField]
    //private TMP_Text endCellText;

    //[SerializeField]
    //private TMP_Text pathLengthText;
    [SerializeField]
    private GameObject startCellDebugger;
    [SerializeField]
    private GameObject endCellDebugger;

    [SerializeField]
    private GameObject pathLengthDebugger;

    private TMP_Text currentDebugger;

    [Space, Header("Buttons")]
    [SerializeField]
    private Button setStartButton;
    [SerializeField]
    private Button setEndButton;
    [SerializeField]
    private Button setObstaclesButton;
    [SerializeField]
    private Button getPathButton;

    [Space, Header("Cell Setters")]
    [SerializeField]
    private GameObject startSetter;
    [SerializeField]
    private GameObject endSetter;
    [SerializeField]
    private GameObject obstacleSetter;

    private void Start()
    {
        setStartButton.onClick.AddListener(() => SetStart());
        setEndButton.onClick.AddListener(() => SetEnd());
        setObstaclesButton.onClick.AddListener(() => SetObstacle());

        getPathButton.onClick.AddListener(() => GetPath());


        LevelController.Instance.OnSettingCell += LevelController_OnSettingCell;


        SetStart();
        pathLengthDebugger.SetActive(false);
    }

    private void LevelController_OnSettingCell(object sender, LevelController.SetCellArgs e)
    {
        if (currentDebugger != null)
        {
            currentDebugger.text = e.TogglingCell.Position + "\nx: " + e.TogglingCell.IndexX + ", y: " + e.TogglingCell.IndexY;
        }
    }

    private void SetSetters(bool start, bool end, bool obstacle)
    {
        startSetter.SetActive(start);
        endSetter.SetActive(end);
        obstacleSetter.SetActive(obstacle);
    }

    public void SetStart()
    {
        SetSetters(true, false, false);
        currentDebugger = startCellDebugger.transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetEnd()
    {
        SetSetters(false, true, false);
        currentDebugger = endCellDebugger.transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetObstacle()
    {
        SetSetters(false, false, true);
        currentDebugger = null;
    }

    public void GetPath()
    {
        float pathLength = LevelController.Instance.GetPath().Count;
        //pathLengthText.text = pathLength.ToString();

        //if (!pathLengthText.gameObject.activeSelf)
        //{
        //    pathLengthText.gameObject.SetActive(true);
        //}
        pathLengthDebugger.transform.GetChild(1).GetComponent<TMP_Text>().text = pathLength.ToString();

        if (!pathLengthDebugger.activeSelf)
        {
            pathLengthDebugger.SetActive(true);
        }
    }
}
