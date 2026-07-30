using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace FinishPopTip
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class FinishPopTip : BaseUnityPlugin
    {
        private ConfigEntry<KeyCode> hotkey { get; set; }

        private void Awake()
        {
            hotkey = Config.Bind("General", "热键", KeyCode.S, "触发功能的热键");
        }

        private void Update()
        {
            if (!Input.GetKeyDown(hotkey.Value))
                return;

            if (!UIPopTip.Inst)
                return;

            // 提示数据缓存
            Traverse.Create(UIPopTip.Inst).Field("WaitForShow").GetValue<Queue<PopTipData>>().Clear();
            // 物品获取消息合并缓存
            Traverse.Create(UIPopTip.Inst).Field("addItemMergeMsgDict").GetValue<Dictionary<string, int>>().Clear();
            // 提示条 UI 缓存
            var tips = Traverse.Create(UIPopTip.Inst).Field("Tips").GetValue<List<UIPopTipItem>>();
            for (int i = tips.Count - 1; i >= 0; i--)
            {
                tips[i].TweenDestory();
                tips.RemoveAt(i);
            }
        }
    }
}
