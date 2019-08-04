using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    [SerializeField]
    private float maxStretch = 3.0f;         //³Ì¤j¦ù®i
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;

    public Ball nextBall;
    public bool isStart = false;

    private Transform catapult;
    private SpringJoint2D springJoint2D;
    private Rigidbody2D rigidbody2D;
    private CircleCollider2D circleCollider2D;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSqr;
    private float circleRadius;
    private Vector2 prevVelocity;

	private bool isClicked = false;

    private void Awake()
    {
        springJoint2D = GetComponent<SpringJoint2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();

        catapult = springJoint2D.connectedBody.transform;
    }

    private void Start()
    {
        LineRendererSetup();
        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        circleRadius = circleCollider2D.radius;
    }

    void Update ()
	{
        if (!isStart)
            return;

		if (isClicked)
            Dragging();

        if (springJoint2D != null)
        {
            if (rigidbody2D.bodyType == RigidbodyType2D.Dynamic && prevVelocity.sqrMagnitude > rigidbody2D.velocity.sqrMagnitude)
            {
                Destroy(springJoint2D);
                rigidbody2D.velocity = prevVelocity;
            }

            if (!isClicked)
                prevVelocity = rigidbody2D.velocity;

            LineRendererUpdate();
        }
        else
        {
            catapultLineFront.enabled = false;
            catapultLineBack.enabled = false;

            if (nextBall != null)
            {
                nextBall.enabled = true;
                nextBall.StartBall();
            }
            else
                NoBall();

            enabled = false;
        }
	}

    private void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

        if (catapultToMouse.magnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretchSqr);
        }

        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }

    private void OnMouseDown ()
	{
        springJoint2D.enabled = false;
        isClicked = true;
    }

    private void OnMouseUp ()
	{
        springJoint2D.enabled = true;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        isClicked = false;
	}

    private void StartBall()
    {
        StartCoroutine(StartPointAnimation(1.0f));
    }

    private void NoBall()
    {
        //Game UI Restart Btn
    }

    private void Restart()
    {
        GameManager.EnemyAlive = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LineRendererSetup()
    {
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "Foreground";
        catapultLineBack.sortingLayerName = "Foreground";

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;
    }

    private void LineRendererUpdate()
    {
        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius / 2);
        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }

    IEnumerator StartPointAnimation(float time)
    {
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1f / time;
        float percent = 0;
        Vector2 initialPoint = transform.position;
        Vector2 startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform.position;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;

            transform.position = Vector2.Lerp(initialPoint, startPoint, percent);
            yield return null;
        }
        catapultLineFront.enabled = true;
        catapultLineBack.enabled = true;
        isStart = true;
    }
}
