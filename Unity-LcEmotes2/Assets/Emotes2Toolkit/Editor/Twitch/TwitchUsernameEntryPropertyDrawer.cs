using Emotes2Toolkit.Editor.UxmlUtils;
using LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Data;
using UnityEditor;
using UnityEngine.UIElements;

namespace Emotes2Toolkit.Editor.Twitch
{
    [CustomPropertyDrawer(typeof(TwitchUsernameEntry))]
    public class TwitchUsernameEntryPropertyDrawer : UxmlPropertyDrawer
    {
        protected override string UxmlPath => "Twitch/uxml/TwitchUsernameEntry.uxml";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            if (!UxmlVisualTree)
                return root;

            UxmlVisualTree.CloneTree(root);

            return root;
        }
    }
}