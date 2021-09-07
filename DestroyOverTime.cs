using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float timeToDestroy;

    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }
}