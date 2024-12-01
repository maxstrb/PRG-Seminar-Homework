using System.Text;

// Původně jsem dělal většinu funkci static podle přílohy, ale to se mi nelíbilo :(, rozhodl jsem se že je budu dělat non-static

internal class BVS<T, U>(T key, U value) where T : IEquatable<T>, IComparable<T> where U : IFormattable
{
    public BVS<T, U>? Left;
    public BVS<T, U>? Right;

    public T Key = key;
    public U? Value = value;

    public BVS<T, U>? Find(T key_to_find)
    {
        return Key.Equals(key_to_find) ? this : Key.CompareTo(key_to_find) < 0 ? Right?.Find(key_to_find) : Left?.Find(key_to_find);
    }

    public void Insert(T key_to_insert, U value_to_insert)
    {
        if (Key.Equals(key_to_insert))
        {
            Value = value_to_insert;
            return;
        }

        ref BVS<T, U>? future_parent = ref Key.CompareTo(key_to_insert) < 0 ? ref Right : ref Left;

        if (future_parent is null)
        {
            future_parent = new(key_to_insert, value_to_insert);
            return;
        }

        future_parent.Insert(key_to_insert, value_to_insert);
    }

    public void Show()
    {
        Console.WriteLine(ToString());
    }

    public BVS<T, U> GetMinimum()
    {
        return (Left is null) ? this : Left.GetMinimum();
    }

    public override string ToString()
    {
        StringBuilder val = ToStringWithTrailingComa();
        _ = val.Remove(val.Length - 2, 2);

        return val.ToString();
    }

    private StringBuilder ToStringWithTrailingComa()
    {
        StringBuilder sb = new();

        if (Left is not null)
        {
            _ = sb.Append(Left.ToStringWithTrailingComa());
        }

        _ = sb.Append(
        Value is null ? "null" : Value.ToString()
        + ", ");

        if (Right is not null)
        {
            _ = sb.Append(Right.ToStringWithTrailingComa());
        }

        return sb;
    }

    public void DeleteByKey(T key_to_delete)
    {
        _ = Delete(key_to_delete);
    }

    public BVS<T, U>? Delete(T key_to_delete)
    {
        if (Key.CompareTo(key_to_delete) > 0)
        {
            if (Left != null)
                Left = Left.Delete(key_to_delete);
        }
        else if (Key.CompareTo(key_to_delete) < 0)
        {
            if (Right != null)
                Right = Right.Delete(key_to_delete);
        }
        else
        {
            // Node to delete found
            if (Left == null)
                return Right;
            if (Right == null)
                return Left;

            // Node with two children
            BVS<T, U> minRight = Right.GetMinimum();
            Key = minRight.Key;
            Value = minRight.Value;
            Right = Right.Delete(minRight.Key);
        }
        return this;
    }
}