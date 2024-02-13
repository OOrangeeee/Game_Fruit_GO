using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool isGround;
    public float checkRaduis;
    public LayerMask groundLayer;
    public Vector2 bottomOffset;//½Åµ×Î»ÒÆ²îÖµ
    private void Update()
    {
        check();
    }
    public void check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRaduis);
    }
}
