using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIController : Singleton<UIController>
{
    [Header("Debuggers")]
    [SerializeField]
    private GameObject startCellDebugger;
    [SerializeField]
    private GameObject endCellDebugger;

    [SerializeField]
    private GameObject pathLengthDebugger;

    [SerializeField]
    private GameObject noPathDebugger;

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

    [Space, SerializeField]
    private Button resetButton;

    [Space, Header("Cell Setters")]
    [SerializeField]
    private GameObject startSetter;
    [SerializeField]
    private GameObject endSetter;
    [SerializeField]
    private GameObject obstacleSetter;

    private void Awake()
    {
        setStartButton.onClick.AddListener(() => SetStart());
        setEndButton.onClick.AddListener(() => SetEnd());
        setObstaclesButton.onClick.AddListener(() => SetObstacle());

        getPathButton.onClick.AddListener(() => GetPath());

        resetButton.onClick.AddListener(() => ResetGame());

        LevelController.Instance.OnSettingCell += LevelController_OnSettingCell;


        SetStart();
        pathLengthDebugger.SetActive(false);
        noPathDebugger.SetActive(false);
    }

    private void LevelController_OnSettingCell(object sender, LevelController.SetCellArgs e)
    {
        if (currentDebugger != null) currentDebugger.text = e.TogglingCell.IndexX + ", " + e.TogglingCell.IndexY;
    }

    private IEnumerator DisplayNoPathError()
    {
        noPathDebugger.SetActive(true);
        SFXController.Instance.PlayNoPath();

        yield return new WaitForSeconds(2.5f);

        noPathDebugger.SetActive(false);
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
        float pathLength;
        try
        {
            pathLength = LevelController.Instance.GetPath().Count;
        }
        catch
        {
            StartCoroutine(DisplayNoPathError());
            return;
        }

        pathLengthDebugger.transform.GetChild(1).GetComponent<TMP_Text>().text = pathLength.ToString();

        if (!pathLengthDebugger.activeSelf) pathLengthDebugger.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
