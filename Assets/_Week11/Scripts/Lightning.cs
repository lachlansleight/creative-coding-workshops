using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lightning : MonoBehaviour
{
    public GameObject Particles;
    public float Lifetime = 1f;
    public UnityEvent OnStrike;
    
    public void Strike()
    {
        Particles.gameObject.SetActive(true);
        OnStrike?.Invoke();
        StartCoroutine(DestroyAfterLifetime());
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}
