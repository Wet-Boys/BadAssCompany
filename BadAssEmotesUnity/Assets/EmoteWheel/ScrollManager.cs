using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject content;
    public GameObject buttonTemplate;
    public List<GameObject> buttons = new List<GameObject>();
    public List<string> emoteNames = new List<string>();
    public GameObject emoteInQuestion;
    void Start()
    {
        foreach (var item in emoteNames)
        {
            GameObject cum = GameObject.Instantiate(buttonTemplate);
            cum.name = item;
            cum.GetComponentInChildren<TextMeshProUGUI>().text = item;
            cum.transform.parent = buttonTemplate.transform.parent;
            cum.transform.transform.localScale = Vector3.one;
            buttons.Add(cum);
        }
        gameObject.transform.parent.Find("Wheels").gameObject.SetActive(true);
        gameObject.SetActive(false);




        content = gameObject.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;

        buttonTemplate = gameObject.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Button").gameObject;






        /*
             public static void Setup()
    {
        manager = Settings.picker.transform.Find("emotepicker").Find("EmoteContainer").gameObject.AddComponent<ScrollManager>();
        DebugClass.Log($"----------{manager}");
        manager.content = manager.gameObject.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;
        DebugClass.Log($"----------{manager.content}");
        manager.buttonTemplate = manager.gameObject.transform.Find("Scroll View").Find("Viewport").Find("Content").Find("Button").gameObject;
        DebugClass.Log($"----------{manager.buttonTemplate}");
        //manager.emoteNames = //set this up somewhere
        var butt = manager.buttonTemplate.AddComponent<ButtonScript>();
        manager.buttonTemplate.GetComponent<Button>().onClick.AddListener(butt.PickNewEmote);
        DebugClass.Log($"----------{butt}");
        manager.gameObject.transform.Find("Finish").GetComponent<Button>().onClick.AddListener(FinishClicked);

        TMP_InputField field = manager.gameObject.transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>();
        field.onValueChanged.AddListener(delegate { manager.UpdateButtonVisibility(field.text); });
        DebugClass.Log($"----------{field}");
        GameObject wheels = manager.transform.parent.Find("Wheels").gameObject;
        for (int i = 0; i < wheels.GetComponentsInChildren<GameObject>().Length - 1; i++)
        {
            if (wheels.GetComponentsInChildren<GameObject>()[i].GetComponent<Button>())
            {
                wheels.GetComponentsInChildren<GameObject>()[i].GetComponent<Button>().onClick.AddListener(delegate { manager.Activate(wheels.GetComponentsInChildren<GameObject>()[i]); });
            }
        }
        DebugClass.Log($"----------{wheels.GetComponentsInChildren<GameObject>().Length}");
        manager.emoteNames = new List<string>();
        for (int i = 0; i < 30; i++)
        {
            manager.emoteNames.Add($"{i}");
        }
        DebugClass.Log($"----------end of setup");
    }
    static void FinishClicked()
    {
        DebugClass.Log($"----------should cancel");
        manager.Cancel();
    }
         
         */
    }
    void OnEnable()
    {
        UpdateButtonVisibility("");
    }

    public void SetEmoteInQuestion(GameObject e)
    {
        emoteInQuestion = e;
    }
    public void Activate(GameObject button)
    {
        gameObject.transform.parent.Find("Wheels").gameObject.SetActive(false);
        Debug.Log("activated");
        emoteInQuestion = button;
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        gameObject.transform.parent.Find("Wheels").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void UpdateButtonVisibility(string filter)
    {
        List<GameObject> validButtons = new List<GameObject>();
        foreach (var item in buttons)
        {
            if (item.GetComponentInChildren<TextMeshProUGUI>().text.ToUpper().Contains(filter.ToUpper()))
            {
                validButtons.Add(item);
            }
            else
            {
                item.SetActive(false);
            }
        }
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, validButtons.Count * 32);
        for (int i = 0; i < validButtons.Count; i++)
        {
            validButtons[i].SetActive(true);
            validButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -16 + (content.GetComponent<RectTransform>().sizeDelta.y * .5f) + (i * -32));
            validButtons[i].GetComponent<ButtonScript>().emoteInQuestion = emoteInQuestion;
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
