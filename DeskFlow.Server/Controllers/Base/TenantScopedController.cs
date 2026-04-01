namespace DeskFlow.Server.Controllers.Base
{
    /// <summary>
    ///  Forces all tenant-scoped controllers to resolve tenantId
    ///  consistently from the JWT — never from the request body
    /// </summary>
    public abstract class TenantScopedController : BaseApiController
    {
       
        protected Guid ResolveTenantId() => CurrentTenantId;
    }
}
