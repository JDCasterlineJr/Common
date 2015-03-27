using (var instance = SingleGlobalInstance.AcquireWithin(TimeSpan.FromSeconds(2)))
{
    if (!instance.HasHandle)
    {
        Console.WriteLine("Another instance of the application is running. "Only a single instance is allowed to run at a time.");
        return;
    }
    Console.WriteLine("Yeah! I have access!");
    Console.ReadLine();
}