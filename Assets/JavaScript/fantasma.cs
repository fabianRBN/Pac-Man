using UnityEngine;
using System;


public class fantasma : MonoBehaviour  {
    private int direccion = 1;
    private Rigidbody rbody;
    public Transform Target;
    public float MoveSpeed = 4f;
    public float RotationSpeed = 5f;
    private Transform myTransform;
    public pacman pc;
    void Awake()
    {
        myTransform = transform;
    }
    void Start()
    {
        Target = GameObject.FindWithTag("pacman").transform;
        pc = GameObject.FindWithTag("pacman").GetComponent<pacman>();
               
    }
    void Update()
    {

        if (pc.pacmanenojado)
        {


            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(-1 * Target.position - myTransform.position), RotationSpeed * Time.deltaTime);
            myTransform.position += myTransform.forward * MoveSpeed * Time.deltaTime;
        }
        else
        {
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation( Target.position - myTransform.position), RotationSpeed * Time.deltaTime);
            myTransform.position += myTransform.forward * MoveSpeed * Time.deltaTime;
        }

        

    }

}
