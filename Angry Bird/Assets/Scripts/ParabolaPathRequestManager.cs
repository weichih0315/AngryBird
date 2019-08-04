using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaPathRequestManager : MonoBehaviour {

    private Queue<ParabolaPathRequest> pathRequestQueue = new Queue<ParabolaPathRequest>();     //使用佇列  先要求先處理
    private ParabolaPathRequest currentPathRequest;

    private static ParabolaPathRequestManager instance;
    private ParabolaPathFinding pathfinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<ParabolaPathFinding>();
    }

    public static void RequestPath(Vector3 startPoint, float power, float angle, float gravity, Action<Vector3[], bool> callback)
    {
        ParabolaPathRequest newRequest = new ParabolaPathRequest(startPoint, power, angle, gravity, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)        //處理所有的路徑需求  完成時再次確認
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.startPoint, currentPathRequest.power, currentPathRequest.angle, currentPathRequest.gravity);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();                                           //處理所有的路徑需求
    }

    struct ParabolaPathRequest
    {
        public Vector3 startPoint;              //起點
        public float power;                     //力大小
        public float angle;                     //角度
        public float gravity;                   //重力加速度
        public Action<Vector3[], bool> callback;                    //回傳  路徑 成功找到 於請求者

        public ParabolaPathRequest(Vector3 _startPoint, float _power, float _angle, float _gravity, Action<Vector3[], bool> _callback)
        {
            startPoint = _startPoint;
            power = _power;
            angle = _angle;
            gravity = _gravity;
            callback = _callback;
        }
    }
}


