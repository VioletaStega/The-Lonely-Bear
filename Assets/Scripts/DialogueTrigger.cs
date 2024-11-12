using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField]
    private GameObject visualCue;
    [SerializeField]
    private TextAsset inkJSON;

    private bool playerInRange;


    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(true);
    }

    private void Update()
    {
        InRange();
    }

    private void InRange()
    {
        if (playerInRange == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                visualCue.SetActive(false);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                DialogueManager.GetInstance().ContinueStory();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
