using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class roomTimer : MonoBehaviour {

    float timeAmount = 0.0f;
    bool beginTime = false;

    public float turnOrange = 70f;
    public float turnRed = 20f;

    void OnEnable ()
    {
        EventManager.Instance.StartListening<StartTimer> (StartTimer);
    }

    void OnDisable ()
    {
        EventManager.Instance.StopListening<StartTimer> (StartTimer);
    }

    public void StartTimer(StartTimer e) {

        if (e.Time > 0f) {
            timeAmount = e.Time;
        }

        beginTime = beginTime ? false : true;

        if (!beginTime) {
            StartCoroutine(doneLevel(4f));
        }

    }

    void FixedUpdate() {
        if (beginTime) {

            string timeStr = timeAmount.ToString("n2");
			string seconds;
			if (timeAmount <= 100) {
				seconds = timeStr.Substring (0, 2);
			} else {
				seconds = timeStr.Substring (0, 3);
			
			}
			string miliseconds;
			if (timeAmount <= 100) {
				miliseconds = timeStr.Substring (2);
			} else {
				miliseconds = timeStr.Substring (3);
			}

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
                GameManager.Instance.RestartGame();
            }
        }
    }

    IEnumerator doneLevel(float secondsToWait) {

        GetComponent<RectTransform>().localPosition = new Vector3(0, -55f, 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, 250);
        string timeStr = timeAmount.ToString("n2");
		string seconds;
		if (timeAmount <= 100) {
			seconds = timeStr.Substring (0, 2);
		} else {
			seconds = timeStr.Substring (0, 3);

		}
		string miliseconds;
		if (timeAmount <= 100) {
			miliseconds = timeStr.Substring (2);
		} else {
			miliseconds = timeStr.Substring (3);
		}

        string color1 = "";
        string color2 = "";

        if (timeAmount < 10f) {
            seconds = timeStr.Substring(0, 1);
            miliseconds = timeStr.Substring(1);
        }

        string completed = "";
        int i = 0;
        foreach (char c in "Completed!") {

            switch (i) {
                case 0:
                    completed += "<color=#FF0000><size=100>" + c + "</size></color>";
                    break;
                case 1:
                    completed += "<color=#ff7200><size=100>" + c + "</size></color>";
                    break;
                case 2:
                    completed += "<color=#ffcc00><size=100>" + c + "</size></color>";
                    break;
                case 3:
                    completed += "<color=#eeff00><size=100>" + c + "</size></color>";
                    break;
                case 4:
                    completed += "<color=#aeff00><size=100>" + c + "</size></color>";
                    break;
                case 5:
                    completed += "<color=#6aff00><size=100>" + c + "</size></color>";
                    break;
                case 6:
                    completed += "<color=#00ff3f><size=100>" + c + "</size></color>";
                    break;
                case 7:
                    completed += "<color=#00ffd0><size=100>" + c + "</size></color>";
                    break;
                case 8:
                    completed += "<color=#00ffff><size=100>" + c + "</size></color>";
                    break;
                case 9:
                    completed += "<color=#00b2ff><size=100>" + c + "</size></color>";
                    break;
                default:
                    break;
            }

            i++;
        }

        GetComponent<Text>().lineSpacing = 1.5f;
        GetComponent<Text>().text = completed + "\n" + "<color=white><size=80>" + seconds  + "</size></color>" + "<color=white><size=40>" + miliseconds + " seconds left</size></color>";
        yield return new WaitForSeconds(secondsToWait);
        GetComponent<Text>().text = "";
    }
}
