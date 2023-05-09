using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RackDAT_API.Middlewares
{
    public class EjemploMiddleware
    {
        private readonly RequestDelegate _next;
        public EjemploMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
    }
}
