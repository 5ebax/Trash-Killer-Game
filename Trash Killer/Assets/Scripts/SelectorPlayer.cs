using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorPlayer : MonoBehaviour
{
   public Image female;
   public Image male;

   private Vector3 characterPosition;
   private Vector3 offScreen;

   private int Character = 1;
   private readonly string selectedCharacter ="selectedCharacter";


   private void Awake()
   {
       characterPosition = male.transform.position;
       offScreen = female.transform.position;
   }

    private void Start()
    {
        PlayerPrefs.SetInt(selectedCharacter, 1);
    }

    public void NextCharacter()
   {
       switch(Character)
       {
           case 1:
            PlayerPrefs.SetInt(selectedCharacter, 2);
            male.enabled = false;
            male.transform.position = offScreen;
            female.transform.position = characterPosition;
            female.enabled = true;
            break;
           default:
            ResetInt();
           break;
       }
   }
   public void PreviousCharacter()
   {
       
       switch(Character)
       {
           case 1:
            PlayerPrefs.SetInt(selectedCharacter, 1);
            female.enabled = false;
            female.transform.position = offScreen;
            male.transform.position = characterPosition;
            male.enabled = true;
            break;
           default:
            ResetInt();
           break;
       }
   }
   private void ResetInt()
   {
       if(Character > 2)
       {
        Character = 1;
       }
   }

}
