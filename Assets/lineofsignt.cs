using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class EnemyLineOfSightChecker : MonoBehaviour
{
    public SphereCollider Collider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayers;

    public delegate void GainSightEvent(Transform Target);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Transform Target);
    public LoseSightEvent OnLoseSight;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
        Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!CheckLineOfSight(other.transform))
        {
            CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(other.transform));
        }
        else
        {
            OnGainSight?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        OnLoseSight?.Invoke(other.transform);
        if (CheckForLineOfSightCoroutine != null)
        {
            StopCoroutine(CheckForLineOfSightCoroutine);
        }
    }

    private bool CheckLineOfSight(Transform target)
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f; // Adjust eye height
        Vector3 direction = (target.position - origin).normalized;

        float dotProduct = Vector3.Dot(transform.forward, direction);

        if (dotProduct >= Mathf.Cos(FieldOfView * Mathf.Deg2Rad * 0.5f))
        {
            if (Physics.Raycast(origin, direction, out RaycastHit hit, Collider.radius, LineOfSightLayers))
            {
                if (hit.transform == target)
                {
                    OnGainSight?.Invoke(target);
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator CheckForLineOfSight(Transform target)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);

        while (!CheckLineOfSight(target))
        {
            yield return wait;
        }

        OnGainSight?.Invoke(target);
    }
}
