using UnityEngine;
using UnityEditor;

static class MonsterCardUnityIntegration {

	[MenuItem("Assets/Create/MonsterCardAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<MonsterCardAsset>();
	}

}
