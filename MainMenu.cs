using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    public PlayableDirector cutSceneTimeline;
    public GameObject Player;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayGameAnim()
    {
        cutSceneTimeline.Play();

        //disable mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Player.GetComponent<PlayerController>().Pause = false;
    }
}