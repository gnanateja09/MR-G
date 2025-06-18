using UnityEngine;
public class SpawnTracker : MonoBehaviour
{
    public spawn spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnSoldierDestroyed();
        }
    }
}
