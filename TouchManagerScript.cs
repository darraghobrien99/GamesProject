using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class TouchManagerScript : MonoBehaviour
{
    private float timeOfTap;
    private float startTime;
    private float tapThreshold = 0.05f;
    private float starting_distance_to_selected_object;
    private float rotateSpeed = 10f;
    private Touch touch;
    private Vector2 oldTouchPosition;
    private Vector2 NewTouchPosition;
    private Touch oldTouch1;
    private Touch oldTouch2;
    private float initialDistance;
    private Vector3 initialScale;
    [SerializeField]

    Icontrollable selectedObject;
    Icontrollable object_hit;
    // Start is called before the first frame update
    void Start()
    {
        GameObject ourCameraPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ourCameraPlane.transform.position = new Vector3(0, Camera.main.transform.position.y, 0);
        ourCameraPlane.transform.up = (Camera.main.transform.position - ourCameraPlane.transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            RaycastHit info;

            Ray ourRay = Camera.main.ScreenPointToRay(Input.touches[0].position);

            Debug.DrawRay(ourRay.origin, 30 * ourRay.direction);

            if (Physics.Raycast(ourRay, out info))
            {
                if (info.collider != null)
                {
                    object_hit = info.transform.GetComponent<Icontrollable>();
                    selectedObject = object_hit;
                }
            }

            if (Input.touchCount == 2)
            {
                       
                Touch firstTouchPos = Input.GetTouch(0);
                Touch secondTouchPos = Input.GetTouch(1);

                if (firstTouchPos.phase == TouchPhase.Began || secondTouchPos.phase == TouchPhase.Began)
                {
                    initialDistance = Vector2.Distance(firstTouchPos.position, secondTouchPos.position);
                    if (selectedObject != null)
                    {
                        
                        initialScale = info.transform.localScale;
                        print("Scaling");
                    }
                }


                else
                {
                    float currentDistance = Vector2.Distance(firstTouchPos.position, secondTouchPos.position);

                    if (Mathf.Approximately(initialDistance, 0))
                    {
                        return;
                    }

                    float factor = currentDistance / initialDistance;

                    if (selectedObject != null)
                    {
                        selectedObject.scaleObject(initialScale, factor);
                        print("Scaling 2");
                    }
                }
            }

            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    timeOfTap = 0;
                    break;
                case TouchPhase.Ended:
                

                    if (Physics.Raycast(ourRay, out info))
                    {


                        if (object_hit != null && IsATap())
                        {
                            object_hit.youve_been_touched();
                            Debug.Log("This a tap");
                            selectedObject = object_hit;
                            starting_distance_to_selected_object = Vector3.Distance(Camera.main.transform.position, info.transform.position);
                        }

                            
                    }
                    // object_hit.deselect_object();
                    break;

                // Drag Code
                case TouchPhase.Moved:
                    Ray new_position_ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                   // selectedObject.MoveTo(new_position_ray.GetPoint(starting_distance_to_selected_object));
                    Debug.Log("This is a drag");

                    break;
            }

            


        }
    }


    private void RotateObject()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                oldTouchPosition = touch.position;
            }

            else if (touch.phase == TouchPhase.Moved)
            {
                NewTouchPosition = touch.position;
            }

            Vector2 rotDirection = oldTouchPosition - NewTouchPosition;
            Debug.Log(rotDirection);
            if (rotDirection.x < 0)
            {
                RotateRight();
                print("Rotating Right");
            }

            else if (rotDirection.x > 0)
            {
                RotateLeft();
                print("Rotating Left");
            }
        }
    }

    void RotateLeft()
    {
        transform.rotation = Quaternion.Euler(0f, 1.5f * rotateSpeed, 0f) * transform.rotation;
    }

    void RotateRight()
    {
        transform.rotation = Quaternion.Euler(0f, -1.5f * rotateSpeed, 0f) * transform.rotation;
    }


     private bool IsATap()
     {
         timeOfTap = 0;

        if(Input.touches[0].phase == TouchPhase.Began)
         {
            startTime = Time.deltaTime;
         }

         if(Input.touches[0].phase == TouchPhase.Ended)
         {
             timeOfTap = Time.deltaTime - startTime;
        }

         if(timeOfTap <= tapThreshold && timeOfTap != 0)
         {
             print("Object Tapped");
            return true;
         }
       else
        {
            return false;
         }


     }

}
