namespace CodeSmith
{
    public abstract class SingleTemplate<T, TSource> : ISingleTemplate<T, TSource>
    {
        public abstract T GetAndCreate(TSource source);
    }
}
