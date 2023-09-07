using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObject : MonoBehaviour
{
 private Myobject thisObject;


    private void Awake()
    {
        thisObject = GetComponent<Myobject>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(thisObject.id, PlayerPrefs.GetInt(thisObject.id) + 1);
            Destroy(gameObject);
        }
    }
}
