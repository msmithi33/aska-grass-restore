using HarmonyLib;
using SSSGame;
using UnityEngine;

namespace AskaPlantGrass
{
    [HarmonyPatch(typeof(Character))]
    public static class CharacterSpawnPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Character.Spawned))]
        public static void Spawned(Character __instance)
        {
            if (!__instance.IsPlayer() || __instance.GetLocalAuthorityMask() != 1)
                return; // not our own local player's character - do nothing

            if (__instance.GetComponentInChildren<PlantGrassTool>() != null)
                return; // already attached (e.g. re-fired on respawn)

            var toolObj = new GameObject("AskaPlantGrass_Tool");
            toolObj.transform.SetParent(__instance.gameObject.transform, false);
            toolObj.transform.localPosition = new Vector3(0f, 0f, 2f);

            toolObj.AddComponent<HeightmapTool>();
            toolObj.AddComponent<PlantGrassTool>();

            Plugin.Log.LogInfo("PlantGrassTool attached to local player.");
        }
    }
}
