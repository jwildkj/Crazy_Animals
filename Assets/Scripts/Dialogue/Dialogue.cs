using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] content;

    [Tooltip("표정")]
    public int[] expression;

    [Tooltip("선택지")]
    public int[] select;

    [Tooltip("스킵 라인")]
    public int[] skip;
}

/*
[System.Serializable]
public class DialogueEvent
{
    public string name;

    public Vector2 line;
    public Dialogue[] dialogues;
}

*/