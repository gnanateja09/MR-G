using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Meta.XR.MRUtilityKit;

public class runtimenavmeshbuilder : MonoBehaviour
{
    private NavMeshSurface navmeshSurface;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navmeshSurface = GetComponent<NavMeshSurface>();
        MRUK.Instance.RegisterSceneLoadedCallback(BuildNavmesh);
    }

    public void BuildNavmesh()
    {
        StartCoroutine(BuildNavmeshRoutine());

    }


    public IEnumerator BuildNavmeshRoutine()
    {
        yield return new WaitForEndOfFrame();
        navmeshSurface.BuildNavMesh();
    }

}
