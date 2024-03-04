using UnityEngine;

public class TouchHandller : MonoBehaviour
{
    public TraceUIMobile traceUIMobile;

    private void Update()
    {
        // マウスの左クリックを判定
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            traceUIMobile.HandleTouch(mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Mouse Up");
            traceUIMobile.StopTouch();
        }

        // タッチを判定
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // 最初のタッチを取得

            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                // タッチされている間、ここが実行されます
                Vector3 touchPosition = touch.position;
                traceUIMobile.HandleTouch(touchPosition);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // タッチが終了したとき、ここが実行されます
                traceUIMobile.StopTouch();
            }
        }
    }
}
