using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles the UI when the player is in the menu scene.
/// </summary>
public class MenuUIManager : MonoBehaviour {

    //private enum Navigation { // Not needed ?
    //    Main = 1,
    //    Worlds = 2,
    //    Options = 3
    //}

    [SerializeField] private RectTransform worldSpaceCanvas;
    [SerializeField] private Text starsCountText;
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private Transform worldPanelPrefab;
    [SerializeField] private GameObject mainMenuObj, optionsMenuObj, worldsMenuObj;
    [SerializeField] private Button backButton;

    [Tooltip("Animation played when a button is clicked.")]
    [SerializeField] private AnimationCurve navigationAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float navigationSpeed = 1f;
    [SerializeField] private AnimationCurve backBtnFadeAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float backBtnFadeSpeed = 1f;
    [SerializeField] private float backBtnAnimDelay = 0.75f;

    [Tooltip("Animation played when sliding to a world panel.")]
    [SerializeField] private AnimationCurve slideAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float slideSpeed = 1f;

    [Tooltip("The spacing of the stars horizontalGroup when there are 2 stars to display.")]
    [SerializeField] private float spacingWhenTwo;

    //private Navigation navigation;
    private Transform cameraTransform;
    private bool delayButtonAnimation = true;

    public Transform FirstWorldPanel { get; private set; }
    public Transform LastWorldPanel { get; private set; }
    public int SelectedWorldPanel { get; private set; }
    public int WorldPanelCount {
        get {
            return worldsMenuObj.transform.childCount;
        }
    }

    private RectTransform backBtnRectTransform;
    private Vector2 backButtonOffset;
    private Image backButtonImg;

    public static MenuUIManager Instance { get; private set; }

