using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartPractice()
    {
        Debug.Log("START PRACTICE BUTTON CLICKED");
        SceneManager.LoadScene("EnvironmentSelection");
    }

    // VIEW PROGRESS BUTTON
    public void ViewProgress()
    {
        Debug.Log("VIEW PROGRESS BUTTON CLICKED");
        SceneManager.LoadScene("ViewProgress");
    }
}
