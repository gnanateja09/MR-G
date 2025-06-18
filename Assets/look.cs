using UnityEngine;

public class TestLookAt : MonoBehaviour
{
    public string targetTag = "Player";  // Set this in the Inspector or keep "Player"
    private Transform target;

    void Start()
    {
        GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
        else
        {
            Debug.LogError("No GameObject found with tag: " + targetTag);
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Only rotate to face player on the Y axis (no tilting)
            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(targetPosition);
        }
    }
}
