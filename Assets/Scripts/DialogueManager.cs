using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    //Singleton de DialogueManager
    private static DialogueManager instanceSingleton;

    //Panel donde aparecerá el dialogo
    [SerializeField]
    private GameObject dialoguePanel;
    //El texto que queremos mostrar
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    //Variable que me permitirá trabajar con los métodos de Ink
    private Story currentStory;
    //Variable bool para saber si el diálogo se está ejecutando
    private bool dialogueIsPlaying;
    //Para saber si es la primera vez que interactuo con el elemento/personaje
    //que me hablará.
    private bool firstClick;

    [SerializeField]
    private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;


    void Awake()
    {
        firstClick = false;
        IniChoices();

        //Si existe otro Singleton, y no soy yo, eliminame
        if (instanceSingleton != null && instanceSingleton != this)
        {
            Destroy(this);
        }
        //Si no existe el Singleton pues le hago que apunte hacia este script
        else
        {
            instanceSingleton = this;
        }
        
        dialoguePanel.SetActive(false);
    }


    void Update()
    {
        //dialogueIsPlaying == false
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            ContinueStory();
        }
    }

    private void IniChoices()
    {
        //Inicialización/asignación de Choices Text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public static DialogueManager GetInstance()
    {
        return instanceSingleton;
    }
    
    public void EnterDialogueMode(TextAsset archiveJSON)
    {
        //Cargamos el archivo JSON
        currentStory = new Story(archiveJSON.text);
        //Pongo a true la variable dialogueIsPlaying
        dialogueIsPlaying = true;
        //Activo el panel de canvas para que se muestren los dialogos
        dialoguePanel.SetActive(true);
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        firstClick = true;
    }

    public void ContinueStory()
    {
        //Si hay más lineas para leer dentro del JSON, sigo avanzando
        if (currentStory.canContinue)
        {
            //Despliega la siguiente linea de diálogo
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        //si no hay más lineas, cierro el panel de texto y los dialogos
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //Comprobamos que nuestra UI pueda asumir el número de opciones que estamos introduciendo
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Se dieron más opciones de las que la UI puede admitir. " +
                "Número de opciones dadas: " + currentChoices.Count);
        }

        //Hacemos que desde el primre momento los botones estén desactivados
        for (int index = 0; index < choices.Length; index++)
        {
            choices[index].gameObject.SetActive(false);
        }

        int i = 0;
        //Habilitar e inicializar las opciones hasta el número total de choices que haya 
        //por linea de diálogo
        foreach (Choice choice in currentChoices)
        {
            choices[i].gameObject.SetActive(true);
            choicesText[i].text = choice.text;
            i++;
        }
    }

    //Selecciona una opción al pulsar un botón
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
