using DeploymentManager.AppDomains.Models;
using DeploymentManager.Contracts.Caches;
using DeploymentManager.Contracts.Services;
using DNI.Core.Contracts;
using DNI.Core.Contracts.Managers;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Services.Extensions;
using DNI.Core.Shared.Enumerations;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeploymentManager.Services.Caches
{
    
    public class DeploymentCache : IDeploymentCache
    {
        public DeploymentCache(
            ISystemClock systemClock,
            ILogger<DeploymentCache> logger,
            ICacheState<DateTimeOffset> dateTimeOffsetCacheState,
            ICacheManager cacheManager,
            ITargetService targetService,
            ITargetTypeService targetTypeService)
        {
            dateTimeOffsetCacheState.OnStageItemChanged(OnValueChanged);
            this.dateTimeOffsetCacheState = dateTimeOffsetCacheState;
            this.systemClock = systemClock;
            this.logger = logger;
            this.cacheManager = cacheManager;
            this.targetService = targetService;
            this.targetTypeService = targetTypeService;
        }

        private void OnValueChanged(ICacheStateItem<DateTimeOffset> cacheStateItem)
        {
            logger.LogInformation("Cache State for {0} updated at {1}", 
                cacheStateItem.Key, cacheStateItem.State);
        }

        public Task<IEnumerable<Target>> Targets { 
            get => GetOrSetWhenDefault(CacheType.DistributedCache, nameof(Targets), () => targetService.GetTargetsAsync(null, CancellationToken.None)); 
        }
        public Task<IEnumerable<TargetType>> TargetTypes { 
            get => GetOrSetWhenDefault(CacheType.DistributedCache, nameof(TargetTypes), () => targetTypeService.GetTargetTypes(CancellationToken.None)); 
        }

        private IAsyncCacheService GetAsyncCacheService(CacheType cacheType)
        {
            return cacheManager.GetAsyncCacheService(cacheType);
        }

        private async Task<T> GetAsync<T>(CacheType cacheType, string key)
            where T: class
        {
            var cacheService  = GetAsyncCacheService(cacheType);
            
            var attempt = await cacheService.GetAsync<T>(key, CancellationToken.None);

            if (attempt.Successful && dateTimeOffsetCacheState.IsValid(key, systemClock))
            {
                
                return attempt.Result;
            }

            return default;
        }

        private async Task SetAsync<T>(CacheType cacheType, string key, T value)
            where T: class
        {
            var cacheService  = GetAsyncCacheService(cacheType);
            
            var attempt = await cacheService.SetAsync<T>(key, value, CancellationToken.None);

            if (!attempt.Successful)
            {
                throw attempt.Exception;
            }
        }

        private async Task<T> GetOrSetWhenDefault<T>(CacheType cacheType, string key, Func<Task<T>> populateWhenDefaultDelegate)
            where T: class
        {
            var cachedValue = await GetAsync<T>(cacheType, key);

            if(cachedValue == default)
            {
                var populatedValue = await populateWhenDefaultDelegate();

                if(populatedValue != null)
                {
                    await SetAsync<T>(cacheType, key, populatedValue);
                    return populatedValue;
                }
            }

            return cachedValue;
        }

        public async Task RemoveAsync(CacheType cacheType, string key, CancellationToken cancellationToken)
        {
            var cacheService = GetAsyncCacheService(cacheType);

            await cacheService.RemoveAsync(key, cancellationToken);
        }

        private readonly ICacheState<DateTimeOffset> dateTimeOffsetCacheState;
        private readonly ISystemClock systemClock;
        private readonly ILogger<DeploymentCache> logger;
        private readonly ICacheManager cacheManager;
        private readonly ITargetService targetService;
        private readonly ITargetTypeService targetTypeService;
    }
}
