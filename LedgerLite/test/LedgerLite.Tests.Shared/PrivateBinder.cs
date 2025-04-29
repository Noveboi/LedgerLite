using System.Reflection;
using System.Runtime.CompilerServices;

namespace LedgerLite.Tests.Shared;

public class PrivateBinder : Bogus.Binder
{
    public override Dictionary<string, MemberInfo> GetMembers(Type t)
    {
        var members = base.GetMembers(t);

        const BindingFlags privateBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        var allPrivateMembers = t.GetMembers(privateBindingFlags)
            .OfType<FieldInfo>()
            .Where(fi => fi.IsPrivate)
            .Where(fi => !fi.GetCustomAttributes<CompilerGeneratedAttribute>().Any())
            .ToArray();

        foreach (var privateField in allPrivateMembers)
        {
            members.TryAdd(privateField.Name, privateField);
        }

        return members;
    }
}
