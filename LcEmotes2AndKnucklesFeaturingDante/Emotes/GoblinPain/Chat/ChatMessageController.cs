using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LcEmotes2AndKnucklesFeaturingDante.Emotes.GoblinPain.Chat;

[ExecuteAlways]
public class ChatMessageController : MonoBehaviour
{
    public TextMeshPro? usernameLabel;
    public Transform? content;

    public GameObject? textPrefab;

    public ChatEmoteList? emoteList;
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
        if (content is null || textPrefab is null || emoteList is null)
            return;

        message = ExpandMessageMacros(message);

        var textComponents = message.Split(emoteList.EmoteKeys, StringSplitOptions.None);

        var emoteComponents = GetEmoteComponents(message);
        int emoteIndex = 0;

        foreach (var text in textComponents)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var textObject = Instantiate(textPrefab, content);
                textObject.transform.SetAsLastSibling();

                var textChat = textObject.GetComponent<TextMeshProChatWidth>();
                var textChatLabel = textChat.label;
                if (textChatLabel is null)
                    return;

                textChatLabel.text = text;

                _generatedChatComponents.Add(textChat);
            }

            if (emoteIndex >= emoteComponents.Count)
                continue;

            var emoteName = emoteComponents[emoteIndex]
                .Replace(":", "");
            emoteIndex++;
            
            var emotePrefab = emoteList[emoteName];
            if (emotePrefab is null)
                continue;

            var emoteObject = Instantiate(emotePrefab, content);
            emotePrefab.transform.SetAsLastSibling();

            var emoteChat = emoteObject.GetComponent<ChatEmoteWidth>();
            
            _generatedChatComponents.Add(emoteChat);
        }
    }

    private static List<string> GetEmoteComponents(string message)
    {
        var emoteComponents = new List<string>();
        
        var startOfEmote = false;
        var emoteName = "";

        foreach (var character in message)
        {
            if (character == ':')
            {
                if (!startOfEmote)
                {
                    startOfEmote = true;
                    continue;
                }

                startOfEmote = false;
                emoteComponents.Add(emoteName);
                emoteName = "";
            }

            if (!startOfEmote)
                continue;

            emoteName += character;
        }

        return emoteComponents;
    }

    private static string ExpandMessageMacros(string message)
    {
        if (!message.Contains("["))
            return message;
        
        var expandedMessage = "";
        
        bool randRangeStarted = false;
        bool randRangeTargetStarted = false;
        string intString = "";
        int? randRangeMin = null;
        int? randRangeMax = null;
        string randRangeTarget = "";
        
        foreach (var character in message)
        {
            if (character == '[')
            {
                randRangeStarted = true;
                randRangeMin = null;
                randRangeMax = null;
                intString = "";
                continue;
            }

            if (randRangeStarted)
            {
                if (character == ',')
                {
                    if (!int.TryParse(intString, out var result))
                    {
                        randRangeStarted = false;
                        continue;
                    }

                    intString = "";
                    randRangeMin = result;
                    continue;
                }
                
                if (character == ']')
                {
                    if (!int.TryParse(intString, out var result))
                    {
                        randRangeStarted = false;
                        continue;
                    }

                    intString = "";
                    randRangeMax = result;
                    continue;
                }
                
                if (character == '(' && !randRangeTargetStarted && randRangeMin.HasValue && randRangeMax.HasValue)
                {
                    randRangeTargetStarted = true;
                    randRangeStarted = false;
                    randRangeTarget = "";
                    continue;
                }
                
                if (!randRangeTargetStarted)
                    intString += character;
                
                continue;
            }


            if (randRangeTargetStarted && randRangeMin.HasValue && randRangeMax.HasValue)
            {
                if (character != ')')
                {
                    randRangeTarget += character;
                    continue;
                }

                var amount = Random.Range(randRangeMin.Value, randRangeMax.Value);
                for (int i = 0; i < amount; i++)
                    expandedMessage += randRangeTarget;

                randRangeTargetStarted = false;
                continue;
            }

            expandedMessage += character;
        }

        return expandedMessage;
    }
}