using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    private Rigidbody2D rigidbody2D;
    private TrailRenderer trailRenderer;

    public Vector2 Velocity { get { return rigidbody2D.velocity; } set { rigidbody2D.velocity = value; } }

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public void OnThrow()
    {
        trailRenderer.enabled = true;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }
}
