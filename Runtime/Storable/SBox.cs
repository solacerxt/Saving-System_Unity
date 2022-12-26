
namespace solacerxt.SaveSystem
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
