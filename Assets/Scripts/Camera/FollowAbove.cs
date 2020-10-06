using UnityEngine;
using System.Collections;

public class FollowAbove : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private float smooth;
    [SerializeField] private float heigth;

    private void Update()
    {
        Vector3 goal = target.position;
        goal = new Vector3(target.position.x, target.position.y + heigth, target.position.z);
        transform.position = Vector3.Lerp(transform.position, goal, Time.deltaTime * smooth);
    } 

} 
