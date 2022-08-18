
namespace solacerxt.Saving
{
    [System.Serializable]
    public struct SBox<T> : IStorable
    {
        public T Value;

        public SBox(T value)
        {
            Value = value;
        }
    }
}
