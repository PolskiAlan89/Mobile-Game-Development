using UnityEngine;
using UnityEngine.SceneManagement;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class UIManager : MonoBehaviour
{
    public void ChangeMinigame(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
