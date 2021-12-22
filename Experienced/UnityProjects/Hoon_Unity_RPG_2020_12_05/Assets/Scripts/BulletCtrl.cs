using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AllyType
{
    AT_Ally,   // 아군    
    AT_Enemy,  // 적군
}

public class BulletCtrl : MonoBehaviour
{
    float m_LifeTime = 4.0f;
    Vector3 m_DirTgVec;
    Vector3 a_MoveNextStep;
    private float m_MoveSpeed = 35.0f; // 한 프레임당 이동시키고 싶은 거리

    Vector3 m_OwnTrPos;
    Vector3 a_StartPos = new Vector3(0, 0, 1);

    TrailRenderer m_TRenderer;

    AllyType m_AllyType = AllyType.AT_Ally; // enum 변수

    // Start is called before the first frame update
    void Start()
    {
        m_TRenderer = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_LifeTime = m_LifeTime - Time.deltaTime;
        if (m_LifeTime <= 0.0f)
        {
            ResetState();
        }

        if(m_TRenderer != null)
        {
            if(m_AllyType == AllyType.AT_Enemy)
            {
                if (m_TRenderer.time != 1.0f)
                    m_TRenderer.time = 1.0f;
                // 총알이 움직이기 시작할때에만 꼬리를 복원한다
            }
            else if(m_AllyType == AllyType.AT_Ally)
            {
                m_TRenderer.time = -1.0f; // 지금 나는 그냥 트레일 끄기
            }
        }

        a_MoveNextStep = m_DirTgVec * (Time.deltaTime * m_MoveSpeed);
        a_MoveNextStep.y = 0.0f;

        transform.position = transform.position + a_MoveNextStep;

        // ---- 화면 밖으로 나간 총알 제거해 주기
        if (m_AllyType == AllyType.AT_Ally)
            if (InGame_Mgr.RefCamCtrl.IsScreenOut(this.transform.position, 0.5f) == true)
                ResetState(); // 화면 밖에서 총알을 맞았다면 데미지가 먹지 않도록 처리
        // ---- 화면 밖으로 나간 총알 제거해 주기

    }

    public void ResetState()
    {
        m_LifeTime = 0.0f;
        Destroy(this.gameObject);
        // 게임오브젝트를 파괴하고자 할 때 사용하는 함수
    }

    public void BulletSpawn(Transform a_OwnTr, Vector3 a_DirVec, AllyType a_AllyType = AllyType.AT_Ally)
    {
        m_OwnTrPos = a_OwnTr.position;

        a_DirVec.y = 0.0f;
        m_DirTgVec = a_DirVec;
        m_DirTgVec.Normalize();

        a_StartPos = m_OwnTrPos + (m_DirTgVec * 2.5f);

        transform.position = new Vector3(a_StartPos.x, transform.position.y, a_StartPos.z);
        transform.rotation = Quaternion.LookRotation(m_DirTgVec);
        // <-- 총알이 날아가는 방향을 바라보게 회전시켜주는 부분

        m_LifeTime = 4.0f;

        // --- Trail 초기화
        if (m_TRenderer == null)
        {
            m_TRenderer = GetComponentInChildren<TrailRenderer>();
        }
        if (m_TRenderer != null)
        {
            m_TRenderer.time = -1.0f;
        }
        // --- Trail 초기화

        m_AllyType = a_AllyType;
    }

    private void OnTriggerEnter(Collider other) // 총알이 뭔가에 충돌 되었을 때 발생되는 함수
    {
        if(other.gameObject.name.Contains("Monster") == true) // 맞은 객체
        {
            if(m_AllyType == AllyType.AT_Ally) // 유저가 쏜 총알
            {
                ResetState();

                MonsterCtrl a_Enemy = other.gameObject.GetComponent<MonsterCtrl>();

                if (a_Enemy != null)
                    a_Enemy.TakeDamage(10.0f);
            }
        }

        if(other.gameObject.name.Contains("HeroRoot") == true)
        {
            if(m_AllyType == AllyType.AT_Enemy)
            {
                ResetState(); //자기 자신(총알)도 삭제

                HeroCtrl a_refHero = other.gameObject.GetComponent<HeroCtrl>();
                if (a_refHero != null)
                    a_refHero.TakeDamage(2.0f);
            }
        }
    }

}
