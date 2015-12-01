namespace CodeSmith
{
    public abstract class SingleTemplate<T, TSource> : ISingleTemplate<T, TSource>
    {
        public abstract T Get(TSource source);
    }
}
