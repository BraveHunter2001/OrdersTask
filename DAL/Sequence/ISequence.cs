using Microsoft.EntityFrameworkCore;

namespace DAL.Secuenser;

public interface ISequence<T>
{
    T GetNextValue(SequenceType sequenceType);
}

internal class Sequence<T>(OrdersTaskContext context) : ISequence<T>
{
    public T GetNextValue(SequenceType sequenceType)
    {
        string sql = $"select nextval('\"{sequenceType.ToString()}\"')";
        return context.Database.SqlQueryRaw<T>(sql).AsEnumerable().First();
    }
}