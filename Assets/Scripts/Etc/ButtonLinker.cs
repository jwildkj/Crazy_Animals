using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[System.Serializable]
public struct Params
{
    public string[] PParams;
}
public class ButtonLinker : MonoBehaviour
{
    public string[] Types;
    public Params[] Params;
    public UnityEvent Event;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        for (int i = 0; i < Types.Length; i++)
        {
            int index = i;
            string targetmethod = Event.GetPersistentMethodName(index);
            Type t = Type.GetType(Types[index]);

            btn.onClick.AddListener(() => {
                UnityEngine.Object targetobj = FindFirstObjectByType(t);
                MethodInfo method = targetobj.GetType().GetMethod(targetmethod);
                if (method != null)
                {
                    bool hasParams = index < Params.Length
                     && Params[index].PParams != null
                     && Params[index].PParams.Length > 0;

                    if (hasParams) method.Invoke(targetobj, Params[index].PParams);
                    else method.Invoke(targetobj, null);
                }
            });
            
        }
    }
}
