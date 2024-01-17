using System;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat.Timeline;
using UnityEditor;
using UnityEngine;

namespace Emotes2Toolkit.Editor.Twitch
{
    [CustomEditor(typeof(TwitchChatControlAsset))]
    public class TwitchChatControlAssetEditor : UnityEditor.Editor
    {
        protected TwitchChatControlAsset Target => (TwitchChatControlAsset)target;

        private bool[] _entryVisibilityArray = Array.Empty<bool>();
        
        public override void OnInspectorGUI()
        {
            var messages = Target.messages;

            if (_entryVisibilityArray.Length != messages.Length)
            {
                Array.Resize(ref _entryVisibilityArray, messages.Length);
                Array.Fill(_entryVisibilityArray, true);
            }
            
            GUILayout.Label("Entries");

            var indent = EditorGUI.indentLevel;
            for (int i = 0; i < messages.Length; i++)
            {
                _entryVisibilityArray[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_entryVisibilityArray[i], messages[i]);
                if (!_entryVisibilityArray[i])
                {
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    continue;
                }
                
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginVertical();
                
                Target.weights[i] = EditorGUILayout.FloatField("Weight", Target.weights[i]);
                
                EditorGUILayout.EndVertical();
                
                EditorGUI.indentLevel--;
                
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            EditorGUI.indentLevel = indent;
        }
    }
}