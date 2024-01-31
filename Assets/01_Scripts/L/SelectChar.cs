using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClass
{
    Warrior, Gunner, Magician
}

public class SelectChar : MonoBehaviour
{
    public static string currentCharacter;
    public static int CharNum;
    public CharacterClass character;
    public SelectChar[] chars;

    public void OnClickCharacterBtn()
    {
        // 해당 버튼의 character를 현재 스크립트의 currentCharacter에 할당
        currentCharacter = character.ToString();

        // 슬롯번호를 다른스크립트에 넘겨주기
        switch (currentCharacter)
        {
            case "Warrior":
                CharNum = 0;
                break;
            case "Gunner":
                CharNum = 1;
                break;
            case "Magician":
                CharNum = 2;
                break;
            default:
                break;
        }

        // 할당된 직업 확인
        Debug.Log("Selected Character: " + currentCharacter);
    }
}