using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MatcherChief.Server.Queues.Auditing;

public interface IAuditLoggerFactory
{
    IAuditLogger Get();
}

public class AuditLoggerFactory : IAuditLoggerFactory
{
    private readonly IWebHostEnvironment _env;

    public AuditLoggerFactory(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IAuditLogger Get()
    {
        if (!_env.IsDevelopment())
            return new NoopAuditLogger();

        var path = Path.Combine(_env.ContentRootPath, "audit.json");
        var auditLogger = new AuditLogger(path);
        return auditLogger;
    }
}
