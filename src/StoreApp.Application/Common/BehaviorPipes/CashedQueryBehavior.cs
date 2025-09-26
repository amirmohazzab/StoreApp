using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StoreApp.Application.Contracts;
using StoreApp.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Common.BehaviorPipes
{
    public class CashedQueryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICashQuery, IRequest<TResponse>
    {
        private readonly IDistributedCache _cache; // save data cache
        private readonly IHttpContextAccessor _httpContextAccessor; //access to request

        public CashedQueryBehavior(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
        }

        private Task CreateNewCache(TRequest request, string key, CancellationToken cancellationToken, byte[] serialized)
        {
            return _cache.SetAsync(key, serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeToLive(request)
                },
                cancellationToken);
        }

        private static TimeSpan TimeToLive(TRequest request)
        {
            if (request.HoursSaveData <= 0)
            {
                return TimeSpan.FromMinutes(1);
            }

            return TimeSpan.FromHours(request.HoursSaveData);
        }

        private string GenerateKey()
        {
            return IdGenerator.GenerateCacheKeyFromRequest(_httpContextAccessor.HttpContext.Request);
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            var key = GenerateKey();
            var cachedResponse = await _cache.GetAsync(key, cancellationToken);
            if (cachedResponse != null)
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
            else
            {
                response = await next(); // go to get response
                var serialized = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
                await CreateNewCache(request, key, cancellationToken, serialized);
            }

            return response;
        }
    }
}
