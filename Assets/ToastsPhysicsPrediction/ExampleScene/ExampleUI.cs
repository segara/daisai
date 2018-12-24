using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleUI : MonoBehaviour {

    public static Slider strength;
    public Slider st;
    public Text strengthValue;

    // Use this for initialization
    void Start()
    {
        strength = st;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        strength.value += Input.mouseScrollDelta.y * 20;
        strengthValue.text = ((int)strength.value / 20).ToString();
        Cursor.lockState = (CursorLockMode)System.Convert.ToInt32(!Input.GetKey(KeyCode.Escape));
    }
}
