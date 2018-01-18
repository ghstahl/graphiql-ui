namespace Command.Common
{
    public class ItemContainer<T>
    {
        public ItemContainer(T item)
        {
            Item = item;
        }
        public T Item { get; set; }
    }
}