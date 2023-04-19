using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    public void OnInit(float scale)
    {
        Invoke(nameof(OnDespawn), 1f);
        transform.localScale = Vector3.one*scale;
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }
}
