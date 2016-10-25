using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class roomTimer : MonoBehaviour {

    float timeAmount = 0.0f;
    bool beginTime = false;

    public float turnOrange = 20f;
    public float turnRed = 10f;

    void OnEnable ()
    {
        EventManager.Instance.StartListening<StartTimer> (StartTimer);
    }

    void OnDisable ()
    {
        EventManager.Instance.StopListening<StartTimer> (StartTimer);
    }

    public void StartTimer(StartTimer e) {

        timeAmount = e.Time;
        beginTime = true;

    }

    void FixedUpdate() {
        if (beginTime) {

            string timeStr = timeAmount.ToString("n2");
            string seconds = timeStr.Substring(0, 2);
            string miliseconds = timeStr.Substring(2);

            string color1 = "";
            string color2 = "";

            if (timeAmount < 10f) {
                seconds = timeStr.Substring(0, 1);
                miliseconds = timeStr.Substring(1);
            }

            if (timeAmount > turnOrange) {
                //Green color text
                color1 = "<color=#00CF00><size=80>";
                color2 = "<color=#7fe77f><size=40>";
            } else if (timeAmount > turnRed) {
                //Yellow color text
                color1 = "<color=#ffff1c><size=80>";
                color2 = "<color=#ffff81><size=40>";           
            } else {
                //Red color text
                color1 = "<color=#ff1919><size=80>";
                color2 = "<color=#ff6666><size=40>";               
            }

            GetComponent<Text>().text = color1 + seconds  + "</size></color>" + color2 + miliseconds + "</size></color>";
            timeAmount -= Time.deltaTime;

            if (timeAmount < -0.01f) {
                beginTime = false;
                GetComponent<Text>().text = "<color=#ff1919><size=80>You Died!</size></color>";
                EventManager.Instance.TriggerEvent(new DeadEvent());
            }
        }
    }
}
