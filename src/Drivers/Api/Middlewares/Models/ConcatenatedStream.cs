namespace Api.Middlewares.Models;

public class ConcatenatedStream : Stream
{
    private readonly Stream _first;
    private readonly Stream _second;
    private readonly long _length;

    public ConcatenatedStream(Stream first, Stream second, long length)
    {
        _first = first;
        _second = second;
        _length = length;
    }

    public override async ValueTask<int> ReadAsync(
        Memory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        int read = await _first.ReadAsync(buffer, cancellationToken);

        if (read > 0)
            return read;

        return await _second.ReadAsync(buffer, cancellationToken);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int read = _first.Read(buffer, offset, count);

        if (read > 0)
            return read;

        return _second.Read(buffer, offset, count);
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => _length;
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    public override void Flush() { }

    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException();

    public override void SetLength(long value) =>
        throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException();
}