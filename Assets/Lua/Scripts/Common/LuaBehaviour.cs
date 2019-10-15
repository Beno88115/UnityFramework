using UnityEngine;

public partial class LuaBehaviour : MonoBehaviour, ILuaBehaviour
{
    [SerializeField]
    string m_LuaFile;
    [SerializeField]
    LuaExtendHelper.ComponentBinding[] m_Components;

    private LuaExtendHelper m_LuaHelper;
    protected LuaExtendHelper LuaHelper { get { return m_LuaHelper; } }

    public string LuaComponentName { get { return m_LuaFile; } }

    void Awake()
    {
        m_LuaHelper = new LuaExtendHelper(m_LuaFile, m_Components, this);
        m_LuaHelper.CallFunction(LuaExtendHelper.AWAKE);
    }

    void Start()
    {
        m_LuaHelper.CallFunction(LuaExtendHelper.START);
    }

    void OnEnable()
    {
        m_LuaHelper.CallFunction(LuaExtendHelper.ONENABLE);
    }

    void OnDisable()
    {
        m_LuaHelper.CallFunction(LuaExtendHelper.ONDISABLE);
    }

    void OnDestroy()
    {
        m_LuaHelper.CallFunction(LuaExtendHelper.ONDESTROY);
        m_LuaHelper.Dispose();
    }
}