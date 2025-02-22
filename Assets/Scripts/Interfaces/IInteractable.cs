using UnityEngine;

public enum Interaction {
    DefaultInteraction,
}

public interface IInteractable
{
    public void Interact(Interaction interaction);
}
