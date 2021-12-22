using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // 이동 관련 변수

    float m_LifeTime = 2.0f;
    Vector3 m_OwnTrPos;                     // 발사한 캐릭터 위치
    Vector3 m_DirTgVec;                     // 날아가야 할 방향
    Vector3 a_StartPos = new Vector3(0, 0, 1); // 스폰 위치 

    Vector3 a_MoveNextStep;
    private float m_MoveSpeed = 20.0f; // 한프레임당 이동 시키고 싶은 거리

    // 이동 관련 변수

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_LifeTime = m_LifeTime - Time.deltaTime;
        if(m_LifeTime <= 0.0f)
        {
            ResetState();
        }

        a_MoveNextStep = m_DirTgVec * (Time.deltaTime * m_MoveSpeed);
        a_MoveNextStep.y = 0.0f;

        transform.position = transform.position + a_MoveNextStep;
        // = transform.Translate(a_MoveNextStep);
    }

    public void ResetState()
    {
        m_LifeTime = 0.0f;

        Destroy(this.gameObject);
    }

    public void BulletSpawn(Transform a_OwnTr, Vector3 a_DirVec)
    {
        m_OwnTrPos = a_OwnTr.position;

        a_DirVec.z = 0.0f;
        m_DirTgVec = a_DirVec;
        m_DirTgVec.Normalize(); // 벡터의 정규화

        a_StartPos = m_OwnTrPos + (m_DirTgVec * 0.5f); // 바라보는 방향 + 0.5 에서 발사

        transform.position = new Vector3(a_StartPos.x, a_StartPos.y, 0.0f);

        m_LifeTime = 2.0f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.name.Contains("Zombi") == true)
        {
            ResetState();
            Destroy(col.gameObject);
        }
        
    }
}
