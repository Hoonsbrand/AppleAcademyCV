using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrl : MonoBehaviour
{
    [HideInInspector] public float m_MaxHP;
    [HideInInspector] public float m_CurHP;
    public Image m_HPSdBar = null; // using UnityEngine.UI 필요

    //---------- 키보드 입력값 변수 선언
    float h, v;
    private float m_MoveSpeed = 10.0f; //초당 10픽셀을 이동해라 라는 속도 (이동속도)
    Vector3 MoveNextStep;            //보폭을 계산해 주기 위한 변수
    Vector3 MoveHStep;
    Vector3 MoveVStep;
    //---------- 키보드 입력값 변수 선언




    //----- 마우스 피킹 관련 변수들...
    private Vector3 m_DirVec;         //마우스피킹으로 이동하려는 방향 벡터

    private Vector3 a_CacPos;              // 벡터 계산용 변수
    private bool m_bMoveOnoff = false;     // 현재 마우스피킹으로 이동중인지? 의 여부
    private Vector3 m_TargetPos;           // 마우스피킹 목표점
    private double m_MoveDurTime = 0.0f;   // 목표점까지 도착하는데 걸리는 시간
    private double m_AddTimeCount = 0.0f;  // 누적시간 카운트
    private float a_CacStep;               // 계산용 변수

    float m_AttackDist = 14.0f;            // 공격 거리
    float m_ShootCool = 1.0f;              // 공격 쿨타임 (공격 주기)
    //----- 마우스 피킹 관련 변수들...

    // ------ 총알 발사 관련 변수 선언
    private Vector3 a_CurPos;
    private Vector3 a_CacEndVec;           // 범용으로 사용할 계산용 변수
    // ------ 총알 발사 관련 변수 선언

    // ---- JoyStick 이동 처리 변수
    private float m_JoyMvLen = 0.0f;
    private Vector3 m_JoyMvDir = Vector3.zero;
    // ---- JoyStick 이동 처리 변수

    // --------- LimitMove (주인공 캐릭터가 지형을 벗어나지 못하게 막기)
    Transform m_HeroMesh = null;
    Vector3 HalfSize = Vector3.zero;
    CameraCtrl RefCamCtrl = null;

    float a_LmtBdLeft = 0;
    float a_LmtBdTop = 0;
    float a_LmtBdRight = 0;
    float a_LmtBdBottom = 0;

    Vector3 m_CacCurPos = Vector3.zero;
    //  --------- LimitMove (주인공 캐릭터가 지형을 벗어나지 못하게 막기)

    Rigidbody playerRb;

    InGame_Mgr m_GameMgr = null;

    public Text m_NickName = null;

    // --------------- 애니메이션 관련 변수
    UnitSequence m_Seq;
    Quaternion m_CacRot;
    Vector3 m_Angle;
    bool m_IsAttack = false;
    // --------------- 애니메이션 관련 변수


    // Start is called before the first frame update
    void Start()
    {
        m_MaxHP = 200.0f;
        m_CurHP = m_MaxHP;

        //  --------- LimitMove (주인공 캐릭터가 지형을 벗어나지 못하게 막기)
        RefCamCtrl = FindObjectOfType<CameraCtrl>();
        m_HeroMesh = transform.Find("HeroMesh"); // 자식 오브젝트 찾기
        HalfSize.x = m_HeroMesh.localScale.x / 2.0f;
        HalfSize.y = m_HeroMesh.localScale.y / 2.0f;
        HalfSize.z = m_HeroMesh.localScale.z / 2.0f;
        //  --------- LimitMove (주인공 캐릭터가 지형을 벗어나지 못하게 막기)

        playerRb = gameObject.GetComponent<Rigidbody>();

        if (m_NickName != null)
            m_NickName.text = PlayerPrefs.GetString("UserNick", "Hero");

        m_Seq = gameObject.GetComponentInChildren<UnitSequence>();
        // 차일드 중 첫번째로 나오는 SequenceAni.cs 파일 접근법

        //GameObject a_GameMgrObj = GameObject.Find("InGame_Mgr");
        //if (a_GameMgrObj != null)
        //    m_GameMgr = a_GameMgrObj.GetComponent<InGame_Mgr>();

        //if (m_GameMgr != null)
        //    m_GameMgr.ReflashUserInfoHP();

    }

    // Update is called once per frame
    void Update()
    {
        if(m_GameMgr == null)
        {
            GameObject a_GameMgrObj = GameObject.Find("InGame_Mgr");
            if (a_GameMgrObj != null)
                m_GameMgr = a_GameMgrObj.GetComponent<InGame_Mgr>();

            if (m_GameMgr != null)
                m_GameMgr.ReflashUserInfoHP();
        }

        KeyBDMove();
        JoyStickMvUpdate(); // 우선순위가 중요함
        MsPickMove();

        //  --------- LimitMove (주인공 캐릭터가 지형을 벗어나지 못하게 막기)
        LimitMove();
        //  --------- LimitMove (주인공 캐릭터가 지형을 벗어나지 못하게 막기)

        // 조이스틱, 키보드, 마우스 움직임 없을 떄
        if(m_JoyMvLen <= 0.0f && (0.0f == h && 0.0f == v) && m_bMoveOnoff == false)
        {
            m_Seq.ReSetFrameAni(UnitState.Idle);
        }
        else
        {
            if(m_DirVec.magnitude <= 0.0f)
            {
                m_Seq.ReSetFrameAni(UnitState.Idle);
            }
            else
            {
                // 방향에 따른 애니메이션 구하는 곳
                m_CacRot = Quaternion.LookRotation(m_DirVec);
                m_Seq.CheckAnimDir(m_CacRot.eulerAngles.y);
                // 방향에 따른 애니메이션 구하는 곳
            }
        }

    }

    void KeyBDMove()   //키보드 이동처리
    {
        //-------------- 가감속 없이 이동 처리 하는 방법
        h = Input.GetAxisRaw("Horizontal");
        //화살표키 좌우키를 눌러주면 -1.0f, 0.0f, 1.0f 사이값을 리턴해 준다.
        v = Input.GetAxisRaw("Vertical");
        //화살표키 위아래키를 눌러주면 -1.0f, 0.0f, 1.0f 사이값을 리턴해 준다.
        //-------------- 가감속 없이 이동 처리 하는 방법

        if (0.0f != h || 0.0f != v) //키보드 이동처리
        {
            //Vector3.right == new Vector3(1.0f, 0.0f, 0.0f)
            //Vector3.forward == new Vector3(0.0f, 0.0f, 1.0f)
            MoveHStep = Vector3.right * h;
            MoveVStep = Vector3.forward * v;

            MoveNextStep = MoveHStep + MoveVStep;
            MoveNextStep = MoveNextStep.normalized *
                                        m_MoveSpeed * Time.deltaTime;

            transform.position = transform.position + MoveNextStep;
            //== transform.Translate(MoveNextStep);

            m_DirVec = MoveNextStep.normalized;
        }//if (0.0f != h || 0.0f != v)
    }//void KeyBDMove()

    public void MsPicking(Vector3 a_Pos) // 마우스 피킹 발생 함수
    {
        Vector3 a_CacVec = a_Pos - this.transform.position;
        a_CacVec.y = 0.0f;
        if (a_CacVec.magnitude < 1.0f)
        {
            return;
        }

        m_bMoveOnoff = true; // 플래그 변수

        m_DirVec = a_CacVec;
        m_DirVec.Normalize();  // 단위 벡터를 만든다.
        m_TargetPos = new Vector3(a_Pos.x, this.transform.position.y, a_Pos.z); // a_Pos : 목표점
    }

    void MsPickMove() // 마우스 피킹 이동 계산용 함수
    {
        if (0.0f < m_JoyMvLen || (h != 0.0f || v != 0.0f)) 
            m_bMoveOnoff = false; // 즉시 끄기

        if ((h != 0.0f || v != 0.0f))  // 키보드나 조이스틱으로 움직일 때
            m_bMoveOnoff = false;      // 즉시 끄기

        if(m_bMoveOnoff == true)
        {
            a_CacStep = Time.deltaTime * m_MoveSpeed;
            // 이번에 한걸음 길이 (보폭)

            a_CacPos = this.transform.position;
            a_CacEndVec = m_TargetPos - a_CacPos;
            a_CacEndVec.y = 0.0f;

            if(a_CacEndVec.magnitude <= a_CacStep)
            // 목표점까지의 거리보다 보폭이 크거나 같으면 도착으로 본다.
            {
                m_bMoveOnoff = false;
            }

            else
            {
                m_DirVec = a_CacEndVec;
                m_DirVec.Normalize();
                this.transform.position = a_CacPos + (m_DirVec * a_CacStep);

            }
        }
    }

    public bool IsKJMove()
    {
        if(0.0f < m_JoyMvLen || (0.0f != h || 0.0f != v))
        {
            return true;
        }

        return false;
    }

    public void MsShooting(Vector3 a_TPos) // 클릭이벤트가 발생했을 때 이 함수를 호출합니다.
    {
        // Instantiate 요청 함수
        GameObject newObj = Instantiate(InGame_Mgr.m_BulletObj);

        a_CurPos = transform.position;
        a_CacEndVec = a_TPos - a_CurPos;
        a_CacEndVec.y = 0.0f;
        Vector3 a_CacDir = a_CacEndVec.normalized; // 피킹으로 발사하는 경우

        BulletCtrl a_BulletSC = newObj.GetComponent<BulletCtrl>();
        a_BulletSC.BulletSpawn(this.transform, a_CacDir);
    }

    public void SetJoyStickMv(float a_JoyMvLen, Vector3 a_JoyMvDir) // 값을 받아오고 그것을 이동하는 좌표로 전환해준다
    {
        m_JoyMvLen = a_JoyMvLen;
        if(0.0f < a_JoyMvLen)
        {
            m_JoyMvDir = new Vector3(a_JoyMvDir.x, 0.0f, a_JoyMvDir.y);
        }
    } // publc void SetJoyStickMv 

    public void JoyStickMvUpdate()  // 위에서 얻어온 값으로 움직인다.
    {
        if (0.0f != h || 0.0f != v) // 키보드 우선권 부여
            return;

        /// --- 조이스틱 코드
        if(0.0f < m_JoyMvLen) // 조이스틱으로 움직일 때
        {
            m_DirVec = m_JoyMvDir;

            float amtToMove = m_MoveSpeed * Time.deltaTime;

            transform.Translate(m_JoyMvDir * m_JoyMvLen * amtToMove); // m_JoyMvLen 으로 인해 조이스틱을 얼마나 움직이냐에 따라 미세하게 속도가 달라진다.
        }
    } // public void JoyStickMvUpdate

    void LimitMove()
    {
        if (RefCamCtrl == null)
            return;

        m_CacCurPos = transform.position;

        a_LmtBdLeft = RefCamCtrl.GroundMin.x + 4.0f + HalfSize.x;
        a_LmtBdTop = RefCamCtrl.GroundMin.z + 4.0f + HalfSize.z;
        a_LmtBdRight = RefCamCtrl.GroundMax.x - 4.0f - HalfSize.x;
        a_LmtBdBottom = RefCamCtrl.GroundMax.z - 4.0f - HalfSize.z;

        if (m_CacCurPos.x < a_LmtBdLeft)
            m_CacCurPos.x = a_LmtBdLeft;

        if (a_LmtBdRight < m_CacCurPos.x)
            m_CacCurPos.x = a_LmtBdRight;

        if (m_CacCurPos.z < a_LmtBdTop)
            m_CacCurPos.z = a_LmtBdTop;

        if (a_LmtBdBottom < m_CacCurPos.z)
            m_CacCurPos.z = a_LmtBdBottom;

        transform.position = m_CacCurPos;
    }

    public void TakeDamage(float a_Value)
    {
        if (m_CurHP <= 0.0f)
            return;

        if (m_GameMgr != null)
            m_GameMgr.DamageTxt((int)a_Value, this.transform);

        m_CurHP = m_CurHP - a_Value;
        if (m_CurHP < 0.0f)
            m_CurHP = 0.0f;

        if (m_HPSdBar != null)
            m_HPSdBar.fillAmount = m_CurHP / m_MaxHP;

        if (m_GameMgr != null)
            m_GameMgr.ReflashUserInfoHP();

        if(m_CurHP <= 0.0f)
        {
            // Die
            m_CurHP = 0.0f;

            UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("coin_") == true)
        {
            m_GameMgr.AddGold(100); // 골드증가
            Destroy(other.gameObject);
        }
        else if(other.gameObject.name.Contains("bomb_") == true)
        {
            m_GameMgr.AddSkill(); // 스킬 증가
            Destroy(other.gameObject);
        }
        else if(other.gameObject.name.Contains("Item_Obj") == true)
        {
            GlobalUserData.InvenAddItem(other.gameObject);
            Destroy(other.gameObject);
        }
    }

}//public class HeroCtrl 
