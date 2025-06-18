using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class spawn : MonoBehaviour
{
    public float spawnTimer = 1f;
    public GameObject prefabToSpawn;
    public float normalOffset;
    public float miniEdgeDistance = 0.3f;

    public MRUKAnchor.SceneLabels spawnLabels;

    private float timer;

    private int currentSpawnCount = 0;
    public int maxSpawnCount = 3;

    void Update()
    {
        if (!MRUK.Instance || !MRUK.Instance.IsInitialized)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnTimer && currentSpawnCount < maxSpawnCount)
        {
            SpawnGhost();
            timer = 0f;
        }
    }

public void SpawnGhost()
{
    MRUKRoom room = null;
    var rooms = MRUK.Instance.Rooms;

    foreach (var r in rooms)
    {
        if (r.IsPositionInRoom(Camera.main.transform.position))
        {
            room = r;
            break;
        }
    }

    if (room == null) return; // No valid room found

    LabelFilter labelFilter = new LabelFilter(spawnLabels);

    if (room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.VERTICAL, miniEdgeDistance, labelFilter, out Vector3 pos, out Vector3 norm))
    {
        Vector3 spawnPosition = pos + norm * normalOffset;
        spawnPosition.y = 0;

        GameObject spawned = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        currentSpawnCount++;

        spawned.AddComponent<SpawnTracker>().spawner = this;
    }
}
    // Called from SpawnTracker
    public void OnSoldierDestroyed()
{
    currentSpawnCount--;
}
}