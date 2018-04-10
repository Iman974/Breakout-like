using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

    public enum SlideDirection {
        Left = -1,
        Right = 1
    }

    [Tooltip("The minimum touch slide that is required to trigger the worldsMenu's sliding")]
    [SerializeField] private float minSlideThreshold = 1f;

    private SlideDirection slideDirection;
    private bool canSlide = true;
    private Transform cameraTransform;
    private MenuUIManager menuUIManager;

    void Start() {
        cameraTransform = Camera.main.transform;
        menuUIManager = MenuUIManager.Instance;
    }

    void Update() {
        if (Input.touchCount < 1 || !canSlide) {
            return;
        }

        Touch touch = Input.GetTouch(0);
        //if () {
        //    if (/*!Mathf.Approximately(touch.deltaPosition.x, 0f) &&*/true) {
        //        float x = Mathf.Clamp(cameraTransform.position.x - touch.deltaPosition.x * touch.deltaTime,
        //            menuUIManager.FirstWorldPanel.position.x, menuUIManager.LastWorldPanel.position.x);
        //        cameraTransform.position = new Vector3(x, 0f);
        //        slideDirection = (SlideDirection)(touch.deltaPosition.x > 0f ? 1 : -1);
        //    }
        /*}*/
        if (touch.phase == TouchPhase.Moved && Mathf.Abs(touch.deltaPosition.x) > minSlideThreshold) {
            slideDirection = (SlideDirection)(touch.deltaPosition.x > 0f ? 1 : -1);

            if (slideDirection == SlideDirection.Left) {
                if (menuUIManager.SelectedWorldPanel >= menuUIManager.WorldPanelCount - 1) {
                    return;
                }
            } else if (menuUIManager.SelectedWorldPanel <= 0) {
                return;
            }

            StartCoroutine(WaitForAnimationEnding(menuUIManager.DoTouchSlide(slideDirection)));
            canSlide = false;
        }
    }

    private IEnumerator WaitForAnimationEnding(Coroutine animation) {
        yield return animation;

        canSlide = true;
    }

    //private bool IsCameraOnEdges() {
    //    return Mathf.Approximately(cameraTransform.position.x, menuUIManager.FirstWorldPanel.position.x) ||
    //           Mathf.Approximately(cameraTransform.position.x, menuUIManager.LastWorldPanel.position.x);
    //}
}
