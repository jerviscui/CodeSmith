namespace CodeSmith
{
    public interface ISingleTemplate<out T, in TSource>
    {
        T GetAndCreate(TSource source);
    }
}
