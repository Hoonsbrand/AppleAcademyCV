using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MonAIState
{
    MAI_Idle,           // 숨쉬기 상태
    MAI_Patrol,         // 패트롤 상태
    MAI_AggroTrace,     // 적으로부터 공격을 당했을 때 추적 상태
    MAI_NormalTrace,    // 일반 추적 상태
    MAI_ReturnPos,      // 추적 놓쳤을 때 제자리로 돌아오는 상태
    MAI_Attack,         // 공격 상태
}
public class MonsterCtrl : MonoBehaviour
{
    //---------- 몬스터 AI 변수들...
    MonAIState m_AIState = MonAIState.MAI_Patrol;
    GameObject m_AggroTarget = null;
    Vector3 a_CacVLen = Vector3.zero;
    float a_CacDist = 0.0f;
    float m_AttackDist = 20.0f;             //공격거리
    float m_TraceDist = 25.0f;              //추적거리

    Vector3 m_BasePos = Vector3.zero;    //몬스터의 초기 스폰 위치(기준점이 된다.)
    bool m_bMvPtOnOff = false;           //Patrol Move
    float m_WaitTime = 0.0f;             //Patrol시에 목표점에 도착하면 잠시 대기시키기 위한 랜덤 시간변수
    Vector3 m_PatrolTarget = Vector3.zero; //Patrol시 움직여야될 다음 목표 좌표
    Vector3 m_DirMvVec = Vector3.zero;     //Patrol시 움직여야될 방향 벡터
    private double m_MoveDurTime = 0.0;    //목표점까지 도착하는데 걸리는 시간
    private double m_AddTimeCount = 0.0;   //누적시간 카운트 
    float m_NowStep = 0.0f;
    float m_MoveVelocity = 2.0f;           //평면 초당 이동 속도...
    Vector3 a_MoveNextStep = Vector3.zero;
    Vector3 m_MoveDir = Vector3.zero;   //평면 진행 방향
    float m_ShootCool = 1.0f;   //공격 쿨타임 (공격 주기)

    //Patrol시 계산용 변수
    Vector3 a_CacEndVec = Vector3.zero;
    Vector3 a_DirPtVec = Vector3.zero;
    Quaternion a_CacPtRot = Quaternion.identity;
    Vector3 a_CacPtAngle = Vector3.zero;
    Vector3 a_Vect = Vector3.zero;
    int a_AngleRan = 0;
    int a_LengthRan = 0;
    //Patrol시 계산용 변수
    //---------- 몬스터 AI 변수들...

    InGame_Mgr m_GameMgr = null;

    private float m_MaxHP;
    [HideInInspector] public float m_CurHP;
    public Image m_HPSdBar = null; // using UnityEngine.UI; 필요

    // Start is called before the first frame update
    void Start()
    {
        m_MaxHP = 100.0f;
        m_CurHP = m_MaxHP;

        //---------- 몬스터 AI 변수들 초기화
        m_BasePos = this.transform.position;
        m_bMvPtOnOff = false; //Patrol Move
        m_WaitTime = Random.Range(0.5f, 3.0f);
        //---------- 몬스터 AI 변수들 초기화

        GameObject a_GameMgrObj = GameObject.Find("InGame_Mgr");
        if (a_GameMgrObj != null)
            m_GameMgr = a_GameMgrObj.GetComponent<InGame_Mgr>();

    }

    // Update is called once per frame
    void Update()
    {
        MonsterAI();
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

        m_AggroTarget = m_GameMgr.m_refHero.gameObject;
        m_AIState = MonAIState.MAI_AggroTrace;

        if(m_CurHP <= 0.0f)       // 몬스터 사망처리
        {
            m_GameMgr.AddMonKill();   // 몬스터 kill count + 1

            ItemDrop();

            Destroy(gameObject);  // 몬스터 GameObject 제거됨
        }
    }

