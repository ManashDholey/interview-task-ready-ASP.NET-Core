using System;
using System.Collections.Generic;

// 1. Observer Interface
public interface IObserver
{
    void Update(string status);
}

// 2. Subject Interface
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

// 3. Concrete Subject
public class Order : ISubject
{
    private readonly List<IObserver> _observers = new();
    private string _status = "Pending";

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            Notify();
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_status);
        }
    }
}

// 4. Concrete Observers
public class EmailNotifier : IObserver
{
    public void Update(string status) => 
        Console.WriteLine($"[Email] Order status changed to: {status}");
}

public class SmsNotifier : IObserver
{
    public void Update(string status) => 
        Console.WriteLine($"[SMS] Order status changed to: {status}");
}

// Usage
var order = new Order();
var email = new EmailNotifier();
var sms = new SmsNotifier();

order.Attach(email);
order.Attach(sms);

order.Status = "Shipped"; // Both observers are notified automatically
