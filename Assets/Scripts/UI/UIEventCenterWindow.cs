using System;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;
using GameFramework;
using System.Collections.Generic;

public class UIEventCenterWindow : UIWindow 
{
    static readonly int CUSTOM_EVENT_ID = 99999;

    [SerializeField]
    Button btnRed; 
    [SerializeField]
    Button btnGreen;
    [SerializeField]
    Button btnBlue;
    [SerializeField]
    Button btnBack;
    [SerializeField]
    Button btn1;
    [SerializeField]
    Button btn2;
    [SerializeField]
    Button btn3;
    [SerializeField]
    Button btn4;
    [SerializeField]
    Button btn5;
    [SerializeField]
    Button btn6;

    List<Button> m_Observers = new List<Button>();

    void Start()
    {
        this.btnRed.onClick.AddListener(OnRedButtonClicked);
        this.btnGreen.onClick.AddListener(OnGreenButtonClicked);
        this.btnBlue.onClick.AddListener(OnBlueButtonClicked);
        this.btnBack.onClick.AddListener(OnBackButtonClicked);

        this.btn1.onClick.AddListener(()=>{ m_Observers.Add(btn1); btn1.interactable = false; });
        this.btn2.onClick.AddListener(()=>{ m_Observers.Add(btn2); btn2.interactable = false; });
        this.btn3.onClick.AddListener(()=>{ m_Observers.Add(btn3); btn3.interactable = false; });
        this.btn4.onClick.AddListener(()=>{ m_Observers.Add(btn4); btn4.interactable = false; });
        this.btn5.onClick.AddListener(()=>{ m_Observers.Add(btn5); btn5.interactable = false; });
        this.btn6.onClick.AddListener(()=>{ m_Observers.Add(btn6); btn6.interactable = false; });

        EventManager.Instance.Subscribe(CUSTOM_EVENT_ID, new EventHandler<GameEventArgs>(OnHandleCustomEvent));
    }

    void OnRedButtonClicked()
    {
        EventManager.Instance.Fire(this, CustomEventArgs.Create(CUSTOM_EVENT_ID, Color.red));
    }

    void OnGreenButtonClicked()
    {
        EventManager.Instance.Fire(this, CustomEventArgs.Create(CUSTOM_EVENT_ID, Color.green));
    }

    void OnBlueButtonClicked()
    {
        EventManager.Instance.Fire(this, CustomEventArgs.Create(CUSTOM_EVENT_ID, Color.blue));
    }

    void OnBackButtonClicked()
    {
        UIManager.Instance.PopWindow(this.SerialId);
    }

    void OnHandleCustomEvent(object sender, GameEventArgs e)
    {
        CustomEventArgs ce = (CustomEventArgs)e;
        if (ce == null)
            return;

        for (int i = 0; i < m_Observers.Count; ++i)
        {
            m_Observers[i].GetComponent<Image>().color = (Color)ce.UserData;
        }
    }
}
