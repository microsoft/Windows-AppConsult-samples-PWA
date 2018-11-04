using Microsoft.Services.Store.Engagement;
using System;
using System.Threading.Tasks;
using Windows.Foundation;

namespace PushWinRTComponent
{
    public sealed class PushNotifications
    {
        public static IAsyncOperation<int> Init()
        {
            return Task.Run(async () =>
            {
                StoreServicesEngagementManager engagementManager = StoreServicesEngagementManager.GetDefault();
                var res = await engagementManager.RegisterNotificationChannelAsync();
                return (int)res.ErrorCode;
            }).AsAsyncOperation();
        }
    }
}
