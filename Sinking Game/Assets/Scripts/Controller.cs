using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
{

    [SerializeField]
    private float grav = 1.76f;

    [SerializeField] 
    private float lerpSpeed;

    private float _jumpTimer;

    [SerializeField] 
    private GameObject mainCam;

    private bool _pressed = false;
    private bool _buffer = false;
    // private float playerX()
    // {
    //     return gameObject.transform.position.x;
    // }

    private float playerY()
    {
        return gameObject.transform.position.y;
    }
    
    private float RotZ()
    {
        //the rotation that is in the inspector does not
        //simply equal transform.rotation. 
        //if you want the rotation in the inspector you have to return/reference
        //localEulerAngle
        return transform.localEulerAngles.z;
    }
    
    private float test;
    
    private Quaternion _startRot;
    private Quaternion _endRot;
    
    private Vector3 gravVector;

    //sets the two points we lerp between.
    IEnumerator CalcLimit(float rotX, float rotY, float limit)
    {
        //returns euler angles for a start and end rotation position. 
        _startRot = Quaternion.Euler(rotX, rotY, limit);
        _endRot = Quaternion.Euler(rotX, rotY, -limit);
        yield return _startRot;
        yield return _endRot;
    }

    IEnumerator SwimUP()
    {
        _pressed = true;
        //sets keypress buffer to true, space can't be pressed again.
        _buffer = true;
        //while this timer is less than 2;
        while (_jumpTimer <= 2)
        {
            //moves the object up by rate of gravity. 
            gameObject.transform.position += gravVector;
            _jumpTimer++;
        }
        //we wait for a mo
        yield return new WaitForSeconds(0.5f);
        _pressed = false;
        _jumpTimer = 0;
        yield return new WaitForSeconds(0.5f);
        _buffer = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CalcLimit(0, 0, RotZ()));
    }

    // Update is called once per frame
    void Update()
    {
        
        Swim();
        DescendPlayer();
        FollowPlayer(playerY());
        
        float time = Mathf.PingPong(Time.time * lerpSpeed, 1);
        gameObject.transform.rotation = Quaternion.Slerp(_startRot, _endRot, time);
    }

    void DescendPlayer()
    {
        //creates vector 3 for gravity.
        gravVector = new Vector3(0, grav * Time.deltaTime, 0);
        //descneds the player by rate of the gravity per frame. 
        if (_pressed == false)
        {
            gameObject.transform.position -= gravVector;
        }
    }

    void FollowPlayer(float camY)
    {
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, camY, mainCam.transform.position.z);
    }

    void Swim()
    {
        if (Input.GetKeyDown("space") && _buffer == false)
        {
            StartCoroutine(SwimUP());
        }
        else
        {
            Debug.Log("sinking");
        }
    }
    


}

