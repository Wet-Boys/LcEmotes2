using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

[ExecuteAlways]
public class ChatMessageController : MonoBehaviour
{
    public TextMeshPro? usernameLabel;
    public Transform? content;

    public GameObject? textPrefab;
    public GameObject? emotePrefab;

    public AbstractChatWidth[] chatComponents = [];

    public bool forceClear;
    
    private readonly List<AbstractChatWidth> _generatedChatComponents = [];
    

    public void SetChat(string username, Color usernameColor, string message)
    {
        if (usernameLabel is null)
            return;
        
        ClearGenerated();

        usernameLabel.color = usernameColor;
        usernameLabel.text = username;
        
        GenMessageComponents(message);
        UpdateLayout();
    }

    private void OnDestroy()
    {
        ClearGenerated();
    }

    private void UpdateLayout()
    {
        var xPos = 0f;
        foreach (var chatComponent in chatComponents)
        {
            chatComponent.SetXPos(xPos);
            xPos += chatComponent.GetWidth();
            chatComponent.UpdateState();
        }

        foreach (var chatComponent in _generatedChatComponents)
        {
            chatComponent.SetXPos(xPos);
            xPos += chatComponent.GetWidth();
            chatComponent.UpdateState();
        }

        if (content is null)
            return;

        content.localPosition = new Vector3(-xPos / 2f, 0, 0);
    }

    private void ClearGenerated()
    {
        if (forceClear)
        {
            _generatedChatComponents.Clear();
            forceClear = false;
            return;
        }
        
        foreach (var chatComponent in _generatedChatComponents)
        {
            if (chatComponent is null)
                continue;
            
            DestroyImmediate(chatComponent.gameObject, false);
        }
                    
        _generatedChatComponents.Clear();
    }

    private void GenMessageComponents(string message)
    {
        if (content is null)
            return;

        if (textPrefab is null || emotePrefab is null)
            return;

        var textArray = message.Split(":GoblinPls:");
        if (textArray.Length == 0)
            return;

        for (var i = 0; i < textArray.Length; i++)
        {
            var textObject = Instantiate(textPrefab, content);
            textObject.transform.SetAsLastSibling();

            var textChat = textObject.GetComponent<TextMeshProChatWidth>();
            var textChatLabel = textChat.label;
            if (textChatLabel is null)
                return;

            textChatLabel.text = textArray[i];

            _generatedChatComponents.Add(textChat);

            if (i == textArray.Length - 1)
                continue;

            var emoteObject = Instantiate(emotePrefab, content);
            emotePrefab.transform.SetAsLastSibling();

            var emoteChat = emoteObject.GetComponent<ChatEmoteWidth>();
            
            _generatedChatComponents.Add(emoteChat);
        }
    }
}