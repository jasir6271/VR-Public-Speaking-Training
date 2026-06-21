using UnityEngine;
using TMPro;

public class EyeContactDetector : MonoBehaviour
{
    public Camera playerCamera;
    public TextMeshProUGUI eyeContactText;

    public float maxDistance = 20f;

    private float totalTime = 0f;
    private float lookingTime = 0f;

    void Update()
    {
        totalTime += Time.deltaTime;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // CHECK IF LOOKING AT AUDIENCE
            if (hit.collider.CompareTag("Audience"))
            {
                lookingTime += Time.deltaTime;
            }
        }

        // CALCULATE %
        float percent = (lookingTime / totalTime) * 100f;

        if (eyeContactText != null)
        {
            eyeContactText.text = "Eye Contact: " + percent.ToString("F0") + "%";
        }
    }
}