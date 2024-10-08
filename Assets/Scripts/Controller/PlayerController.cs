using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public static bool s_canPressKey = true;

    // 이동
    [SerializeField] float moveSpeed = 5.0f;
    Vector3 dir = new Vector3();
    public Vector3 destPos = new Vector3();
    Vector3 originPos = new Vector3();

    // 회전
    [SerializeField] float spinSpeed = 360.0f;
    Vector3 rotDir = new Vector3();
    Quaternion destRot = new Quaternion();


    // 반동
    [SerializeField] float recoilPosY = 0.25f;
    [SerializeField] float recoilSpeed = 1.5f;

    bool canMove = true;
    bool isFalling = false;

    // 기타
    [SerializeField] Transform fakeCube = null;
    [SerializeField] Transform realCube = null;

    TimingManager theTimingManager;
    CameraController theCam;
    Rigidbody myRigid;
    StatusManager theStatus;


    void Start()
    {
        theTimingManager = FindObjectOfType<TimingManager>();
        theCam = FindObjectOfType<CameraController>();
        myRigid = GetComponentInChildren<Rigidbody>();
        theStatus = FindObjectOfType<StatusManager>();
        originPos = transform.position;
    }

    void Update()
    {
        if(GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                if (canMove && s_canPressKey && !isFalling)
                {
                    Calc();

                    if (theTimingManager.CheckTiming())
                    {
                        StartAction();
                    }
                }
            }
        }
    }

    public void Initialized()
    {
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        realCube.localPosition = Vector3.zero;
        canMove = true;
        s_canPressKey = true;
        isFalling = false;
        myRigid.useGravity = false;
        myRigid.isKinematic = true;
    }


    void Calc()
    {
        // 방향 계산
        dir.Set(Input.GetAxisRaw("Vertical"), 0, Input.GetAxisRaw("Horizontal"));

        // 이동 목표값 계산
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);

        // 회전 목표값 계산
        rotDir = new Vector3(-dir.z, 0f, -dir.x);
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        destRot = fakeCube.rotation;
    }

    void StartAction()
    {
        StartCoroutine(SpinCo());
        StartCoroutine(MoveCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(theCam.ZoomCam());
        
    }


    IEnumerator MoveCo()
    {
        canMove = false;

        while (Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destPos;
        canMove = true;
    }

    IEnumerator SpinCo()
    {
        while(Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = destRot;
    }

    IEnumerator RecoilCo()
    {
        while(realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while(realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        realCube.localPosition = Vector3.zero;

    }

    void CheckFalling()
    {
        if(!isFalling && canMove)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                Falling();
            }
        }
    }

    void Falling()
    {
        isFalling = true;
        myRigid.useGravity = true;
        myRigid.isKinematic = false;
    }

    public void ResetFalling()
    {
        theStatus.DecreaseHP(1);
        AudioManager.instance.PlaySFX("Falling");

        if(!theStatus.IsDead())
        {
            isFalling = false;
            myRigid.useGravity = false;
            myRigid.isKinematic = true;

            transform.position = originPos;
            realCube.localPosition = new Vector3(0, 0, 0);
        }


   
    }
}
