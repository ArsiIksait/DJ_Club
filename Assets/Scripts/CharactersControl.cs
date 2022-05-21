using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Characters;

public class CharactersControl : MonoBehaviour
{
    public List<GameObject> ImageObjects;
    void Start()
    {
        Charact.ImageObjects = ImageObjects;
    }
    public static void ChangeCharacter(string UID, int CharacterID)
    {
        int index = 0;
        foreach (string item in Charact.ImageObjectsIndex)
        {
            if (UID == item)
            {
                if (CharacterID >= 1 && CharacterID <= 3)
                {
                    Charact.Animators[index].SetBool($"On{CharacterID}", true);
                }
                break;
            }
            index++;
        }
    }
    public static void Create(string UID,int CharacterID)
    {
        GameObject go = new GameObject(UID);
        int i = 0;
        if (CharacterID == -1)
            i = UnityEngine.Random.Range(0, 2);
        else
            i = CharacterID;
        Instantiate(Charact.ImageObjects[i], go.transform);
        Charact.Animators.Add(Charact.ImageObjects[i].GetComponent<Animator>());
        Charact.ImageObjectsIndex.Add(UID);
        ChangeCharacter(UID,CharacterID);
    }
}
