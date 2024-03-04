using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraceUIMobile : MonoBehaviour
{
    public Image targetImageRight;
    public Image targetImageLeft;

    public AudioSource audioSource;

    private Vector2 previousInputPosition;

    private int clockwiseCounter = 0;
    private int counterClockwiseCounter = 0;

    private Color targetColorRight;
    private Color targetColorLeft;

    private bool isRightPos = true, 
                 isRightNeg = true;

    private bool isLeftPos = true, 
                 isLeftNeg = true;

    [SerializeField] private float colorChangeSpeed = 0.05f;

    [SerializeField] private TextMeshProUGUI debugText;

    public Color CurrentColorRight { get; private set; }
    public Color CurrentColorLeft { get; private set; }

    public Color[] colorPalette;

    private bool isDragging = false;

    void Start()
    {
        targetColorRight = targetImageRight.color;
        targetColorLeft = targetImageLeft.color;
    }

    void Update()
    {
        if (!isDragging && audioSource.isPlaying)
        {
            debugText.text = "Music Stop";
            audioSource.Stop();
        }

        CurrentColorRight = Color.Lerp(targetImageRight.color, targetColorRight, colorChangeSpeed);
        targetImageRight.color = CurrentColorRight;

        CurrentColorLeft = Color.Lerp(targetImageLeft.color, targetColorLeft, colorChangeSpeed);
        targetImageLeft.color = CurrentColorLeft;

        if (clockwiseCounter >= 10)
        {
            Debug.Log("時計回り10回");
            clockwiseCounter = 0;
        }
        if (counterClockwiseCounter >= 10)
        {
            Debug.Log("反時計回り10回");
            counterClockwiseCounter = 0;
        }
    }

    public void HandleTouch(Vector2 inputPos)
    {
        Debug.Log("HandleTouchが呼ばれた");
        isDragging = true;

        RectTransform rectTransformRight = (RectTransform)targetImageRight.transform;
        RectTransform rectTransformLeft = (RectTransform)targetImageLeft.transform;

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransformRight, inputPos, null))
        {
            ChangeColorRight();

            if (inputPos.y > previousInputPosition.y && isRightNeg)
            {
                Debug.Log("R反時計回り");
                counterClockwiseCounter++;
                clockwiseCounter = 0;
                isRightPos = true;
                isLeftNeg = true;
                isLeftPos = true;
                isRightNeg = false;
            }
            else if (inputPos.y < previousInputPosition.y && isRightPos)
            {
                Debug.Log("R時計回り");
                clockwiseCounter++;
                counterClockwiseCounter = 0;
                isRightNeg = true;
                isLeftNeg = true;
                isLeftPos = true;
                isRightPos = false;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(rectTransformLeft, inputPos, null))
        {
            ChangeColorLeft();

            if (inputPos.y > previousInputPosition.y && isLeftPos)
            {
                Debug.Log("L時計回り");
                clockwiseCounter++;
                counterClockwiseCounter = 0;
                isRightPos = true;
                isRightNeg = true;
                isLeftNeg = true;
                isLeftPos = false;
            }
            else if (inputPos.y < previousInputPosition.y && isLeftNeg)
            {
                Debug.Log("L反時計回り");
                counterClockwiseCounter++;
                clockwiseCounter = 0;
                isRightPos = true;
                isRightNeg = true;
                isLeftPos = true;
                isLeftNeg = false;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        previousInputPosition = inputPos;
    }

    public void StopTouch()
    {
        isDragging = false;
    }

    void ChangeColorRight()
    {
        debugText.text = "Music Play";
        targetColorRight = colorPalette[Random.Range(0, colorPalette.Length)];
    }

    void ChangeColorLeft()
    {
        debugText.text = "Music Play";
        targetColorLeft = colorPalette[Random.Range(0, colorPalette.Length)];
    }
}