    void MonsterAI()
    {
        m_ShootCool = m_ShootCool - Time.deltaTime;
        if (m_ShootCool < 0.0f)
            m_ShootCool = 0.0f;

        if (m_AIState == MonAIState.MAI_Patrol)
        {
            //---패트롤 상태라고 하더라도 20m 안쪽으로 적이 접근하면 공격하겠다는 코드
            if (m_GameMgr != null && m_GameMgr.m_refHero != null)
            {
                a_CacVLen = m_GameMgr.m_refHero.transform.position -
                                                       this.transform.position;
                a_CacVLen.y = 0.0f;

                a_CacDist = a_CacVLen.magnitude;

                if (a_CacDist < m_AttackDist) //공격거리
                {
                    m_AIState = MonAIState.MAI_NormalTrace;
                    //일반 추적모드로 돌아가면 공격범위안에 있기 때문에 바로 공격할 것이다.
                    m_AggroTarget = m_GameMgr.m_refHero.gameObject; // 추적해야할 대상

                    return;
                }
            }
            //---패트롤 상태라고 하더라도 20m 안쪽으로 적이 접근하면 공격하겠다는 코드

            if (m_bMvPtOnOff == true) // 왔다 갔다 하다가 쉬다가 다시..
            {
                m_NowStep = Time.deltaTime * m_MoveVelocity;
                //이번에 한걸음 길이 (보폭)

                a_CacEndVec = m_PatrolTarget - this.transform.position;
                a_CacEndVec.y = 0.0f;
                m_DirMvVec = a_CacEndVec.normalized;
                //몬스터끼리 충돌되더라도 타겟으로 이동하기 위한 처리
                //몬스터끼리 충돌되서 목표점에 도착하지 못하는 
                //문제를 해결하기 위해서는 목표점까지 도착하는 시간을 구한 후
                //구한 시간만큼만 이동시키고 도착으로 판정한다.

                m_AddTimeCount = m_AddTimeCount + Time.deltaTime;
                if (m_MoveDurTime <= m_AddTimeCount)
                //목표점에 도착한 것으로 판정한다.
                {
                    m_WaitTime = Random.Range(0.2f, 3.0f);
                    m_bMvPtOnOff = false;
                }
                else
                {
                    this.transform.position = this.transform.position +
                                                    (m_DirMvVec * m_NowStep);
                }
            }
            else
            {
                m_WaitTime = m_WaitTime - Time.deltaTime;
                if (0.0f < m_WaitTime)
                {
                    //숨쉬기 애니메이션으로 바꿔주는 부분
                }
                else
                {
                    m_WaitTime = 0.0f;
                    a_AngleRan = Random.Range(30, 301);
                    a_LengthRan = Random.Range(3, 8);

                    a_DirPtVec = this.transform.position - m_BasePos;
                    a_DirPtVec.y = 0.0f;
                    if (a_DirPtVec.magnitude < 1.0f) //처음 시작할 때 
                    {
                        a_CacPtRot =
                            Quaternion.LookRotation(this.transform.forward);
                    }
                    else
                    {
                        a_CacPtRot = Quaternion.LookRotation(a_DirPtVec);
                    }
                    a_CacPtAngle = a_CacPtRot.eulerAngles;
                    a_CacPtAngle.y = a_CacPtAngle.y + (float)a_AngleRan;
                    a_CacPtRot.eulerAngles = a_CacPtAngle;
                    a_Vect = new Vector3(0, 0, 1);   //a_CacPtRot 로 방향벡터를 만든다.
                    a_Vect = a_CacPtRot * a_Vect;    // Vector3 값
                    a_Vect.Normalize();

                    m_PatrolTarget = m_BasePos + (a_Vect * (float)a_LengthRan);

                    m_DirMvVec = m_PatrolTarget - this.transform.position;
                    m_DirMvVec.y = 0.0f;
                    m_MoveDurTime = m_DirMvVec.magnitude / m_MoveVelocity;
                    //목표지점에 도착하는데까지 걸리는 시간
                    m_AddTimeCount = 0.0;
                    m_DirMvVec.Normalize();

                    //m_WaitTime = Random.Range(0.2f, 3.0f);
                    m_bMvPtOnOff = true;
                }//else
            }//else
        }//if (m_AIState == MonAIState.MAI_Patrol)

        if (m_AIState == MonAIState.MAI_AggroTrace)
        {
            if (m_AggroTarget != null)
            {
                a_CacVLen = m_AggroTarget.transform.position -
                                                this.transform.position;
                a_CacVLen.y = 0.0f;

                a_CacDist = a_CacVLen.magnitude;
                m_MoveDir = a_CacVLen.normalized;

                if (a_CacDist < m_AttackDist) //공격거리
                {
                    m_AIState = MonAIState.MAI_NormalTrace;
                }

                if ((m_AttackDist - 2.0f) < a_CacDist) //공격거리 //else //추적거리이면서 공격거리가 아닐 때... 
                {
                    m_NowStep = m_MoveVelocity * 5.0f * Time.deltaTime; //한걸음 크기
                    a_MoveNextStep = m_MoveDir * m_NowStep;      //한걸음 벡터
                    a_MoveNextStep.y = 0.0f;

                    this.transform.position = this.transform.position +
                                                         a_MoveNextStep;
                }//else if (m_AttackDist <= a_CacDist ) //공격거리가 아닐 때  
            }
            else
            {
                m_AIState = MonAIState.MAI_Patrol;
                m_bMvPtOnOff = false;
            }
        }//if (m_AIState == MonAIState.MAI_AggroTrace)

        if (m_AIState == MonAIState.MAI_NormalTrace)
        {
            if (m_AggroTarget != null)
            {
                a_CacVLen = m_AggroTarget.transform.position - this.transform.position;
                a_CacVLen.y = 0.0f;

                a_CacDist = a_CacVLen.magnitude;

                if (a_CacDist < m_TraceDist) //추적거리
                {
                    m_MoveDir = a_CacVLen.normalized;

                    ////캐릭터 스프링 회전   
                    //a_TargetRot = Quaternion.LookRotation(m_MoveDir);
                    //transform.rotation = Quaternion.Slerp(transform.rotation, a_TargetRot, Time.deltaTime * m_RotSpeed);
                    ////캐릭터 스프링 회전   

                    if (a_CacDist < m_AttackDist) //공격거리
                    {
                        if (m_ShootCool <= 0.0f)
                        {
                            Shooting();
                            m_ShootCool = 0.5f;
                        }
                    }
                    if ((m_AttackDist - 2.0f) < a_CacDist) //공격거리 //else //추적거리이면서 공격거리가 아닐 때... 
                    {
                        m_NowStep = m_MoveVelocity * 1.5f * Time.deltaTime; //한걸음 크기
                        a_MoveNextStep = m_MoveDir * m_NowStep;  //한걸음 벡터
                        a_MoveNextStep.y = 0.0f;

                        this.transform.position = this.transform.position + a_MoveNextStep;

                    }//else if (m_AttackDist <= a_CacDist ) //공격거리가 아닐 때  

                }//if (a_CacDist < m_TraceDist) //추적거리
                else
                {
                    m_AIState = MonAIState.MAI_Patrol;
                    m_bMvPtOnOff = false;
                }
            }//if (m_AggroTarget != null)
            else
            {
                m_AIState = MonAIState.MAI_Patrol;
                m_bMvPtOnOff = false;
            }
        }//if (m_AIState == MonAIState.MAI_NormalTrace)
    }


