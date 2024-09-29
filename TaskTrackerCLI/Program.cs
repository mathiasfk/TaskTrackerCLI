using Adapters.Output.Console;
using Adapters.Persistence.JsonFile;
using Core.Ports;
using Core.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskTrackerCLI;

//HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
//builder.Logging.AddConsole();

////builder.Services.AddHostedService<Worker>();
//builder.Services.AddSingleton<IOutputPort, ConsoleOutputAdapter>();
//builder.Services.AddSingleton<IPersistencePort, JsonFilePersistenceAdapter>();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<Driver>();
        services.AddTransient<ICommandProcessor, CommandProcessor>();
        services.AddTransient<IOutputPort, ConsoleOutputAdapter>();
        services.AddTransient<IPersistencePort, JsonFilePersistenceAdapter>();
    }).Build();

Driver d = host.Services.GetRequiredService<Driver>();
await d.RunAsync(default);


//using System.Text.Json;

//const string CMD_ADD = "add";
//const string CMD_UPDATE = "update";
//const string CMD_DELETE = "delete";
//const string CMD_MARK_IN_PROGRESS = "mark-in-progress";
//const string CMD_MARK_DONE = "mark-done";
//const string CMD_LIST = "list";

//string filename = "task-tracket-state.json";

//int maxID = 0;

//// LOAD
//List<TaskRecord> tasks;
//if (File.Exists(filename))
//{
//    string jsonString = File.ReadAllText(filename);
//    tasks = JsonSerializer.Deserialize<List<TaskRecord>>(jsonString);
//    if (tasks.Count > 0)
//    {
//        maxID = tasks.Max(r => r.Id);
//    }

//}
//else
//{
//    tasks = new List<TaskRecord>();
//}

//// PARSE
//int id = 0;
//string description = "";
//string status = "";

//switch (args[0])
//{
//    case CMD_ADD:
//        description = args[1];
//        id = maxID + 1;
//        status = "todo";
//        break;

//    case CMD_UPDATE:
//        id = int.Parse(args[1]);
//        description = args[2];
//        break;

//    case CMD_DELETE:
//        id = int.Parse(args[1]);
//        break;

//    case CMD_LIST:
//        if(args.Length > 1){
//            status = args[1];
//        }
//        break;

//    case CMD_MARK_IN_PROGRESS:
//        id = int.Parse(args[1]);
//        status = "in-progress";
//        break;

//    case CMD_MARK_DONE:
//        id = int.Parse(args[1]);
//        status = "done";
//        break;

//    default:
//        Console.WriteLine("Invalid command");
//        return;
//}

//// ACT
//switch (args[0])
//{
//    case CMD_ADD:
//        var newTask = new TaskRecord()
//        {
//            Id = id,
//            Description = description,
//            Status = status
//        };
//        tasks.Add(newTask);
//        break;

//    case CMD_UPDATE:
//        var updatedTask = tasks.Where(r => r.Id == id).FirstOrDefault();
//        updatedTask.Description = description;
//        break;

//    case CMD_DELETE:
//        tasks.RemoveAll(r => r.Id == id);
//        break;

//    case CMD_LIST:
//        var filteredTasks = tasks.Where(r => status == "" || r.Status.Equals(status));
//        foreach(var task in filteredTasks)
//        {
//            Console.WriteLine($"{task.Id} - {task.Description} [{task.Status}]");
//        }
//        break;

//    case CMD_MARK_IN_PROGRESS:
//    case CMD_MARK_DONE:
//        var changeStatusTask = tasks.Where(r => r.Id == id).FirstOrDefault();
//        changeStatusTask.Status = status;
//        break;

//    default:
//        Console.WriteLine("Invalid command");
//        break;
//}


//// FLUSH
//string outputString = JsonSerializer.Serialize(tasks);
//File.WriteAllText(filename, outputString);