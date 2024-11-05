using GDWeave;

namespace MoreWag;

public class Mod : IMod
{
    public Config Config;
    public Mod(IModInterface modInterface)
    {
        this.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new TailRootPatch(Config));
    }

    public void Dispose()
    {
    }
}
