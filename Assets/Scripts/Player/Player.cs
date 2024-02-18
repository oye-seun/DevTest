using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _interactDist;

    private List<Interactable> _interactables;

    private void OnEnable()
    {
        _interactables = new List<Interactable>();
        Interactable.InteractableCreated += AddInteractable;
        Interactable.InteractableDestroyed += RemoveInteractable;
    }

    private void OnDisable()
    {
        Interactable.InteractableCreated -= AddInteractable;
        Interactable.InteractableDestroyed -= RemoveInteractable;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void AddInteractable(Interactable itr)
    {
        _interactables.Add(itr);
    }
    private void RemoveInteractable(Interactable itr)
    {
        _interactables.Remove(itr);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.GameState == GameState.Playing) CheckNearest();
    }

    private void CheckNearest()
    {
        int nearest = -1;
        float nearDist = float.MaxValue;
        for(int i = 0; i < _interactables.Count; i++)
        {
            float Dist = Vector3.Distance(transform.position, _interactables[i].transform.position);
            if(Dist < nearDist) { nearDist = Dist; nearest = i; }
        }

        if (nearest < 0) return;
        if (nearDist < _interactDist)
        {
            GameManager.instance.ShowInteractButtonPrompt(_interactables[nearest].PromptPos, _interactables[nearest].transform.position);
            if (PlayerInputs.instance.InteractKeyPressed()) _interactables[nearest].Interact();
        }
        else GameManager.instance.DisableInteractPrompt();
        //_interactables[nearest]
    }
}
