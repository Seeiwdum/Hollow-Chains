namespace HollowChains.Core
{
    /// <summary>Anything with a stable string ID that save data or events can refer to (e.g. "Q001-P2", "ITEM_BUCKET").</summary>
    public interface IIdentifiable
    {
        string Id { get; }
    }
}
