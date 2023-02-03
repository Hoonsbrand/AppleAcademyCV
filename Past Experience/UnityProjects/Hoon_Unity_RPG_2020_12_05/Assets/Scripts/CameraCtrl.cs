using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    //-------------- 카메라가 주인공을 따라다니게 하기 위한 변수
    public GameObject m_HeroObj = null;

    private float smoothTime = 0.2f;
    private float xVelocity = 0.0f;
    private float zVelocity = 0.0f;
    Vector3 newPosition = Vector3.zero;
    //-------------- 카메라가 주인공을 따라다니게 하기 위한 변수


    // --- 카메라 줌인 줌아웃
    private float maxDist = 50.0f;
    private float minDist = 5.0f;
    private float zoomSpeed = 1.0f;
    private float distance = 10.0f;
    // --- 카메라 줌인 줌아웃

    Camera RefCam = null;

    //------------------- LimitMoveCam(카메라가 지형 밖으로 나갈 수 없도록 막기)
    [HideInInspector] public Vector3 GroundMin = Vector3.zero;
    [HideInInspector] public Vector3 GroundMax = Vector3.zero;

    float a_LmtBdLeft = 0;
    float a_LmtBdTop = 0;
    float a_LmtBdRight = 0;
    float a_LmtBdBottom = 0;

    Vector3 a_ScMin = new Vector3(0.0f, 0.0f, 0.0f); //ScreenViewPort 좌측하단
    [HideInInspector] public Vector3 m_CamWMin = Vector3.zero;
    Vector3 a_ScMax = new Vector3(1.0f, 1.0f, 0.0f); //ScreenViewPort 우측상단
    [HideInInspector] public Vector3 m_CamWMax = Vector3.zero;
    Vector3 m_ScWdHalf = Vector3.zero;
    //------------------- LimitMoveCam(카메라가 지형 밖으로 나갈 수 없도록 막기)



    // Start is called before the first frame update
    void Start()
    {
        RefCam = Camera.main.GetComponent<Camera>();
        distance = RefCam.orthographicSize;

        //------------------- LimitMoveCam(카메라가 지형 밖으로 나갈 수 없도록 막기)
        GameObject GroundObj = GameObject.Find("GroundObj");
        Vector3 GrdHalfSize = Vector3.zero;

        GrdHalfSize.x = GroundObj.transform.localScale.x / 2.0f;
        GrdHalfSize.y = GroundObj.transform.localScale.y / 2.0f;
        GrdHalfSize.z = GroundObj.transform.localScale.z / 2.0f;

        //--좌측하단 (전체 지형의 꼭지점 구하기)
        GroundMin.x = GroundObj.transform.position.x - GrdHalfSize.x;
        GroundMin.y = GroundObj.transform.position.y - GrdHalfSize.y;
        GroundMin.z = GroundObj.transform.position.z - GrdHalfSize.z;

        //--우측상단 (전체 지형의 꼭지점 구하기)
        GroundMax.x = GroundObj.transform.position.x + GrdHalfSize.x;
        GroundMax.y = GroundObj.transform.position.y + GrdHalfSize.y;
        GroundMax.z = GroundObj.transform.position.z + GrdHalfSize.z;
        //------------------- LimitMoveCam(카메라가 지형 밖으로 나갈 수 없도록 막기)


    }

    // Update is called once per frame
    void LateUpdate() // 
    {
        newPosition = transform.position;
        newPosition.x = Mathf.SmoothDamp(transform.position.x,
            m_HeroObj.transform.position.x, ref xVelocity, smoothTime);  //(현재 좌표에서 목표 좌표까지 0.2초로 이동) (현재좌표, 목표좌표, ref ~~, 이동시간)
        newPosition.z = Mathf.SmoothDamp(transform.position.z,           //(ref -> 함수로부터 값을 받는것)
            m_HeroObj.transform.position.z, ref zVelocity, smoothTime);
        transform.position = newPosition;

        //transform.position = new Vector3(
        //                    m_HeroObj.transform.position.x, 
        //                    transform.position.y,
        //                    m_HeroObj.transform.position.z);

        // ---------------------- PC에서만 작동되는 줌인 줌아웃 기능





        if (Input.GetAxis("Mouse ScrollWheel") < 0 && distance < maxDist) // GetAxis -> 소수점까지나옴
        {
            distance += zoomSpeed;
            RefCam.orthographicSize = distance;
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0 && distance > minDist)
        {
            distance -= zoomSpeed;
            RefCam.orthographicSize = distance;
        }
        // ---------------------- PC에서만 작동되는 줌인 줌아웃 기능


        //------------------- LimitMoveCam(카메라가 지형 밖으로 나갈 수 없도록 막기)
        //a_ScMin = new Vector3(0.0f, 0.0f, 0.0f); //ScreenViewPort 좌측하단
        m_CamWMin = Camera.main.ViewportToWorldPoint(a_ScMin); // 카메라의 min max 값을 얻어옴
        //카메라 화면 좌측하단 코너의 월드 좌표
        //MinX : m_CamWMin.x,  MinZ : m_CamWMin.z

        //Vector3 a_ScMax = new Vector3(1.0f, 1.0f, 0.0f); //ScreenViewPort 우측상단
        m_CamWMax = Camera.main.ViewportToWorldPoint(a_ScMax);
        //카메라 화면 우측상단 코너의 월드 좌표
        //MaxX : m_CamWMax.x,  MaxZ : m_CamWMax.z

        m_ScWdHalf.x = (m_CamWMax.x - m_CamWMin.x) / 2.0f;
        m_ScWdHalf.z = (m_CamWMax.z - m_CamWMin.z) / 2.0f;

        a_LmtBdLeft = GroundMin.x + 4.0f + m_ScWdHalf.x;
        a_LmtBdTop = GroundMin.z + 4.0f + m_ScWdHalf.z;
        a_LmtBdRight = GroundMax.x - 4.0f - m_ScWdHalf.x;
        a_LmtBdBottom = GroundMax.z - 4.0f - m_ScWdHalf.z;

        if (a_LmtBdRight <= a_LmtBdLeft)
            newPosition.x = transform.position.x;
        else
        {
            if (newPosition.x < (float)a_LmtBdLeft)
                newPosition.x = (float)a_LmtBdLeft;

            if ((float)a_LmtBdRight < newPosition.x)
                newPosition.x = (float)a_LmtBdRight;
        }

        if (a_LmtBdBottom <= a_LmtBdTop)
            newPosition.z = transform.position.z;
        else
        {
            if (newPosition.z < (float)a_LmtBdTop)
                newPosition.z = (float)a_LmtBdTop;

            if ((float)a_LmtBdBottom < newPosition.z)
                newPosition.z = (float)a_LmtBdBottom;
        }
        //------------------- LimitMoveCam(카메라가 지형 밖으로 나갈 수 없도록 막기)

        transform.position = newPosition;
    }

    public bool IsScreenOut(Vector3 a_CkVec, float Margin = 0) // Margin = 여백
    {
        if(a_CkVec.x < m_CamWMin.x + Margin ||
           m_CamWMax.x - Margin < a_CkVec.x ||
           a_CkVec.z < m_CamWMin.z + Margin ||
           m_CamWMax.z - Margin < a_CkVec.z)
        {
            return true; // 화면 밖
        }

        return false;    // 화면 안
    }
}
