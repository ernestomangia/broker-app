using Broker.Infrastructure;

namespace Broker.Application.Core;

public abstract class ServiceBase : IDisposable
{
    #region Constructor(s)

    protected ServiceBase(AppDbContext context)
    {
        Context = context;
    }

    #endregion

    #region Private Member(s)

    protected AppDbContext Context { get; }

    #endregion

    public void Dispose()
    {
        Context.Dispose();
    }
}