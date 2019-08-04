using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult : MonoBehaviour {

    [SerializeField]
    private Transform leftCatapult, rightCatapult, startPoint;

    [SerializeField]
    private LineRenderer leftLineRenderer, rightLineRenderer, trajectoryLineRenderer;

    [SerializeField]
    private float throwSpeed, maxStretch, minStretch;

    [Header("TrajectoryLine")]
    [SerializeField]
    private int segmentCount = 15;
    [SerializeField]
    private float segmentSpacing = 2;


    private Bird currentBird;
    private Vector3 centerCatapult;
    private bool isDrag;

    private void Start()
    {
        centerCatapult = new Vector3((leftCatapult.position.x + rightCatapult.position.x) / 2,
            (leftCatapult.position.y + rightCatapult.position.y) / 2, 0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D collider2D = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
            if (collider2D != null && collider2D.tag == "Bird")
            {
                if (currentBird == null)
                {
                    currentBird = collider2D.transform.GetComponent<Bird>();
                    StartCoroutine(MoveToStartPoint(0.5f));
                }
                else if (collider2D.GetComponent<Bird>() == currentBird)
                {
                    isDrag = true;
                }
            }        
        }

        if (Input.GetMouseButtonUp(0) && isDrag)
        {
            isDrag = false;
            trajectoryLineRenderer.enabled = false;

            float distance = (currentBird.transform.position - centerCatapult).magnitude;
            if (distance > minStretch)
            {
                leftLineRenderer.enabled = false;
                rightLineRenderer.enabled = false;
                ThrowBird(distance);
            }
            else
            {
                StartCoroutine(MoveToStartPoint(0.3f));
            }
        }

        if (isDrag)
        {            
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePoint.z = 0;

            float distance = (centerCatapult - mousePoint).magnitude;
            currentBird.transform.position = (distance > maxStretch) ? centerCatapult + (mousePoint - centerCatapult).normalized * maxStretch : mousePoint;

            DisplayCatapultLineRenderers();
            DisplayTrajectoryLineRenderer(distance);
        }
    }

    private void ThrowBird(float distance)
    {
        Vector3 velocity = centerCatapult - currentBird.transform.position;
        currentBird.OnThrow();
        currentBird.Velocity = new Vector2(velocity.x, velocity.y) * throwSpeed * distance;
        currentBird = null;
    }

    private void DisplayCatapultLineRenderers()
    {
        leftLineRenderer.enabled = true;
        rightLineRenderer.enabled = true;
        leftLineRenderer.SetPosition(1, currentBird.transform.position);
        rightLineRenderer.SetPosition(1, currentBird.transform.position);
    }

    private void DisplayTrajectoryLineRenderer(float distance)
    {
        trajectoryLineRenderer.enabled = true;

        Vector2[] segments = new Vector2[segmentCount];     //片段陣列       
        Vector2 segVelocity = (centerCatapult - currentBird.transform.position) * throwSpeed * distance;        //初始速度

        segments[0] = currentBird.transform.position;       //第一點
        for (int i = 1; i < segmentCount; i++)
        {
            float time = i * Time.fixedDeltaTime * segmentSpacing;
            segments[i] = segments[0] + segVelocity * time + 0.5f * Physics2D.gravity * Mathf.Pow(time, 2); //S = Vot + (1 / 2)at ^ 2
        }

        trajectoryLineRenderer.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
            trajectoryLineRenderer.SetPosition(i, segments[i]);
    }

    IEnumerator MoveToStartPoint(float time)
    {
        float animationSpeed = 1f / time;
        float percent = 0;
        Vector2 initialPoint = currentBird.transform.position;

        while (percent < 1)
        {
            percent += animationSpeed * Time.deltaTime;

            currentBird.transform.position = Vector2.Lerp(initialPoint, startPoint.position, percent);
            yield return null;
        }

        DisplayCatapultLineRenderers();
    }
}