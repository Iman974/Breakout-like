using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles the UI when the player is in the menu scene.
/// </summary>
public class MenuUIManager : MonoBehaviour {

    private enum Navigation {
        Main = 1,
        Worlds = 2,
        Options = 3
    }

    [SerializeField] private RectTransform worldSpaceCanvas;
    [SerializeField] private Text starsCountText;
    [SerializeField] private LevelButton levelButtonPrefab;
    [SerializeField] private Transform worldPanelPrefab;
    [SerializeField] private GameObject backButtonObj, mainMenuObj, optionsMenuObj, worldsMenuObj;
    [SerializeField] private AnimationCurve navigationAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve backBtnRevealAnimation = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private float navigationSpeed = 1f;
    [SerializeField] private float backBtnRevealSpeed = 2f;
    [SerializeField] private float backBtnAnimDelay = 0.75f;

    [Tooltip("The spacing of the stars horizontalGroup when there are 2 stars to display.")]
    [SerializeField] private float spacingWhenTwo;

    private Navigation navigation;
    private Camera mainCamera;
    private bool delayButtonAnimation = true;
    private Transform firstWorldPanel;

#if UNITY_STANDALONE
    private RectTransform backButtonRectTransform;
#else
    private Image backButtonImg;
#endif

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
        mainCamera = Camera.main;

#if UNITY_STANDALONE
        backButtonRectTransform = backButtonObj.GetComponent<RectTransform>();
#else
        //backButtonImg = 
#endif
        GeneratePanels();

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

        firstWorldPanel = worldsMenuObj.transform.GetChild(0);
    }

    //private void Update() {
    //    //Debug.Log("Width: " + Camera.main.pixelWidth + ", Height: " + Camera.main.pixelHeight + ", Scaled width: " +
    //    //    Camera.main.scaledPixelWidth + ", Scaled height: " + Camera.main.scaledPixelHeight + ", Aspect: " + Camera.main.aspect);
    //}

    private void UpdateTotalStarsCount(int newCount) {
        if (newCount > 0) {
            starsCountText.text = "x" + newCount;
        } else {
            starsCountText.rectTransform.parent.gameObject.SetActive(false);
        }
    }

    //public void GoBack() {
    //    if (worldsMenuObj.activeSelf) {
    //        navigation = Navigation.Worlds;
    //    } else if (optionsMenuObj.activeSelf) {
    //        navigation = Navigation.Options;
    //    }

    //    switch (navigation) {
    //        case Navigation.Worlds:
    //            //worldsMenuObj.SetActive(false);
    //            //backButtonObj.SetActive(false);
    //            //mainMenuObj.SetActive(true);
    //            //StartCoroutine(TransitionTowards(mainMenuObj.transform, worldsMenuObj, backButtonObj));
    //            break;
    //        case Navigation.Options:
    //            //optionsMenuObj.SetActive(false);
    //            //backButtonObj.SetActive(false);
    //            //mainMenuObj.SetActive(true);
    //            //StartCoroutine(TransitionTowards(mainMenuObj.transform, optionsMenuObj, backButtonObj));
    //            break;
    //    }
    //}

    public void DoTransition(Transform towardsMenu) {
        StartCoroutine(TransitionTowards(towardsMenu));
    }

    public void DoWorldsMenuTransition() {
        StartCoroutine(TransitionTowards(firstWorldPanel));
    }

    /// <summary>
    /// Moves the camera smoothly, based on the navigation animation curve, towards a menu's position.
    /// </summary>
    /// <param name="towardsMenu">
    /// Menu to lerp towards.
    /// </param>
    /// <returns></returns>
    private IEnumerator TransitionTowards(Transform towardsMenu) {
        Vector3 startPosition = mainCamera.transform.position;
        Vector2 endPosition = towardsMenu.position;

        StartCoroutine(AnimateBackButton());

        for (float time = 0f; time < 1f; time += navigationSpeed * Time.deltaTime) {
            mainCamera.transform.position = VectorUtility.BuildVector(Vector2.LerpUnclamped(startPosition, endPosition,
                navigationAnimation.Evaluate(time)), startPosition.z);
            yield return null;
        }
        mainCamera.transform.position = endPosition;
    }

    /// <summary>
    /// Moves the back button out of the screen or into it.
    /// </summary>
    private IEnumerator AnimateBackButton(/*bool isReveal = true*/) {
        if (delayButtonAnimation) {
            delayButtonAnimation = false;
            yield return new WaitForSeconds(backBtnAnimDelay);
        } else {
            delayButtonAnimation = true;
        }

#if UNITY_STANDALONE
        Vector2 startPosition = backButtonRectTransform.anchoredPosition;
        Vector2 endPosition = -startPosition;
        //Vector3[] fourCorners = new Vector3[4];

        for (float time = 0f; time < 1f; time += backBtnRevealSpeed * Time.deltaTime) {
            backButtonRectTransform.anchoredPosition = Vector2.LerpUnclamped(startPosition, endPosition,
                backBtnRevealAnimation.Evaluate(time));

            //backButtonRectTransform.GetWorldCorners(fourCorners);
            //if (fourCorners[1].y < mainCamera.ViewportToWorldPoint(Vector3.zero).y) {
            //    break;
            //}
            yield return null;
        }
        backButtonRectTransform.anchoredPosition = endPosition;

#else
        for (int i = 0; i < length; i++) {
            
        }
#endif
    }

    private void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Debug.Log("Touch ! " + touch.phase);
        }
    }

    ///// <summary>
    ///// Same as TransitionTowards(), but disables the given gameObjects at the end of the animation.
    ///// </summary>
    ///// <param name="towardsMenu">
    ///// Menu to lerp towards.
    ///// </param>
    ///// <param name="toDisable">
    ///// Gameobjects to disable at the end.
    ///// </param>
    ///// <returns></returns>
    //private IEnumerator TransitionTowards(Transform towardsMenu, params GameObject[] toDisable) {
    //    yield return StartCoroutine(TransitionTowards(towardsMenu));

    //    foreach (GameObject gameObject in toDisable) {
    //        gameObject.SetActive(false);
    //    }
    //}
}
