using System.Runtime.CompilerServices;

namespace AdventOfCode.Library;

public ref struct UnionFind
{
    private int _distinct;
    private int _count;
    private Span<int> _parents;
    private Span<int> _size;

    public readonly int Count => _count;
    public readonly ReadOnlySpan<int> Parents => _parents;
    public readonly ReadOnlySpan<int> Size => _size;
    public readonly int Distinct => _distinct;

    public UnionFind(int size)
    {
        _distinct = size;
        _count = size;
        _parents = new int[size];
        _size = new int[size];

        for (int i = 0; i < size; i++)
        {
            _parents[i] = i;
            _size[i] = 1;
        }
    }

    public UnionFind(Span<int> parents, Span<int> sizes)
    {
        int size = parents.Length;

        _distinct = size;
        _count = size;
        _parents = parents;
        _size = sizes;

        for (int i = 0; i < size; i++)
        {
            _parents[i] = i;
            _size[i] = 1;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Find(int node)
    {
        int curr = node;
        while (_parents[curr] != curr)
            curr = _parents[curr];

        _parents[node] = curr;

        return curr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Union(int x, int y)
    {
        var xp = Find(x);
        var yp = Find(y);

        if (xp == yp)
            return;

        if (_size[xp] < _size[yp])
            (xp, yp) = (yp, xp);

        _parents[yp] = xp;
        _size[xp] += _size[yp];
        _distinct--;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsConnected(int x, int y)
        => Find(x) == Find(y);
}