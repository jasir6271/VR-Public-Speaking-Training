using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentSelector : MonoBehaviour
{
    public static string selectedEnvironment = "";

    public void SelectOneToOne()
    {
        selectedEnvironment = "OneToOneScene";
        Debug.Log("One-to-One Selected");
    }

    public void SelectClassroom()
    {
        selectedEnvironment = "ClassroomScene";
        Debug.Log("Classroom Selected");
    }

    public void SelectAuditorium()
    {
        selectedEnvironment = "AuditoriumScene";
        Debug.Log("Auditorium Selected");
    }

    public void Continue()
    {
        if (selectedEnvironment != "")
        {
            SceneManager.LoadScene(selectedEnvironment);
        }
        else
        {
            Debug.Log("No environment selected");
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
