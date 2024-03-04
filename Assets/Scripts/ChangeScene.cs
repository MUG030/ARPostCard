using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName; // シーン名を格納する変数

    public void Change()
    {
        SceneManager.LoadScene(sceneName);
    }
}
