using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string csv)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset csvData = Resources.Load<TextAsset>(csv);

        Debug.Log(csvData);

        string[] data = csvData.text.Split(new char[] { '\n' }); //'\n' 단위로 쪼개기

        for (int i = 1; i < data.Length;) //- 1;)
        {
            string[] col = data[i].Split(new char[] { ',' }); //',' 단위로 쪼개기

            Dialogue dialogue = new Dialogue();

            dialogue.name = col[1];
            //List 생성
            List<string> contentList = new List<string>();
            //List<int> expressionList = new List<int>();
            //List<int> skipList = new List<int>();
            List<string> expressionList = new List<string>();
            List<string> skipList = new List<string>();

            do
            {
                contentList.Add(col.Length > 2 ? col[2] : "");
                //expressionList.Add(col.Length > 3 ? int.Parse(col[3]) : 0);
                //skipList.Add(col.Length > 4 ? int.Parse(col[4]) : 0);
                expressionList.Add(col.Length > 3 ? col[3] : "");
                skipList.Add(col.Length > 4 ? col[4] : "");

                if (++i < data.Length) //- 1)
                {
                    col = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            }
            while (string.IsNullOrWhiteSpace(col[0])); //(col[0].ToString() == ""); //ID가 공백이면 content만 추가

            dialogue.content = contentList.ToArray();
            dialogue.expression = expressionList.ToArray();
            dialogue.skip = skipList.ToArray();

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }
}
