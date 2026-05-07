using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Final"); 
    }
}
