//In C#, disposal patterns provide a standardized way to deterministically release unmanaged resources (such as file handles, database connections, and network sockets) that the .NET Garbage Collector (GC) cannot automatically clean up.
//Because different scenarios require different levels of cleanup, C# uses four primary disposal patterns.

//1. Basic Dispose Pattern (Managed Resources Only)

//Use this pattern when your class is sealed (or won't be inherited) and only holds managed IDisposable objects (like a FileStream or HttpClient) without any raw unmanaged pointers.

public sealed class ResourceHolder : IDisposable
{
    private readonly MemoryStream _stream = new();
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed) return;

        _stream.Dispose();
        _disposed = true;
    }
}
// Use code with caution.
// • Idempotency. Tracking state with _disposed ensures calling Dispose() multiple times will not throw an exception.
// • No finalizer needed. Because the GC already handles managed memory, adding a finalizer here hurts performance.

// 2. Standard Dispose Pattern (Inheritance & Unmanaged Resources)

// This is the classic .NET Framework Design Guidelines pattern. Use it when your class can be inherited by other classes or directly wraps raw unmanaged handles (like an IntPtr).

public class BaseResource : IDisposable
{
    private IntPtr _unmanagedHandle; // Raw unmanaged resource
    private Stream _managedStream;   // Managed IDisposable resource
    private bool _disposed;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this); // Prevents finalizer from running
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Free managed state (other IDisposable objects)
            _managedStream?.Dispose();
        }

        // Free unmanaged resources directly
        if (_unmanagedHandle != IntPtr.Zero)
        {
            // CloseHandle(_unmanagedHandle);
            _unmanagedHandle = IntPtr.Zero;
        }

        _disposed = true;
    }

    // Finalizer (destructor) acts as a safety net if Dispose() wasn't called
    ~BaseResource()
    {
        Dispose(disposing: false);
    }
}
// Use code with caution.
// • Overriding in derived classes. Subclasses override Dispose(bool disposing), clean up their own resources, and call base.Dispose(disposing).
// • Why disposing matters. When called from the finalizer (disposing == false), other managed objects might already be garbage-collected, so you must only release unmanaged memory.

// 3. SafeHandle Pattern (Modern Unmanaged Wrapper)

// Microsoft recommends wrapping raw unmanaged handles in a SafeHandle subclass rather than writing finalizers yourself. SafeHandle inherits from CriticalFinalizerObject, guaranteeing reliable cleanup without the pitfalls of manual finalizers.

public sealed class SafeFileWrapper : IDisposable
{
    // SafeFileHandle derives from SafeHandle
    private readonly SafeFileHandle _handle;
    private bool _disposed;

    public SafeFileWrapper(SafeFileHandle handle) => _handle = handle;

    public void Dispose()
    {
        if (_disposed) return;
        
        _handle?.Dispose(); // SafeHandle handles finalization internally
        _disposed = true;
    }
}
// Use code with caution.

// 4. Asynchronous Dispose Pattern (IAsyncDisposable)

// Introduced in C# 8.0 / .NET Core 3.0, use this when cleanup involves asynchronous I/O operations (like flushing a buffer to disk or closing a network stream without blocking a thread).

public class AsyncResource : IAsyncDisposable
{
    private readonly Stream _stream = new MemoryStream();

    public async ValueTask DisposeAsync()
    {
        await _stream.DisposeAsync().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }
}
// Use code with caution.
// • Consumption. Consumed using await using var resource = new AsyncResource();.
• Dual implementation. Classes often implement both IDisposable and IAsyncDisposable to support both synchronous and asynchronous callers.
