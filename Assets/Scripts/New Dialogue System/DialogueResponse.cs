[System.Serializable]
public class DialogueResponse
{
    public string responseText;
    public DialogueNode nextNode;
    public bool isGoodResponse;
    public bool isBadResponse;
}