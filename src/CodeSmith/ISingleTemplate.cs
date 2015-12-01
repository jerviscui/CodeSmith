namespace CodeSmith
{
    public interface ISingleTemplate<out T, in TSource>
    {
        T Get(TSource source);
    }
}
