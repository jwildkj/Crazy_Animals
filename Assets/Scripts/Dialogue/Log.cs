using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Log
{
    public string name;
    public string content;

    public Log(string _name, string _content)
    {
        name = _name;
        content = _content;
    }
}
