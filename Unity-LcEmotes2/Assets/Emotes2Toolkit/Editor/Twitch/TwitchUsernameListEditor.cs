using System;
using System.IO;
using Emotes2Toolkit.Editor.UxmlUtils;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Emotes2Toolkit.Editor.Twitch
{
    [CustomEditor(typeof(TwitchUsernameList))]
    public class TwitchUsernameListEditor : UxmlCustomEditor<TwitchUsernameList>
    {
        public VisualTreeAsset uxmlVisualTree;

        public VisualTreeAsset uxmlTwitchUsernameListItem;

        [UxmlBindValue("CreateEntryRoot.UsernameField")]
        private string _usernameFieldValue;
        [UxmlBindValue("CreateEntryRoot.UsernameColorField")]
        private Color _usernameColorFieldValue;
        [UxmlBindValue("CreateEntryRoot.SpecialMessageField")]
        private string _specialMessageFieldValue;
        [UxmlBindValue("CreateEntryRoot.PrefabField")]
        private GameObject _prefabFieldValue;

        protected override VisualElement CreateCustomEditorGUI()
        {
            var root = new VisualElement();

            if (!uxmlVisualTree)
                return root;

            uxmlVisualTree.CloneTree(root);

            return root;
        }

        [UxmlBindButton("CreateEntryRoot.CreateEntryButton")]
        private void CreateEntry()
        {
            var targetPath = AssetDatabase.GetAssetPath(Target);

            var parent = Directory.GetParent(targetPath);
            if (parent is null)
            {
                Debug.LogWarning("Target has no parent!");
                return;
            }

            var path = EditorUtility.SaveFolderPanel("Save To Directory", parent.FullName, "");

            if (string.IsNullOrEmpty(path))
                return;

            var entryPath = Path.Combine(path, $"{_usernameFieldValue}-TwitchUsernameEntry.asset");
            entryPath = FileUtil.GetProjectRelativePath(entryPath);
            CreateTwitchUsernameEntry(entryPath);
            
            var entry = AssetDatabase.LoadAssetAtPath<TwitchUsernameEntry>(entryPath);
            if (entry is null)
                return;

            var weightedEntryPath = Path.Combine(path, $"{_usernameFieldValue}-TwitchUsername-WeightedEntry.asset");
            weightedEntryPath = FileUtil.GetProjectRelativePath(weightedEntryPath);
            CreateWeightedEntry(weightedEntryPath, entry);
            
            var weightedEntry = AssetDatabase.LoadAssetAtPath<WeightedTwitchUsernameEntry>(weightedEntryPath);
            if (weightedEntry is null)
                return;
            
            int index = Target.weightedEntries.Length;
            Array.Resize(ref Target.weightedEntries, index + 1);

            Target.weightedEntries[index] = weightedEntry;
            EditorUtility.SetDirty(Target);
            AssetDatabase.SaveAssetIfDirty(Target);
            AssetDatabase.Refresh();
            
            Repaint();
        }

        private void CreateTwitchUsernameEntry(string path)
        {
            var instance = CreateInstance<TwitchUsernameEntry>();
            instance.username = _usernameFieldValue;
            instance.usernameColor = _usernameColorFieldValue;
            instance.specialMessage = _specialMessageFieldValue;
            instance.prefabOverride = _prefabFieldValue;
            
            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();
        }

        private void CreateWeightedEntry(string path, TwitchUsernameEntry entry)
        {
            var instance = CreateInstance<WeightedTwitchUsernameEntry>();
            instance.weight = 1f;
            instance.entry = entry;
            
            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();
        }

        [UxmlOnBindElement("EntriesListView")]
        private void SetupListView(ListView entriesListView)
        {
            entriesListView.makeItem = () =>
            {
                var root = new VisualElement();

                if (!uxmlTwitchUsernameListItem)
                    return root;

                uxmlTwitchUsernameListItem.CloneTree(root);

                return root;
            };

            var weightedEntriesProp = serializedObject.FindProperty("weightedEntries");

            entriesListView.bindItem = (item, index) =>
            {
                var weightedEntryProp = weightedEntriesProp.GetArrayElementAtIndex(index);
                
                var weightedEntry = new SerializedObject(weightedEntryProp.objectReferenceValue);
                var entryObj = new SerializedObject(weightedEntry.FindProperty("entry").objectReferenceValue);
                
                item.Q<FloatField>("WeightField").Bind(weightedEntry);
                item.Q("TwitchUsernameEntry").Bind(entryObj);
            };

            entriesListView.itemsSource = Target.weightedEntries;
        }
    }
}
