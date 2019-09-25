using UnityEditor;

public static class AssetBundleSimulation 
{
    [MenuItem("Tools/AssetBundles/Simulation Mode", false, 0)]
	static void ToggleSimulationMode()
	{
        ResourceManager.SimulateAssetBundleInEditor = !ResourceManager.SimulateAssetBundleInEditor;
	}

	[MenuItem("Tools/AssetBundles/Simulation Mode", true)]
	static bool ToggleSimulationModeValidate()
	{
        Menu.SetChecked("Tools/AssetBundles/Simulation Mode", ResourceManager.SimulateAssetBundleInEditor);
		return true;
	}
}