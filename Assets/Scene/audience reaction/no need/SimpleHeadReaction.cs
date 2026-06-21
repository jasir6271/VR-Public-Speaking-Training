using UnityEngine;

public class SimpleHeadReaction : MonoBehaviour
{
    Quaternion originalRotation;
    bool reacting = false;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    public void React()
    {
        if (!reacting)
            StartCoroutine(ShakeHead());
    }

    System.Collections.IEnumerator ShakeHead()
    {
        reacting = true;

        transform.localRotation = Quaternion.Euler(0, 30, 0);
        yield return new WaitForSeconds(0.3f);

        transform.localRotation = Quaternion.Euler(0, -30, 0);
        yield return new WaitForSeconds(0.3f);

        transform.localRotation = originalRotation;
        reacting = false;
    }
}