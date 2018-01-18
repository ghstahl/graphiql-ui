using System.Collections.Generic;

namespace Command.Common
{
    public class ItemsContainer<T>
    {
        public ItemsContainer(List<T> items)
        {
            Items = items;
        }
        public ItemsContainer(T[] items)
        {
            Items = new List<T>(items);
        }
        public List<T> Items { get; set; }
    }
}