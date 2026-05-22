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
public class ButtonLinker : MonoBehaviour
{
    public Managertype type;
    public string[] Params;
    public UnityEvent Event;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = GetComponent<Button>();
        string targetmethod = Event.GetPersistentMethodName(0);
        switch (type)
        {
            case Managertype.Gamemanager:
                Gamemanager targetobj = FindFirstObjectByType<Gamemanager>();

                btn.onClick.AddListener(() => {
                    MethodInfo method = targetobj.GetType().GetMethod(targetmethod);
                    if (method != null)
                    {
                        method.Invoke(targetobj, Params);
                    }
                });
                break;
            case Managertype.Audiomanager:
                Audiomanager targetobj2 = FindFirstObjectByType<Audiomanager>();

                btn.onClick.AddListener(() => {
                    MethodInfo method = targetobj2.GetType().GetMethod(targetmethod);
                    if (method != null)
                    {
                        method.Invoke(targetobj2, Params);
                    }
                });
                break;
            case Managertype.Scenemanager:
                Scenemanager targetobj3 = FindFirstObjectByType<Scenemanager>();

                btn.onClick.AddListener(() => {
                    MethodInfo method = targetobj3.GetType().GetMethod(targetmethod);
                    if (method != null)
                    {
                        method.Invoke(targetobj3, Params);
                    }
                });
                break;
        }
    }
}
