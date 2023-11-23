using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;


public class UIScore : MonoBehaviour
{
    public GameObject player;
    public GameObject virtualCam;
    public GameObject gameWinScreen;

    public int currentStage;
    public int fixCount = 0;
    private int maxRobots;

    public TMP_Text counterText;

    public static UIScore instance { get; private set; }

    public List<GameObject> stages;

    //private GameObject BoundryEmptyObject;
    //public PolygonCollider2D BoundryShape;
    CinemachineConfiner2D confiner2D;
    //Rigidbody rbt;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //set Ruby to the stage defined by currentStage
        player.transform.SetParent(stages[currentStage].transform,true);
        player.transform.position=stages[currentStage].transform.Find("StartPoint").transform.position;

        //set camera confiner to current stage
        confiner2D = virtualCam.GetComponent<CinemachineConfiner2D>();
        confiner2D.m_BoundingShape2D = stages[currentStage].transform.Find("CameraConfiner").GetComponent<PolygonCollider2D>();




        maxRobots = player.transform.parent.transform.Find("Robots").transform.childCount;

        counterText = GetComponent<TextMeshProUGUI>();
        counterText.text=(0+"/"+maxRobots);
    }


    public void SetValue(int value)
    {
       
  

        fixCount += value;
        if (fixCount<maxRobots)
        {
            counterText.text=(fixCount.ToString()+"/"+maxRobots);
        }
        else
        {
            
            
            counterText.text=(maxRobots+"!");

            currentStage += 1;
            if (currentStage<(stages.Count))
            {

                //some text prompt here to progress to next stage, or transition to next stage automatically

                //advance one stage ahead then move Ruby
                fixCount = 0;
                player.transform.SetParent(stages[currentStage].transform,true);
                player.transform.position=stages[currentStage].transform.Find("StartPoint").transform.position;

                //update counter to the new stage's robots:
                maxRobots = player.transform.parent.transform.Find("Robots").transform.childCount; 
                counterText.text=(0+"/"+maxRobots);

                //set camera confiner to current stage
                confiner2D = virtualCam.GetComponent<CinemachineConfiner2D>();
                confiner2D.m_BoundingShape2D = stages[currentStage].transform.Find("CameraConfiner").GetComponent<PolygonCollider2D>();
            }
            else
            {
               gameWinScreen.SetActive(true);
               RubyController.gameOver = true; 
            }

        }
    }

}
