using System;
using UnityEngine;
using GameFramework;
using GameFramework.Data;

public class DataManager : SingletonMono<DataManager>
{
    private IDataModule m_DataModule;
    private EventPool<DataEventArgs> m_EventPool;

    protected override void Awake()
    {
        base.Awake();
        m_EventPool = new EventPool<DataEventArgs>(EventPoolMode.AllowNoHandler | EventPoolMode.AllowMultiHandler);
    }

    public void Initialize()
    {
        this.m_DataModule = GameFrameworkEntry.GetModule<IDataModule>();
        this.m_DataModule.SetDataHelper(new DataHelper());

        this.m_DataModule.UpdateDataComplete += OnUpdateDataComplete;
        this.m_DataModule.CreateDataSuccess += OnCreateDataSuccess;
        this.m_DataModule.ParseDataFailure += OnParseDataFailure;
    }

    public void Subscribe(Type dataType, EventHandler<DataEventArgs> handler)
    {
        m_EventPool.Subscribe(DataName2ID(dataType.Name), handler);
    }

    public void Unsubscribe(Type dataType, EventHandler<DataEventArgs> handler)
    {
        m_EventPool.Unsubscribe(DataName2ID(dataType.Name), handler);
    }

    private int DataName2ID(string dataTypeName)
    {
        return this.m_DataModule.GetDataSerialID(dataTypeName);
    }

    public T GetData<T>() where T : DataBase
    {
        return this.m_DataModule.GetData<T>();
    }
    
    private void OnUpdateDataComplete(object sender, UpdateDataCompleteEventArgs e)
    {
        string[] dataNames = e.UpdateDataNames;
        for (int i = 0; i < dataNames.Length; ++i) {
            this.m_EventPool.Fire(this, DataEventArgs.Create(DataName2ID(dataNames[i])));
        }
    }

    private void OnCreateDataSuccess(object sender, CreateDataSuccessEventArgs e)
    {
        Debug.Log("create new data:" + e.NewDataName);
    }

    private void OnParseDataFailure(object sender, ParseDataFailureEventArgs e)
    {
        Debug.LogError("parse data failure");
    }

    private void Update()
    {
        m_EventPool.Update(Time.deltaTime, Time.unscaledDeltaTime);
    }

    private void OnDestroy()
    {
        m_EventPool.Shutdown();
    }

    protected override bool IsGlobalScope
    {
        get { return true; }
    }
}