namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
    /// <summary>
    /// 调用设置种类
    /// </summary>
    internal enum CallSiteKind
    {
        Factory,

        Constructor,

        Constant,

        IEnumerable,

        ServiceProvider,

        Scope,

        Transient,

        CreateInstance,

        ServiceScopeFactory,

        Singleton
    }
}
