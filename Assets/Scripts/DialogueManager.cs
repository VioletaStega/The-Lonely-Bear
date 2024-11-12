using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    //Singleton de DialogueManager
    private static DialogueManager instanceSingleton;

    //Panel donde aparecer� el dialogo
    [SerializeField]
    private GameObject dialoguePanel;
    //El texto que queremos mostrar
    [SerializeField]
    private TextMeshProUGUI dialogueText;

    //Variable que me permitir� trabajar con los m�todos de Ink
    private Story currentStory;
    //Variable bool para saber si el di�logo se est� ejecutando
    private bool dialogueIsPlaying;
    //Para saber si es la primera vez que interactuo con el elemento/personaje
    //que me hablar�.
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
        //Inicializaci�n/asignaci�n de Choices Text
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
        //Si hay m�s lineas para leer dentro del JSON, sigo avanzando
        if (currentStory.canContinue)
        {
            //Despliega la siguiente linea de di�logo
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        //si no hay m�s lineas, cierro el panel de texto y los dialogos
        else
        {
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //Comprobamos que nuestra UI pueda asumir el n�mero de opciones que estamos introduciendo
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("Se dieron m�s opciones de las que la UI puede admitir. " +
                "N�mero de opciones dadas: " + currentChoices.Count);
        }

        //Hacemos que desde el primre momento los botones est�n desactivados
        for (int index = 0; index < choices.Length; index++)
        {
            choices[index].gameObject.SetActive(false);
        }

        int i = 0;
        //Habilitar e inicializar las opciones hasta el n�mero total de choices que haya 
        //por linea de di�logo
        foreach (Choice choice in currentChoices)
        {
            choices[i].gameObject.SetActive(true);
            choicesText[i].text = choice.text;
            i++;
        }
    }

    //Selecciona una opci�n al pulsar un bot�n
    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
