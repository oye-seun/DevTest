using UnityEditor;
public class PuzzleRock : Interactable
{
    public override void Interact()
    {
        throw new System.NotImplementedException();
    }
}

[CustomEditor(typeof(PuzzleRock))]
public class PuzzleRockEditor : InteractableEditor
{
    protected override void OnSceneGUI()
    {
        base.OnSceneGUI();
    }
}