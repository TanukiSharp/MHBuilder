using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHBuilder.Core
{
    public static class EnumerableExtensions
    {
        public static async Task IterateWithAirSpace<T>(this IEnumerable<T> source, int itemsInterval, Action<T> onItem)
        {
            await IterateWithAirSpace(source, itemsInterval, onItem, async () => await Task.Yield());
        }

        public static async Task IterateWithAirSpace<T>(this IEnumerable<T> source, int itemsInterval, Action<T> onItem, Func<Task> onAirSpace)
        {
            if (itemsInterval < 0)
                throw new ArgumentException($"Invalid '{nameof(itemsInterval)}' argument. Must be greater or equal to zero.");

            if (itemsInterval == 0)
            {
                foreach (T item in source)
                    onItem(item);
                return;
            }

            int n = 0;

            foreach (T item in source)
            {
                onItem(item);
                n++;

                if (n == itemsInterval)
                {
                    await onAirSpace();
                    n = 0;
                }
            }
        }
    }
}
