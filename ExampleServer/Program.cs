using ExampleServer.Data;

TaskModel.TotalTasks = 0;
Console.WriteLine(TaskModel.TotalTasks);

TaskModel task1 = new TaskModel("Wash Car", "The first task");
task1.WriteTotalTasks();

TaskModel task2 = new TaskModel("Make Breakfast", "The second task");
task1.WriteTotalTasks();  //after creation of the second task
task2.WriteTotalTasks();

Console.WriteLine(task2.Id);