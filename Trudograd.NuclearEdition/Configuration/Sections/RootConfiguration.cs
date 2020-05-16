namespace Trudograd.NuclearEdition
{
    public sealed class RootConfiguration
    {
        public BombaganConfiguration Bombagan { get; } = new BombaganConfiguration();
        public DialogConfiguration Dialog { get; } = new DialogConfiguration();
    }
}