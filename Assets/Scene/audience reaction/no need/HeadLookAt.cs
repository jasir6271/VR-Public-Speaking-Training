using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRotation,
            Time.deltaTime * speed
        );
    }
}
