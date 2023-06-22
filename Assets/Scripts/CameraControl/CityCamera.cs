using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class CityCamera : MonoBehaviour
{
    
    [SerializeField]
    private Vector2Int center = new Vector2Int(Screen.width,Screen.height)/2;
    [SerializeField]
    private float distFromPivot = 10;
    public float targetDistFromPivot = 10;
    [SerializeField]
    private float rotation = 0;
    [SerializeField]
    private float height = 10;
    public float targetHeight = 10;
    [SerializeField]
    private Transform pivotPoint;
    [SerializeField]
    private float mouseSensitivity = 1;
    public float Rotation { get => rotation; set => rotation = value % 360; }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        SpinCamera();
        MovePivotPoint();
        ScrollView();
    }
    void MoveMouseCursor()
    {

    }
    void SpinCamera()
    {
        if(Mouse.current.rightButton.IsPressed())
        {
            Rotation += -MousePosDiff().x;

            
            
        }
    }
    void MovePivotPoint()
    {
        Vector2 moveDirection = Vector2.zero;
        if(Keyboard.current.aKey.IsPressed())
        {
            moveDirection.x -= Keyboard.current.leftShiftKey.isPressed ? 2 : 1;
        }
        if(Keyboard.current.dKey.IsPressed())
        {
            moveDirection.x += Keyboard.current.leftShiftKey.isPressed ? 2 : 1;
        }
        if (Keyboard.current.wKey.IsPressed())
        {
            moveDirection.y += Keyboard.current.leftShiftKey.isPressed ? 2 : 1;
        }
        if(Keyboard.current.sKey.IsPressed())
        {
            moveDirection.y -= Keyboard.current.leftShiftKey.isPressed ? 2 : 1;
        }
        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0;
        pivotPoint.transform.Translate(moveDirection.y * direction.normalized/10);
        direction = Camera.main.transform.right;
        direction.y = 0;
        pivotPoint.transform.Translate(moveDirection.x * direction.normalized/10);

        if (Mouse.current.middleButton.IsPressed())
        {
            direction = Camera.main.transform.right;
            direction.y = 0;
            pivotPoint.transform.Translate(direction.normalized * -MousePosDiff().x/10);
            direction = Camera.main.transform.forward;
            direction.y = 0;
            pivotPoint.transform.Translate(direction.normalized * -MousePosDiff().y/10);
        }
    }

    void ScrollView()
    {
        targetHeight += -ScrollDiff()/120f;
        targetHeight = Mathf.Clamp(targetHeight, 1, 15);
        float time = 1f;
        height = Mathf.SmoothDamp(height, targetHeight, ref time, Time.deltaTime, 30f);
        targetDistFromPivot += -ScrollDiff()/60f;
        targetDistFromPivot = Mathf.Clamp(targetDistFromPivot, 5, 10);
        time = 1f;
        distFromPivot = Mathf.SmoothDamp(distFromPivot, targetDistFromPivot, ref time, Time.deltaTime,20f);
        UpdateCamera();
    }

    void UpdateCamera()
    {
        gameObject.transform.localPosition = new(
                Mathf.Cos(Mathf.Deg2Rad * Rotation) * distFromPivot,
                height,
                Mathf.Sin(Mathf.Deg2Rad * Rotation) * distFromPivot);
        gameObject.transform.LookAt(pivotPoint);


        if (pivotPoint.position.magnitude > 15f)
        {
            Debug.Log(Mathf.Atan(pivotPoint.position.y/pivotPoint.position.x));
        }
        

    }

    float ScrollDiff()
    {
        return Mouse.current.scroll.y.ReadValue();
    }


    Vector2 MousePosDiff()
    {
        return Mouse.current.delta.ReadValue() * mouseSensitivity;
    }
}
