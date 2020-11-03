using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MHBuilder.WPF
{
    public static class EnumerableExtensions
    {
        public static async Task IterateWithAirSpace<T>(this IEnumerable<T> source, int itemsInterval, Action<T> onItem)
        {
            await IterateWithAirSpace(source, itemsInterval, onItem, DispatcherPriority.SystemIdle);
        }

        public static async Task IterateWithAirSpace<T>(this IEnumerable<T> source, int itemsInterval, Action<T> onItem, DispatcherPriority airSpacePriority)
        {
            await Core.EnumerableExtensions.IterateWithAirSpace(source, itemsInterval, onItem, async () => await Dispatcher.Yield(airSpacePriority));
        }
    }
}
