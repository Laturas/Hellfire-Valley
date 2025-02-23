
using UnityEngine;

public abstract class AbstractPlaceable: MonoBehaviour
{
    protected Snappable snappable { get; set; }

    public void OccupySnappable(Snappable snap)
    {
        snappable = snap;
        snappable.Occupy();
    }

    public void ReleaseSnappable()
    {
        if (!snappable) return;
        snappable.Release();
        snappable = null;
    }
}
