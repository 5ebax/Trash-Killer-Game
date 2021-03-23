using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charSelected : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] players;
    private int selection;
    public GameObject PlayerCharacter;
    private readonly string selectedCharacter ="selectedCharacter";

    void Awake()
    {
        selection = PlayerPrefs.GetInt(selectedCharacter);

        switch(selection)
        {
            case 1:
                PlayerCharacter = Instantiate(players[0], transform.position, Quaternion.identity);
            break;
            case 2:
                PlayerCharacter = Instantiate(players[1], transform.position, Quaternion.identity);
            break;
        }
    }
}
