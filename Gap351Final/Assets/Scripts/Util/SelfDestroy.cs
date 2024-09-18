using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float m_timeForDestruction;

    private void Start()
    {
        StartCoroutine(DestroySelf(m_timeForDestruction));
    }

    private IEnumerator DestroySelf(float mTimeForDestruction)
    {
        yield return new WaitForSeconds(mTimeForDestruction);

        Destroy(gameObject);
    }
}
