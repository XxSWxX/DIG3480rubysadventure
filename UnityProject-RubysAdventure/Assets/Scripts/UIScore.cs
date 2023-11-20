using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScore : MonoBehaviour
{
    public int fixCount = 0;
    public int maxRobots;

    public TMP_Text counterText;

    public static UIScore instance { get; private set; }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        counterText = GetComponent<TextMeshProUGUI>();
    }

    public void SetValue(int value)
    {

        fixCount += value;
        Debug.Log(GameObject.FindGameObjectsWithTag("Robot").Length);
        counterText.text=fixCount.ToString();

    }

}
