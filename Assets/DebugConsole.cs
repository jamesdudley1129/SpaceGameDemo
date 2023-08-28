using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public Dictionary<string, object> Attribute_Dictionary = new Dictionary<string, object>();
    public Dictionary<string, Component> Component_Dictionary = new Dictionary<string, Component>();
    public Dictionary<string, GameObject> GameObject_Dictionary = new Dictionary<string, GameObject>();
    public Settings GameMangerSettings;
    public RectTransform OutputField;
    public InputField text_Input;
    public List<Text> output = new List<Text>();
    Dictionary<int, Text> row_text = new Dictionary<int, Text>();
    public List<string> outputHistory = new List<string>();
    public GameObject ScrollBar;
    int maxLogHistroy;//not setup yet
    public bool DebugOrder = false;
    public void Start()
    {
        for (int current = 0; current < OutputField.childCount; current++)
        {
            output.Add(OutputField.GetChild(current).GetComponent<Text>());
        }
        for (int index = 0; index < output.Count; index++)
        {
            row_text.Add(index, output[index]);
        }
        int i = 0;
        foreach (Text row in row_text.Values)
        {
            if (DebugOrder)
            {
                row.text = "Field Row = " + i.ToString(); i++;
            }
            else
            {
                row.text = "*";
            }
        }
        
        
    }
    private void Update()
    {
        if (!text_Input.isFocused)
        {
            text_Input.ActivateInputField();
        }
        if (Input.GetButtonDown(GameMangerSettings.Return))
        {
            if (text_Input.text != "")
            {
                
                ProccessInput();
                text_Input.text = "";
                text_Input.textComponent.text = "";
            }          
        }
    }
    public void ProccessInput()
    {
        if (text_Input.text != "")
        {
            UpdateOutput("*");
            UpdateOutput(">" + text_Input.text);
            UpdateOutput("*");
        }
        if (CheckForSimpleCommand(text_Input.text,out string[] replyArray))
        {
            foreach (string reply in replyArray)
            {
                UpdateOutput(reply);
            }
        }
        
    }
    public void UpdateOutput(string new_Data) {
        Debug.Log(new_Data);
        outputHistory.Add(new_Data);
        outputHistory.Reverse();
        int outputindex = 0;
        for (int key = row_text.Count - 1; key >= 0; key--)
        {
            if (outputHistory.Count > outputindex)
            {
                row_text[key].text = outputHistory[outputindex];

            }
            outputindex++;
        }
        outputHistory.Reverse();

    }
    public bool CheckForSimpleCommand(string part, out string[] value)
    {
        string current = part;
        Debug.Log(part);
        if (part.ToLower().StartsWith("/"))
        {
            current = part.Remove(0,1);
            Debug.Log(current);
            if (current.ToLower().StartsWith("help"))
            {

                value = new string[]
                {
                    "'/' is for simple commands like help.",
                    "'/controls' gives access the controls list",
                    "'/exit' exits the console",
                    "You can refrence gameobjects and there attributes with this format",
                    "*incomplete*[GameObject][Component][Property][overide value]",
                    "*incomplete*Type: >'/GOlist' for a list of gameobjects, >'/CMPlist' for a list of components'"
                };
                     
                return true;
            }
            if (current.ToLower().StartsWith("controls"))
            {
                value = new string[]
               {
                    "'alt+f4' force exits application",
                    "'Tab' Toggels Action Menu at the top of the screen",
                    "'-' resets throttle will start to match speed of refrence point",
                    "'+' sets throttle to 100%",
                    "'w,a,s,d' strafe controls for ship",
                    "'q,e' roll controls for ship",
                    "'z,x' lower & Increese's throttle",
                    "'t' targets closest object to camrea fwd",
                    "'right click' opens menu for mouse selected object",
                    "'left click' primary fire(also targets)",
                    "'left, up, right, down' control look dir via keyboard",
                    "'space toggles mouse aim'",
                    "'1' spawns astroid at random velocity",
                    "'2' despawns last astroid",
                    "'F1,F2,F3' switch camrea modes",
                    "'3,4'weapons Debug ON & OFF",
                    "type '/exit' to close debug console or '/help' for help"
               };
                return true;
            }
            if (current.ToLower().StartsWith("exit"))
            {
                gameObject.SetActive(false);
            }
        }
        

        value = null;
        return false;
    }
    public Component CheckForRefrence(string part)
    {
        Component refrence;
        if(Component_Dictionary.TryGetValue(part,out refrence))
        {
            return refrence;
        }
        else
        {
            return null;
        }   
    }
    public object CheckForAttribute(string part)
    {
        
        if (Attribute_Dictionary.TryGetValue(part,out object value ))
        {
            return value;
        }
        else
        {
            return null;
        }
    }
    public GameObject CheckForGameObject(string part)
    {

        if (GameObject_Dictionary.TryGetValue(part, out GameObject value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }
}
