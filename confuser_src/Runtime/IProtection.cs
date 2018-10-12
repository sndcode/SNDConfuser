using dnlib.DotNet;

namespace SNDC
{
    interface IProtection
    {
        string name { get; }
        string description { get; }
        void Protect();
    }
}
