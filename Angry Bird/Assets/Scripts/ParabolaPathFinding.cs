using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaPathFinding : MonoBehaviour {

    [SerializeField]
    private float maxTime;
    
    private ParabolaPathRequestManager requestManager;
    private Vector3 moveSpeed;//初速度向量
    private Vector3 gritySpeed = Vector3.zero;//重力的速度向量

    private void Awake()
    {
        requestManager = GetComponent<ParabolaPathRequestManager>();
    }

    public void StartFindPath(Vector3 startPoint, float power, float angle, float gravity)
    {
        StartCoroutine(FindPath(startPoint, power, angle, gravity));
    }

    private IEnumerator FindPath(Vector3 startPoint, float power, float angle, float gravity)
    {
        List<Vector3> waypoints = new List<Vector3>();
        moveSpeed = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        bool pathSuccess = false;
        float tempTime = 0;

        while (tempTime < maxTime)
        {            
            //v = at ;
            gritySpeed.y = gravity * (tempTime += Time.fixedDeltaTime);
            //位移模拟轨迹
            startPoint += (moveSpeed + gritySpeed) * Time.fixedDeltaTime;
            waypoints.Add(startPoint);
            yield return null;
        }
        pathSuccess = true;

        requestManager.FinishedProcessingPath(SimplifyPath(waypoints), pathSuccess);//找到路徑  返回callback
    }

    //簡化路徑 去除2點直線之間的點
    private Vector3[] SimplifyPath(List<Vector3> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = path[i - 1] - path[i];
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i]);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }
}
