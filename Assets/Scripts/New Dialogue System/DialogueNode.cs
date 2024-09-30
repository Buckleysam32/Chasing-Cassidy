using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueNode
{
    public List<DialogueLine> dialogueLines; // List of dialogue lines
    public List<DialogueResponse> responses;


    [System.Serializable]
    public class DialogueLine
    {
        public string text;    // The dialogue text
        public string speaker; // The name of the speaker
        public Sprite image;
    }

    internal bool IsLastNode()
    {
        return responses.Count <= 0;
    }
}
