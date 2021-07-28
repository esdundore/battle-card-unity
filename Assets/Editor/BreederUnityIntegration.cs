using UnityEngine;
using UnityEditor;

static class BreederUnityIntegration
{

	[MenuItem("Assets/Create/BreederAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<BreederAsset>();
	}

}
