using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class TraceUI : MonoBehaviour
{
    public Image targetImageRight; // 右のImage
    public Image targetImageLeft; // 左のImage

    public AudioSource audioSource; // 音楽を再生するためのAudioSource

    private Vector2 previousInputPosition; // 前回の入力の位置

    private int clockwiseCounter = 0; // 時計回りのカウンター
    private int counterClockwiseCounter = 0; // 反時計回りのカウンター

    private Color targetColorRight;
    private Color targetColorLeft;

    // 右のImageが選択されているかを判断する変数
    private bool isRightPos = true, 
                 isRightNeg = true;

    // 左のImageが選択されているかを判断する変数
    private bool isLeftPos = true, 
                 isLeftNeg = true;

    [SerializeField] private float colorChangeSpeed = 0.05f; // 色の変化速度を制御する変数

    [SerializeField] private TextMeshProUGUI debugText; // デバッグ用のテキスト

    public Color CurrentColorRight { get; private set; } // 右のImageの現在の色を保持するプロパティ
    public Color CurrentColorLeft { get; private set; } // 左のImageの現在の色を保持するプロパティ

    public Color[] colorPalette; // 色のパレット

    // InputHandlerのインスタンスを作成
    private InputHandler inputHandler = new InputHandler();

    // Start is called before the first frame update
    void Start()
    {
        targetColorRight = targetImageRight.color; // 初期色を設定
        targetColorLeft = targetImageLeft.color; // 初期色を設定
    }


    // Update is called once per frame
    void Update()
    {
        // InputHandlerのUpdateメソッドを呼び出す
        inputHandler.Update();

        // InputHandlerのPositionプロパティを使用して現在の入力位置を取得
        Vector2 inputPos;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            inputPos = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else
        {
            // タッチスクリーンが利用できない場合は、マウスの位置を使用
            inputPos = Mouse.current.position.ReadValue();
        }

        // Vector2 mousePos = Mouse.current.position.ReadValue();
        // Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        RectTransform rectTransformRight = (RectTransform)targetImageRight.transform;
        RectTransform rectTransformLeft = (RectTransform)targetImageLeft.transform;

        bool isTouching = false;

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;
            isTouching = touch.press.isPressed && touch.phase.ReadValue() != UnityEngine.InputSystem.TouchPhase.Ended && touch.phase.ReadValue() != UnityEngine.InputSystem.TouchPhase.Canceled;
        }

        if (isTouching || Mouse.current.leftButton.isPressed)
        {
            debugText.text = "Music Play Demo";
            // マウスの位置が右のImageの範囲内にあるかを確認
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransformRight, inputPos, null))
            {
                ChangeColorRight();

                // マウスの動きを判断
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

                // 音楽を再生
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            // マウスの位置が左のImageの範囲内にあるかを確認
            else if (RectTransformUtility.RectangleContainsScreenPoint(rectTransformLeft, inputPos, null))
            {
                ChangeColorLeft();

                // マウスの動きを判断
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

                // 音楽を再生
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            debugText.text = "Music Stop Demo";
            // マウスがなぞっていないときは音楽を停止
            if (audioSource.isPlaying)
            {
                debugText.text = "Music Stop";
                audioSource.Stop();
            }
        }

        // 現在の色と目標の色の間で補間を行う
        CurrentColorRight = Color.Lerp(targetImageRight.color, targetColorRight, colorChangeSpeed);
        targetImageRight.color = CurrentColorRight;

        CurrentColorLeft = Color.Lerp(targetImageLeft.color, targetColorLeft, colorChangeSpeed);
        targetImageLeft.color = CurrentColorLeft;

        // 現在のタッチの位置を記録
        previousInputPosition = inputHandler.Position;

        // カウンターが10に達した場合の処理
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

    void ChangeColorRight()
    {
        debugText.text = "Music Play";
        // 色のパレットからランダムに色を選択
        targetColorRight = colorPalette[Random.Range(0, colorPalette.Length)];
    }

    void ChangeColorLeft()
    {
        debugText.text = "Music Play";
        // 色のパレットからランダムに色を選択
        targetColorLeft = colorPalette[Random.Range(0, colorPalette.Length)];
    }
}
