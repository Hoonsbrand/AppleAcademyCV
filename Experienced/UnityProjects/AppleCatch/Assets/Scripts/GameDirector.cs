using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject timerText;
    GameObject pointText;
    float time = 30.0f;
    int point = 0;
    GameObject generator;

    public void GetApple()
    {
        this.point += 100;
    }

    public void GetBomb()
    {
        this.point /= 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.Find("ItemGenerator");
        this.timerText = GameObject.Find("Time");
        this.pointText = GameObject.Find("Point");
    }

    // Update is called once per frame
    void Update()
    {
        this.time -= Time.deltaTime;

        if (time < 0)
        {
            time = 0;
            generator.GetComponent<ItemGenerator>().SetParameter(100000.0f, 0, 0);
        }
        else if(0 <= time && time < 5)
        {
            generator.GetComponent<ItemGenerator>().SetParameter(0.9f, -0.04f, 6);
        }
        else if(5 <= time && time < 10)
        {
            generator.GetComponent<ItemGenerator>().SetParameter(0.4f, -0.06f, 6);
        }
        else if (10 <= time && time < 20)
        {
            generator.GetComponent<ItemGenerator>().SetParameter(0.7f, -0.04f, 4);
        }
        else if (20 <= time && time < 30)
        {
            generator.GetComponent<ItemGenerator>().SetParameter(1.0f, -0.03f, 2);
        }

        this.timerText.GetComponent<Text>().text = this.time.ToString("F1");
        this.pointText.GetComponent<Text>().text = this.point.ToString() + " point";

    } 
}
