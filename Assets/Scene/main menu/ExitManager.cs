using UnityEngine;

public class ExitManager : MonoBehaviour
{
    public GameObject exitPopup;

    // Called when Exit button is clicked
    public void ShowExitPopup()
    {
        exitPopup.SetActive(true);
    }

    // Called when NO is clicked
    public void CancelExit()
    {
        exitPopup.SetActive(false);
    }

    // Called when YES is clicked
    public void ConfirmExit()
    {
        Debug.Log("Exiting to main menu...");

        // If you have a main scene
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

        // If you just want to quit app
         Application.Quit();
    }
}
