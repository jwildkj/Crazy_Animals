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
        //빈 줄 건너뛰는 아래랑 같은 코드. 둘 중 하나만 사용.
        //string[] data = csvData.text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < data.Length;) //- 1;)
        {
            // 빈 줄이면 건너뜀
            if (string.IsNullOrWhiteSpace(data[i]))
            {
                i++;
                continue;
            }

            string[] col = data[i].Split(new char[] { ',' }); //',' 단위로 쪼개기

            Dialogue dialogue = new Dialogue();

            dialogue.name = col[1];
            //List 생성
            List<string> contentList = new List<string>();
            List<int> expressionList = new List<int>();
            List<int> selectList = new List<int>();
            List<int> skipList = new List<int>();

            do
            {
                contentList.Add(col.Length > 2 ? col[2] : "");
                expressionList.Add(col.Length > 3 && int.TryParse(col[3].Trim(), out int expression) ? expression : 0);
                selectList.Add(col.Length > 4 && int.TryParse(col[4].Trim(), out int select) ? select : 0);
                skipList.Add(col.Length > 5 && int.TryParse(col[5].Trim(), out int skip) ? skip : 0);

                if (++i < data.Length) //- 1)
                {
                    // 다음 줄이 빈 줄이면 종료
                    if (string.IsNullOrWhiteSpace(data[i]))
                    {
                        break;
                    }

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
            dialogue.select = selectList.ToArray();
            dialogue.skip = skipList.ToArray();

            dialogueList.Add(dialogue);
        }

        return dialogueList.ToArray();
    }
}
