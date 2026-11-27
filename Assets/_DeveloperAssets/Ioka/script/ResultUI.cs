using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    [SerializeField] Button TitleButton;
    [SerializeField] Button GameButton;

    [Header("オーディオ")]
    [SerializeField] AudioClip moveSelect;
    [SerializeField] AudioClip decision;

    private bool isTiteSelected = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SelectButton(TitleButton);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            isTiteSelected=true;
            SoundManager.Instance.PlaySE(moveSelect);
            SelectButton(TitleButton);

        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            isTiteSelected = false;
            SoundManager.Instance.PlaySE(moveSelect);
            SelectButton(GameButton);
           
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            SoundManager.Instance.PlaySE(decision);
            if (selected != null)
            {
                ExecuteEvents.Execute(
                    selected,
                    new BaseEventData(EventSystem.current),
                    ExecuteEvents.submitHandler);
            }
            if(isTiteSelected==true)
            {
                SceneManager.LoadScene("Title");
            }
            if (isTiteSelected==false)
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }
    private void SelectButton(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }
}
