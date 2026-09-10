using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Select
{
    [Tooltip("선택지 내용")]
    public string[] content;

    [Tooltip("이동할 대화 라인")]
    public int[] dialogue;
}

/*
[System.Serializable]
public class SelectEvent
{
    public string name;

    public Vector2 line;
    public Select[] selects;
}
*/