using HarmonyLib;
using System.Reflection;

[HarmonyPriority(Priority.First)]
public class HarmonyInit : IModApi
{
    public void InitMod(Mod mod)
    {
        Log.Out($"Loading my Forge & Forage mod from {Assembly.GetExecutingAssembly().FullName}");

        var harmony = new Harmony("Indominusvoids.Mods.V2");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }
}