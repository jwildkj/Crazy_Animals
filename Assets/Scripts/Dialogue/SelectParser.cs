using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectParser : MonoBehaviour
{
    public Select[] Parse(string csv)
    {
        List<Select> selectList = new List<Select>(); //선지 List 생성
        TextAsset csvData = Resources.Load<TextAsset>(csv); //csv파일 TextAsset으로 변환해서 가져옴

        if (csvData == null) //null 처리
        {
            return null;
        }

        string[] data = csvData.text.Split(new char[] { '\n' }); //'\n' 단위로 쪼갬

        for (int i = 1; i < data.Length;) //- 1;) //변환 과정에서 맨 뒤에 한 줄이 더 들어가는 듯
        {
            // 빈 줄이면 건너뜀
            if (string.IsNullOrWhiteSpace(data[i]))
            {
                i++;
                continue;
            }

            string[] col = data[i].Split(new char[] { ',' }); //',' 단위로 쪼갬

            Select select = new Select();

            //List 생성
            List<string> contentList = new List<string>();
            List<int> dialogueList = new List<int>();

            do
            {
                contentList.Add(col[1]);
                dialogueList.Add(col.Length > 2 && int.TryParse(col[2].Trim(), out int dialogue) ? dialogue : 0);

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

            } while (col[0].ToString() == ""); //ID가 공백이면 context만 추가

            //List 배열화
            select.content = contentList.ToArray();
            select.dialogue = dialogueList.ToArray();


            selectList.Add(select);
        }
        return selectList.ToArray();
    }
}
