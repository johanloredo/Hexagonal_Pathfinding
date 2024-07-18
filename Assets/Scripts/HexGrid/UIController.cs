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

    private Color red = new Color(0.9f, 0.5f, 0.4f);
    private Color yellow = new Color(0.95f, 0.8f, 0.5f);
    private Color blue = new Color(0.18f, 0.22f, 0.32f);

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
        //setStartButton.image.color = red;
        setEndButton.onClick.AddListener(() => SetEnd());
        //setEndButton.image.color = red;
        setObstaclesButton.onClick.AddListener(() => SetObstacle());
        //setObstaclesButton.image.color = red;

        getPathButton.onClick.AddListener(() => GetPath());
        getPathButton.GetComponent<Image>().color = yellow;

        resetButton.onClick.AddListener(() => ResetGame());
        resetButton.image.color = red;

        LevelController.Instance.OnSettingCell += LevelController_OnSettingCell;


        SetStart();
        noPathDebugger.SetActive(false);

        pathLengthDebugger.GetComponent<Image>().color = yellow;
        pathLengthDebugger.SetActive(false);
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

        setStartButton.image.color = start ? blue : red;
        setEndButton.image.color = end ? blue : red;
        setObstaclesButton.image.color = obstacle ? blue : red;
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
