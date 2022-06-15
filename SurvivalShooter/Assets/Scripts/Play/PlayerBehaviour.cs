using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour
{
    public NavMeshAgent meshAgent;
    public Transform target;

    private void Start()
    {
        _tempPaths = new List<GameObject>();
    }

    private void Update()
    {
        CheckDrawPath();
        CheckMove();
    }

    void CheckMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    target.transform.position = hit.point;
                    meshAgent.SetDestination(target.position);

                    _timerToDraw = timeToDraw;
                }
            }
        }
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public GameObject brush;
    public Transform pointParent;
    public float timeToDraw = 0.1f;
    private float _timerToDraw = 0;

    private List<GameObject> _tempPaths;
        
    void CheckDrawPath()
    {
        if (_timerToDraw <= 0)
        {
            //do nothing
        }
        else
        {
            _timerToDraw -= Time.deltaTime;
            if (_timerToDraw <= 0)
            {
                DrawPath();
            }
        }
    }

    void DrawPath()
    {
        ClearPath();

        var points = meshAgent.path.corners;
        foreach (var point in points)
        {
            //生成一个brush，画出每一个point
            var myPoint = Instantiate(brush, point+Vector3.up*0.5f, Quaternion.identity, pointParent);
            myPoint.SetActive(true);
            _tempPaths.Add(myPoint);
        }
    }

    void ClearPath()
    {
        foreach (var point in _tempPaths)
        {
            Destroy(point);
        }

        _tempPaths = new List<GameObject>();
    }
}
