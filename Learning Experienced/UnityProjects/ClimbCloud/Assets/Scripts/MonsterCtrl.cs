using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    float m_MinX = 0.0f;
    float m_MaxX = 1.0f;

    int Dir = 1;//오른쪽 //왼쪽
    float MoveSpd = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //----자기 발 밑에 있는 구름 찾기
        GameObject[] a_CloudList;
        a_CloudList = GameObject.FindGameObjectsWithTag("CloudObj");
        foreach (GameObject a_CdObj in a_CloudList)
        {
            float a_CacLen = this.transform.position.y - a_CdObj.transform.position.y;
            if (0.0f < a_CacLen && a_CacLen < 3.0f)
            {
                //---스프라이트의 실제 크기 얻어오는 방법
                Vector2 a_CcSize; //= a_CdObj.GetComponent<SpriteRenderer>().size;
                float a_CdSizeX; //= a_CcSize.x * a_CdObj.transform.localScale.x; //구름 사이즈

                //float a_MinX = a_CdObj.transform.position.x - (a_CdSizeX / 2.0f);
                //float a_MaxX = a_CdObj.transform.position.x + (a_CdSizeX / 2.0f);

                Vector2 a_PrtColSize = a_CdObj.GetComponent<BoxCollider2D>().size;
                a_PrtColSize.x = a_PrtColSize.x * a_CdObj.transform.localScale.x; //충돌박스의 크기

                float a_MinX = a_CdObj.transform.position.x - (a_PrtColSize.x / 2.0f);
                float a_MaxX = a_CdObj.transform.position.x + (a_PrtColSize.x / 2.0f);

                if (a_MinX < this.transform.position.x && this.transform.position.x < a_MaxX)
                {
                    //---연이은 구름이 있는지 찾는 알고리즘 만들기...
                    FindContinueColl(ref a_MinX, ref a_MaxX, a_CloudList, a_CdObj);
                    //---연이은 구름이 있는지 찾는 알고리즘 만들기...

                    a_CcSize = gameObject.GetComponent<SpriteRenderer>().size; //몬스터 사이즈
                    a_CdSizeX = a_CcSize.x * gameObject.transform.localScale.x;

                    m_MinX = a_MinX + (a_CdSizeX / 2.0f);
                    m_MaxX = a_MaxX - (a_CdSizeX / 2.0f);

                    break;
                }
            }
        }
        //----자기 발 밑에 있는 구름 찾기

        Dir = Random.Range(0, 2);
        if (Dir == 0)
        {
            Dir = -1;
        }
        else
        {
            Dir = 1;
        }

        MoveSpd = Random.Range(0.4f, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_MaxX <= this.transform.position.x)
        {
            Dir = -1;
            gameObject.GetComponent<SpriteRenderer>().flipX = false; //스프라이트 좌우 반전 시키기...

        }
        if (this.transform.position.x < m_MinX)
        {
            Dir = 1;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        transform.position += (Dir * Vector3.right * MoveSpd * Time.deltaTime);
    }

    //---연이은 구름이 있는지 찾는 알고리즘 만들기...
    void FindContinueColl(ref float a_MinX, ref float a_MaxX, GameObject[] a_CloudList, GameObject a_PrtObj)
    {
        Vector2 a_PrtColSize = a_PrtObj.GetComponent<BoxCollider2D>().size;
        a_PrtColSize.x = a_PrtColSize.x * a_PrtObj.transform.localScale.x;
        a_PrtColSize.y = a_PrtColSize.y * a_PrtObj.transform.localScale.y; //충돌박스의 크기

        Vector2 a_PrtLTPos;
        a_PrtLTPos.x = a_PrtObj.transform.position.x - (a_PrtColSize.x / 2.0f);
        a_PrtLTPos.y = a_PrtObj.transform.position.y + (a_PrtColSize.y / 2.0f);

        //---연이은 구름이 있는지 찾는 알고리즘 만들기...
        GameObject a_CldObj = null;
        for (int ii = 0; ii < a_CloudList.Length; ii++)
        {
            a_CldObj = a_CloudList[ii];

            if (a_PrtObj == a_CldObj)
                continue;

            Vector2 a_CdColSize = a_CldObj.GetComponent<BoxCollider2D>().size;
            a_CdColSize.x = a_CdColSize.x * a_CldObj.transform.localScale.x;
            a_CdColSize.y = a_CdColSize.y * a_CldObj.transform.localScale.y; //충돌박스의 크기

            Vector2 a_CdLTPos;
            a_CdLTPos.x = a_CldObj.transform.position.x - (a_CdColSize.x / 2.0f);
            a_CdLTPos.y = a_CldObj.transform.position.y + (a_CdColSize.y / 2.0f);

            Vector2 a_CdRBPos;
            a_CdRBPos.x = a_CldObj.transform.position.x + (a_CdColSize.x / 2.0f);
            a_CdRBPos.y = a_CldObj.transform.position.y - (a_CdColSize.y / 2.0f);

            if (a_CdLTPos.y - 0.1f < a_PrtLTPos.y && a_PrtLTPos.y < a_CdLTPos.y + 1.0f)
            {
                if (a_MinX <= a_CdLTPos.x && a_CdRBPos.x <= a_MaxX)
                { //이미 반영된 오브젝트라면 스킵
                    continue;
                }

                if (a_MinX <= a_CdRBPos.x && a_CdLTPos.x <= a_MaxX) //충돌중
                {
                    if (a_CdLTPos.x < a_MinX)
                    {
                        a_MinX = a_CdLTPos.x;
                    }
                    if (a_MaxX < a_CdRBPos.x)
                    {
                        a_MaxX = a_CdRBPos.x;
                    }

                    ii = 0;  //처음부터 다시 찾는다.
                }// if (a_MinX <= a_CdRBPos.x && a_CdLTPos.x <= a_MaxX) //충돌중
            }//if(a_CdLTPos.y - 0.1f < a_PrtLTPos.y && a_PrtLTPos.y < a_CdLTPos.y + 1.0f)
        }//foreach (GameObject a_CldObj in a_CloudList)
        //---연이은 구름이 있는지 찾는 알고리즘 만들기...
    }//void FindContinueColl(
}
