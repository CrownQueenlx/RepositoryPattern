using ExampleServer.Data;

// Instance of our class
TaskModel.TotalTasks = 0;
Console.WriteLine(TaskModel.TotalTasks);

TaskModel task1 = new TaskModel("Wash Car", "The first task");
task1.WriteTotalTasks();

TaskModel task2 = new TaskModel("Make Breakfast", "The second task");
task1.WriteTotalTasks();  //after creation of the second task
task2.WriteTotalTasks();

Console.WriteLine(task2.Id);

// Implicit Types
// var assumes the type from the righthand side of the expression
var task3 = new TaskModel("Task 3", "The third task");


// Target-typed new
//Implicit new will assume the type from the lefthand side
TaskModel task4 = new("Task 4", "The fourth task");
//examples of implicit types
var task5 = new TaskModel("", "");
TaskModel task6 = new("","");

task1.IsComplete = true;
task4.IsComplete = true;


TaskRepository repo = new();
repo.AddTask(task1);
repo.AddTask(task2);
repo.AddTask(task3);
repo.AddTask(task4);

repo.DeleteTaskById(3);


// tasks gets its type form the GetTasks() return type (becasue it is implicit)
// var tasks = repo.GetTasks();
var tasks = repo.GetTasksByStatus(true);
foreach (var task in tasks)
{   //ternary
    string status = task.IsComplete? "complete": "incomplete";
    Console.WriteLine($"{task.Description} is {status}.");
}
