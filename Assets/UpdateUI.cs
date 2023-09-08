using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Xml.Serialization;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private GameObject ObjectPrefab;
    private TextMeshProUGUI UIText;
    private string ObjectID;

    public static int userGems = 0;

    private void Awake()
    {
        UIText = GetComponent<TextMeshProUGUI>();
        ObjectID = ObjectPrefab.GetComponent<Myobject>().id;
    }
     
    private void LateUpdate()
    {
        userGems = PlayerPrefs.GetInt(ObjectID, 0);
        UIText.text = PlayerPrefs.GetInt(ObjectID).ToString();
    }
}
