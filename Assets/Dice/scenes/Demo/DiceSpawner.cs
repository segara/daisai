using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSpawner : MonoBehaviour {
    [SerializeField] private GameObject spawnPoint1 = null;
    [SerializeField] private GameObject spawnPoint2 = null;
    [SerializeField] private GameObject spawnPoint3 = null;

    private int diceVal1 = 1;
    private int diceVal2 = 1;
    private int diceVal3 = 1;

    [SerializeField] GameObject dice1;
    [SerializeField] GameObject dice2;
    [SerializeField] GameObject dice3;
    [SerializeField]  GameObject dicesParent;
    Vector3 forceSaver_first = Vector3.zero;
    Vector3 forceSaver_second = Vector3.zero;
    Vector3 forceSaver_third = Vector3.zero;
    Vector3 torqueSaver_first  = Vector3.zero;
    Vector3 torqueSaver_second = Vector3.zero;
    Vector3 torqueSaver_third  = Vector3.zero;

    int predict_number1 = 1;
    int predict_number2 = 2;
    int predict_number3 = 3;


    // Update is called once per frame
    void Update () {
        UpdateRoll();
    }

    string randomColor
    {
        get
        {
            string _color = "blue";
            int c = System.Convert.ToInt32(Random.value * 6);
            switch (c)
            {
                case 0: _color = "red"; break;
                case 1: _color = "green"; break;
                case 2: _color = "blue"; break;
                case 3: _color = "yellow"; break;
                case 4: _color = "white"; break;
                case 5: _color = "black"; break;
            }
            return _color;
        }
    }

    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.right * -15f + new Vector3(- 7f * Random.value, 7f * Random.value, -3 * Random.value);
        return rollTarget;
    }

    private Vector3 Torque(Transform die_transform)
    {
        Vector3 torqueTarget = new Vector3(-50 * Random.value * die_transform.localScale.magnitude, -50 * Random.value * die_transform.transform.localScale.magnitude, -50 * Random.value * die_transform.transform.localScale.magnitude);
        return torqueTarget;
    }

    void UpdateRoll()
    {
        //spawnPoint = GameObject.Find("spawnPoint");
        // check if we have to roll dice
       if (Input.GetMouseButtonDown(Dice.MOUSE_LEFT_BUTTON)){
            forceSaver_first = Force();
            forceSaver_second = Force();
            forceSaver_third = Force();
            torqueSaver_first = Torque(dice1.transform);
            torqueSaver_second = Torque(dice2.transform);
            torqueSaver_third = Torque(dice3.transform);
       }
     
        if (Input.GetMouseButtonDown(Dice.MOUSE_RIGHT_BUTTON))
        {
           
            Dice.ClearDataOnly();

            dice1.transform.GetChild(0).localEulerAngles = Vector3.zero;
            dice2.transform.GetChild(0).localEulerAngles = Vector3.zero;
            dice3.transform.GetChild(0).localEulerAngles = Vector3.zero;

            PhysicsPreview.SetPhysicsStop();

            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint1.transform.position, forceSaver_first, true, dice1, torqueSaver_first);
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint2.transform.position, forceSaver_second, true, dice2, torqueSaver_second);
            Dice.Roll("1d6", "d6-" + randomColor, spawnPoint3.transform.position, forceSaver_third, true, dice3, torqueSaver_third);

            PhysicsPreview.delegateAfterFinishRolling = delegate () {

                StartCoroutine(PlayAfterStopDice());
            };

            PhysicsPreview.SetPhysicsSimulateOnlyStart();
        }
     

    }
    private IEnumerator PlayAfterStopDice()
    {
        dicesParent.transform.localPosition = new Vector3(dicesParent.transform.localPosition.x - 1000, 0, 0);
        yield return null;

        dice1.GetComponent<Die_d6>().GetValue();
        dice2.GetComponent<Die_d6>().GetValue();
        dice3.GetComponent<Die_d6>().GetValue();

        diceVal1 = dice1.GetComponent<Die_d6>().value;
        diceVal2 = dice2.GetComponent<Die_d6>().value;
        diceVal3 = dice3.GetComponent<Die_d6>().value;

        Debug.Log(diceVal1);
        Debug.Log(diceVal2);
        Debug.Log(diceVal3);

        Vector3 dice1rot = ChangeToValueAngle(diceVal1, dice1Value);
        Vector3 dice2rot = ChangeToValueAngle(diceVal2, dice2Value);
        Vector3 dice3rot = ChangeToValueAngle(diceVal3, dice3Value);

        dice1.transform.GetChild(0).localEulerAngles = dice1rot;
        dice2.transform.GetChild(0).localEulerAngles = dice2rot;
        dice3.transform.GetChild(0).localEulerAngles = dice3rot;

        dicesParent.transform.localPosition = new Vector3(dicesParent.transform.localPosition.x + 1000, 0, 0);
        PhysicsPreview.PhysicsRecordPlayStart();

        yield return null;

    }
    private Vector3 ChangeToValueAngle(int curValue, int toValue)
    {
        if (curValue == 1)
        {
            switch (toValue)
            {
                case 2:
                    return (new Vector3(-90, 0, 0));
                case 3:
                    return (new Vector3(0, 90, 0));
                case 4:
                    return (new Vector3(0, -90, 0));
                case 5:
                    return (new Vector3(90, 0, 0));
                case 6:
                    return (new Vector3(180, 0, 0));

            }
        }
        if (curValue == 2)
        {
            switch (toValue)
            {
                case 1:
                    return (new Vector3(90, 0, 0));
                case 3:
                    return (new Vector3(0, 0, 90));
                case 4:
                    return (new Vector3(0, 0, -90));
                case 5:
                    return (new Vector3(0, 0, -180));
                case 6:
                    return (new Vector3(-90, 0, 0));
            }
        }
        if (curValue == 3)
        {
            switch (toValue)
            {
                case 1:
                    return (new Vector3(0, -90, 0));
                case 2:
                    return (new Vector3(0, 0,-90));
                case 4:
                    return (new Vector3(0, 0, -180));
                case 5:
                    return (new Vector3(0, 0, 90));
                case 6:
                    return (new Vector3(0, 90, 0));
            }
        }
        if (curValue == 4)
        {
            switch (toValue)
            {
                case 1:
                    return (new Vector3(0, 90, 0));
                case 2:
                    return (new Vector3(0, 0, 90));
                case 3:
                    return (new Vector3(0, 0, 180));
                case 5:
                    return (new Vector3(0, 0, -90));
                case 6:
                    return (new Vector3(0, -90, 0));
            }
        }
        if (curValue == 5)
        {
            switch (toValue)
            {
                case 1:
                    return (new Vector3(-90, 0, 0));
                case 2:
                    return (new Vector3(0, 0, 180));
                case 3:
                    return (new Vector3(0, 0, -90));
                case 4:
                    return (new Vector3(0, 0, 90));
                case 6:
                    return (new Vector3(90, 0, 0));

            }
        }
        if (curValue == 6)
        {
            switch (toValue)
            {
                case 1:
                    return (new Vector3(180, 0, 0));
                case 2:
                    return (new Vector3(90, 0, 0));
                case 3:
                    return (new Vector3(0, -90, 0));
                case 4:
                    return (new Vector3(0, 90, 0));
                case 5:
                    return (new Vector3(-90, 0, 0));

            }
        }
        return Vector3.zero;
    }
    int dice1Value = 1;
    int dice2Value = 1;
    int dice3Value = 1;

    string str_dice1Value = string.Empty;
    string str_dice2Value = string.Empty;
    string str_dice3Value = string.Empty;
    void OnGUI()
    {
        str_dice1Value = GUI.TextField(new Rect(10, 0, 200, 20), str_dice1Value, 25);
        str_dice2Value = GUI.TextField(new Rect(10, 50, 200, 20), str_dice2Value, 25);
        str_dice3Value = GUI.TextField(new Rect(10, 100, 200, 20), str_dice3Value, 25);

        int.TryParse(str_dice1Value, out dice1Value);
        int.TryParse(str_dice2Value, out dice2Value);
        int.TryParse(str_dice3Value, out dice3Value);
    }
}
