using System.Collections;
using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Console : MonoBehaviour
{
    public GameObject input;
    public InputField InputField;
    public static string Command;
    bool state = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (state == true)
            {
                input.SetActive(false);
                state = false;
            }
            else {
                input.SetActive(true);
                state = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (state)
            {
                Command = InputField.text;
                InputField.text = "";
                if (Command != "")
                    ExecuteCommand(Command);
            }
        }
    }
    public static void ExecuteCommand(string Command)
    {
        var split = Command.Split(' ');
        Debug.Log(Command);
        switch (split[0].ToLower())
        {
            //case "create":
            case "c":
                string UID = split[1];
                int CID = -1;
                if (int.TryParse(split[2], out int i))
                    CID = i;
                CharactersControl.Create(UID,CID);
                break;
        }
    }
}