    private void Awake() {
        #region Singleton
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject);
            return;
        }
        #endregion
    }

    private void Start() {
        cameraTransform = Camera.main.transform;

        backButtonImg = backButton.GetComponent<Image>();
        backBtnRectTransform = backButton.GetComponent<RectTransform>();
        backButtonOffset = backBtnRectTransform.anchoredPosition;

        GeneratePanels();
        FirstWorldPanel = worldsMenuObj.transform.GetChild(0);
        LastWorldPanel = worldsMenuObj.transform.GetChild(worldsMenuObj.transform.childCount - 1);

        UpdateTotalStarsCount(GameManager.TotalStarsCount);
    }

    /// <summary>
    /// Generates the worlds panels and the level buttons for each of them.
    /// </summary>
    private void GeneratePanels() {
        int worldNumber = 1;

        foreach (var world in LevelManager.LevelsInfo.Worlds) {
            Transform worldPanelObj = Instantiate(worldPanelPrefab, worldsMenuObj.transform);
            worldPanelObj.localPosition = new Vector3(worldSpaceCanvas.rect.width * worldNumber, 0f);

            Transform levelPanelObj = worldPanelObj.GetComponentInChildren<GridLayoutGroup>().transform;
            Text worldNameTitle = worldPanelObj.GetComponentInChildren<Text>();

            worldNameTitle.text = "World " + worldNumber;

            for (int i = 0; i < world.LevelCount; i++) {
                LevelButton levelButton = Instantiate(levelButtonPrefab, levelPanelObj);
                levelButton.Level = world[i];

                Text levelText = levelButton.GetComponentInChildren<Text>();
                levelText.text = (i + 1).ToString();

                LevelStats levelStats = world[i].Stats;

                if (levelStats.IsDone) {
                    HorizontalLayoutGroup layoutGroup = levelButton.GetComponentInChildren<HorizontalLayoutGroup>();

                    for (int starImgIndex = 0; starImgIndex < levelStats.StarsCount; starImgIndex++) {
                        layoutGroup.transform.GetChild(starImgIndex).gameObject.SetActive(true);
                    }

                    if (levelStats.StarsCount == 2) {
                        layoutGroup.spacing = spacingWhenTwo;
                    }
                }
            }

            worldNumber++;
        }
    }

    private void UpdateTotalStarsCount(int newCount) {
        if (newCount > 0) {
            starsCountText.text = "x" + newCount;
        } else {
            starsCountText.rectTransform.parent.gameObject.SetActive(false);
        }
    }

    public void DoTransition(Transform to) {
        StartCoroutine(AnimateBackButton(to));
        MoveCameraTo(to);
    }

    public void DoWorldsMenuTransition() {
        StartCoroutine(AnimateBackButton(false, true));
        StartCoroutine(SlideToDelayed());
    }

    private IEnumerator SlideToDelayed() {
        yield return MoveCameraTo(FirstWorldPanel);

        //canSlide = true;
        //TouchManager.
    }

    /// <summary>
    /// Moves the camera, based on the navigation animation curve, to a menu's position.
    /// </summary>
    /// <param name="to">
    /// Menu to lerp towards.
    /// </param>
    /// <returns></returns>
    private Coroutine MoveCameraTo(Transform to) {
        return StartCoroutine(AnimationUtility.MoveToPosition(cameraTransform, to.position, navigationAnimation, navigationSpeed));
    }

    /// <summary>
    /// Fades the back button in or out.
    /// </summary>
    private IEnumerator AnimateBackButton(Transform towardsMenu) {
        if (delayButtonAnimation) {
            delayButtonAnimation = false;

            yield return new WaitForSeconds(backBtnAnimDelay);

            // Sets the back button in the appropriate corner (bottom left, bottom right, top left or top right)
            bool isRightCorner = towardsMenu.position.x <= mainMenuObj.transform.position.x;
            bool isTopCorner = towardsMenu.position.y <= mainMenuObj.transform.position.y;

            Vector2 anchorPosition = new Vector2(isRightCorner ? 1f : 0f, isTopCorner ? 1f : 0f);

            backBtnRectTransform.anchorMin = anchorPosition;
            backBtnRectTransform.anchorMax = anchorPosition;
            backBtnRectTransform.anchoredPosition = new Vector2((isRightCorner ? 1f : -1f ) * backButtonOffset.x,
                (isTopCorner ? 1f : -1f) * backButtonOffset.y);
            backBtnRectTransform.rotation = Quaternion.Euler(0f, isRightCorner ? 0f : 180f, 0f);
        } else {
            delayButtonAnimation = true;
        }

        Color startColor = backButtonImg.color;
        Color endColor = startColor;
        endColor.a = 1f - endColor.a;

        for (float time = 0f; time < 1f; time += backBtnFadeSpeed * Time.deltaTime) {
            backButtonImg.color = Color.Lerp(startColor, endColor, backBtnFadeAnimation.Evaluate(time));
            yield return null;
        }
        backButtonImg.color = endColor;
        backButton.interactable = Mathf.Approximately(endColor.a, 1f);
    }

    private IEnumerator AnimateBackButton(bool isRightCorner, bool isTopCorner) {
        if (delayButtonAnimation) {
            delayButtonAnimation = false;

            yield return new WaitForSeconds(backBtnAnimDelay);

            Vector2 anchorPosition = new Vector2(isRightCorner ? 1f : 0f, isTopCorner ? 1f : 0f);

            backBtnRectTransform.anchorMin = anchorPosition;
            backBtnRectTransform.anchorMax = anchorPosition;
            backBtnRectTransform.anchoredPosition = new Vector2((isRightCorner ? 1f : -1f) * backButtonOffset.x,
                (isTopCorner ? 1f : -1f) * backButtonOffset.y);
            backBtnRectTransform.rotation = Quaternion.Euler(0f, isRightCorner ? 0f : 180f, 0f);
        } else {
            delayButtonAnimation = true;
        }

        Color startColor = backButtonImg.color;
        Color endColor = startColor;
        endColor.a = 1f - endColor.a;

        for (float time = 0f; time < 1f; time += backBtnFadeSpeed * Time.deltaTime) {
            backButtonImg.color = Color.Lerp(startColor, endColor, backBtnFadeAnimation.Evaluate(time));
            yield return null;
        }
        backButtonImg.color = endColor;
        backButton.interactable = Mathf.Approximately(endColor.a, 1f);
    }
    
    public Coroutine DoTouchSlide(TouchManager.SlideDirection direction) {
        SelectedWorldPanel = direction == TouchManager.SlideDirection.Left ? SelectedWorldPanel + 1 : SelectedWorldPanel - 1;

        Vector2 endPosition = worldsMenuObj.transform.GetChild(SelectedWorldPanel).position;

        return StartCoroutine(AnimationUtility.MoveToPosition(cameraTransform, endPosition, slideAnimation, slideSpeed));

        //Debug.Log(SelectedWorldPanel);
        //Transform worldsMenu = worldsMenuObj.transform;
        //Transform previousPanel = worldsMenu.GetChild(SelectedWorldPanel);
        //Transform nextPanel;

        //if (direction == TouchManager.SlideDirection.Left) {
        //    nextPanel = worldsMenu.GetChild(SelectedWorldPanel < worldsMenu.childCount - 1 ? SelectedWorldPanel + 1 : SelectedWorldPanel);
        //} else {
        //    nextPanel = worldsMenu.GetChild(SelectedWorldPanel > 0 ? SelectedWorldPanel - 1 : SelectedWorldPanel);
        //}

        //Vector3 startPosition = cameraTransform.position;

        //if (previousPanel == nextPanel) {
        //    StartCoroutine(AnimationUtility.MoveToPosition(cameraTransform, previousPanel.position, slideAnimation, slideSpeed));
        //    Debug.Log(SelectedWorldPanel + " -> equal panels");
        //    return;
        //}

        //float distanceToPrevious = Mathf.Abs(previousPanel.position.x - startPosition.x);
        //float distanceToNext = Mathf.Abs(nextPanel.position.x - startPosition.x);

        //Vector2 endPosition;

        //if (distanceToNext < distanceToPrevious) {
        //    endPosition = nextPanel.position;
        //    SelectedWorldPanel++;
        //} else {
        //    endPosition = previousPanel.position;
        //    SelectedWorldPanel--;
        //}
        //Debug.Log(SelectedWorldPanel);
        //StartCoroutine(AnimationUtility.MoveToPosition(cameraTransform, endPosition, slideAnimation, slideSpeed));
    }
}
