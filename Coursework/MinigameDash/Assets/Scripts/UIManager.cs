using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public void ChangeMinigame(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