    public virtual void Shooting()
    {
        if (m_AggroTarget == null)
            return;

        a_CacVLen = m_AggroTarget.transform.position - this.transform.position;
        a_CacVLen.y = 0.0f;
        Vector3 a_CacDir = a_CacVLen.normalized;

        GameObject newObj = (GameObject)Instantiate(InGame_Mgr.m_BulletObj);
        //오브젝트의 클론(복사체) 생성 함수   
        BulletCtrl a_BulletSC = newObj.GetComponent<BulletCtrl>();
        a_BulletSC.BulletSpawn(this.transform, a_CacDir, AllyType.AT_Enemy);
    }

    public void ItemDrop()
    {
        int a_Rnd = Random.Range(0, 6); 
        if(0 <= a_Rnd && a_Rnd < 6) // 6이 나오면 꽝
        {
            GameObject m_Item = null;
            m_Item = (GameObject)Instantiate(Resources.Load("Prefabs/Item_Obj")); // Load 는 Resources만 가능
            m_Item.transform.position = this.transform.position;
            int a_TexInx = a_Rnd;
            string a_ObjName = "Item";
            if(a_Rnd == 0)
            {
                a_ObjName = "coin_Item_Obj";
            }
            else if (a_Rnd == 1)
            {
                a_ObjName = "bomb_Item_Obj";
            }
            else if (a_Rnd == 2)
            {
                a_ObjName = "armor_Item_Obj";
            }
            else if (a_Rnd == 3)
            {
                a_ObjName = "boots_Item_Obj";
            }
            else if (a_Rnd == 4)
            {
                a_ObjName = "axe_Item_Obj";
            }
            else if (a_Rnd == 5)
            {
                a_ObjName = "helmets_Item_Obj";
            }

            Renderer a_RefRender = m_Item.GetComponent<Renderer>();
            a_RefRender.material.SetTexture("_MainTex", m_GameMgr.m_ItemImg[a_TexInx]);

            m_Item.name = a_ObjName;

            ItemObjInfo a_RefItemInfo = m_Item.GetComponent<ItemObjInfo>();
            if(a_RefItemInfo != null)
            {
                a_RefItemInfo.InitItem((Item_Type)a_TexInx, a_ObjName,
                                        Random.Range(1, 6), Random.Range(1, 6));
            }

            Destroy(m_Item.gameObject, 15.0f);

            
        }
    }
}

