using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergePushEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float pushRadius;
    [SerializeField] private float pushMagnitute;

    private Vector2 pushPosition;

    [Header("DEBUG")]
    [SerializeField] private bool enableGizmos;
    private void Awake()
    {
        MergeManager.onMergeProcessed += MergeProcessedCallback;

    }
    private void OnDestroy()
    {
        MergeManager.onMergeProcessed -= MergeProcessedCallback;
    }


    private void MergeProcessedCallback(PlanetType planetType, Vector2 mergePos)
    {
        pushPosition = mergePos;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(mergePos, pushRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Planet planet))
            {
                Vector2 force = ((Vector2)planet.transform.position - mergePos).normalized;
                force *= pushMagnitute;
                planet.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!enableGizmos) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(pushPosition, pushRadius);
    }
#endif
}
