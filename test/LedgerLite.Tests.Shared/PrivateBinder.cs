using System.Reflection;
using System.Runtime.CompilerServices;
using Binder = Bogus.Binder;

namespace LedgerLite.Tests.Shared;

public class PrivateBinder : Binder
{
    public override Dictionary<string, MemberInfo> GetMembers(Type t)
    {
        var members = base.GetMembers(t: t);

        const BindingFlags privateBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        var allPrivateMembers = t.GetMembers(bindingAttr: privateBindingFlags)
            .OfType<FieldInfo>()
            .Where(predicate: fi => fi.IsPrivate)
            .Where(predicate: fi => !fi.GetCustomAttributes<CompilerGeneratedAttribute>().Any())
            .ToArray();

        foreach (var privateField in allPrivateMembers) members.TryAdd(key: privateField.Name, value: privateField);

        return members;
    }
}