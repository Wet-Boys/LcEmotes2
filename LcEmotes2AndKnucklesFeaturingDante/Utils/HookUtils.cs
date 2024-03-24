using System;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace LcEmotes2AndKnucklesFeaturingDante.Utils;

internal static class HookUtils
{
    private const BindingFlags DefaultFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    private const BindingFlags StaticFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

    public static Hook NewHook<TTarget, TDest>(string targetMethodName, string destMethodName, TDest instance)
    {
        var targetMethod = typeof(TTarget).GetMethod(targetMethodName, DefaultFlags);
        var destMethod = typeof(TDest).GetMethod(destMethodName, DefaultFlags);

        return new Hook(targetMethod, destMethod, instance);
    }

    public static Hook NewHook<TTarget>(string targetMethodName, MethodInfo destMethod)
    {
        var targetMethod = typeof(TTarget).GetMethod(targetMethodName, DefaultFlags);

        return new Hook(targetMethod, destMethod);
    }
    
    /// <summary>
    /// For static destination classes.
    /// </summary>
    public static Hook NewHook<TTarget>(string targetMethodName, Type destType, string destMethodName, bool isStatic)
    {
        if (isStatic)
        {
            var targetMethod = typeof(TTarget).GetMethod(targetMethodName, StaticFlags);
            var destMethod = destType.GetMethod(destMethodName, StaticFlags);

            return new Hook(targetMethod, destMethod);
        }
        else
        {
            var targetMethod = typeof(TTarget).GetMethod(targetMethodName, DefaultFlags);
            var destMethod = destType.GetMethod(destMethodName, StaticFlags);

            return new Hook(targetMethod, destMethod);
        }
    }
}