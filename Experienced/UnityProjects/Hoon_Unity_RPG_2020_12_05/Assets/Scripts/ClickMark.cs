using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMark : MonoBehaviour
{
    float m_AddTimer = 0.0f;
    bool m_IsOnoff = true;
    Renderer m_RefRemder;
    Color32 m_WtColor = new Color32(255, 255, 255, 200);
    Color32 m_BrColor = new Color32(0, 130, 255, 200);

    // Start is called before the first frame update
    void Start()
    {
        m_RefRemder = this.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_AddTimer = m_AddTimer + Time.deltaTime;
        if(0.5f <= m_AddTimer)
        {
            m_IsOnoff = !m_IsOnoff;
            if (m_RefRemder != null)
            {
                if (m_IsOnoff == true)
                    m_RefRemder.material.SetColor("_TintColor", m_WtColor);

                else
                    m_RefRemder.material.SetColor("_TintColor", m_BrColor);
            }

            m_AddTimer = 0.0f;
        }
    }

    public void ResetEff()
    {
        m_AddTimer = 0.0f;
        m_IsOnoff = true;
        if(m_RefRemder == null)
        {
            m_RefRemder = this.gameObject.GetComponent<Renderer>();
        }

        m_RefRemder.material.SetColor("_TintColor", m_WtColor);
    }

   
}
