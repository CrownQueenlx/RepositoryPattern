// TaskModel is out POCO (plain old C# object)
// It is going to represent the data object

namespace ExampleServer.Data;

// Task or ToDo is something we want to get done
// A task Model instance represents a single task
// Identifies, Title, Descrition, a competion status
public class TaskModel
{
    public static int TotalTasks = 0;

    // constructor
    public TaskModel(string title, string description)
    {
        TotalTasks++;
        Id = TotalTasks;

        Title = title;
        Description = description;
    }
    // properties
    public int Id { get; } //id is only set through new construction
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }

    // method
    public void WriteTotalTasks()
    {
        Console.WriteLine($"Task {Id}/{TotalTasks}"); //num id of total
    }
}
