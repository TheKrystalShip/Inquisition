namespace TheKrystalShip.Inquisition.Handlers
{
    public interface IPrefixHandler
    {
        string Get(ulong guildId);
        void Set(ulong guildId, string prefix);
    }
}