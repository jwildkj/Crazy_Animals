using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum Managertype
{
    Gamemanager,
    Audiomanager,
    Scenemanager
}
[System.Serializable]
public struct Params
{
    public string[] PParams;
}
public class ButtonLinker : MonoBehaviour
{
    public Managertype[] type;
    public Params[] Params;
    public UnityEvent Event;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        for (int i = 0; i < type.Length; i++)
        {
            int index = i;
            string targetmethod = Event.GetPersistentMethodName(index);
            switch (type[index])
            {
                case Managertype.Gamemanager:
                    Gamemanager targetobj = FindFirstObjectByType<Gamemanager>();

                    btn.onClick.AddListener(() => {
                        MethodInfo method = targetobj.GetType().GetMethod(targetmethod);
                        if (method != null)
                        {
                            method.Invoke(targetobj, Params[index].PParams);
                        }
                    });
                    break;
                case Managertype.Audiomanager:
                    Audiomanager targetobj2 = FindFirstObjectByType<Audiomanager>();

                    btn.onClick.AddListener(() => {
                        MethodInfo method = targetobj2.GetType().GetMethod(targetmethod);
                        if (method != null)
                        {
                            method.Invoke(targetobj2, Params[index].PParams);
                        }
                    });
                    break;
                case Managertype.Scenemanager:
                    Scenemanager targetobj3 = FindFirstObjectByType<Scenemanager>();

                    btn.onClick.AddListener(() => {
                        MethodInfo method = targetobj3.GetType().GetMethod(targetmethod);
                        if (method != null)
                        {
                            method.Invoke(targetobj3, Params[index].PParams);
                        }
                    });
                    break;
            }
            
        }
    }
}
