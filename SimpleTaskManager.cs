using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleTaskManager
{
    class Task
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Priority { get; set; }  // NEW: Priority field

        public Task(int id, string description, string priority = "Normal")
        {
            Id = id;
            Description = description;
            IsCompleted = false;
            CreatedAt = DateTime.Now;
            Priority = priority;  // NEW: Set priority
        }

        public override string ToString()
        {
            string status = IsCompleted ? "[✓]" : "[ ]";
            string priorityColor = Priority switch
            {
                "High" => "🔴",
                "Low" => "🟢",
                "Normal" => "🟡"
            };
            return $"{status} {priorityColor} {Id}. [{Priority}] {Description} (Created: {CreatedAt:MM/dd/yyyy HH:mm})";
        }
    }

    class TaskManager
    {
        private List<Task> tasks;
        private int nextId;

        public TaskManager()
        {
            tasks = new List<Task>();
            nextId = 1;
        }

        public void AddTask(string description, string priority = "Normal")  // NEW: Priority parameter
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Task description cannot be empty!");
                return;
            }

            // Validate priority
            if (priority != "High" && priority != "Normal" && priority != "Low")
            {
                Console.WriteLine("Invalid priority! Using 'Normal' as default.");
                priority = "Normal";
            }

            Task newTask = new Task(nextId++, description, priority);
            tasks.Add(newTask);
            Console.WriteLine($"✓ Task added successfully with {priority} priority! (ID: {newTask.Id})");
        }

        public void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found. Your list is empty!");
                return;
            }

            Console.WriteLine("\n=== YOUR TASKS ===");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }
            Console.WriteLine($"\nTotal: {tasks.Count} tasks ({tasks.Count(t => t.IsCompleted)} completed)");
        }

        public void CompleteTask(int id)
        {
            Task task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine($"Task with ID {id} not found!");
                return;
            }

            if (task.IsCompleted)
            {
                Console.WriteLine("This task is already completed!");
            }
            else
            {
                task.IsCompleted = true;
                Console.WriteLine($"✓ Task {id} marked as completed!");
            }
        }

        public void DeleteTask(int id)
        {
            Task task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                Console.WriteLine($"Task with ID {id} not found!");
                return;
            }

            tasks.Remove(task);
            Console.WriteLine($"✓ Task {id} deleted successfully!");
        }

        public void ViewPendingTasks()
        {
            var pendingTasks = tasks.Where(t => !t.IsCompleted).ToList();
            
            if (pendingTasks.Count == 0)
            {
                Console.WriteLine("No pending tasks. Great job!");
                return;
            }

            Console.WriteLine("\n=== PENDING TASKS ===");
            foreach (var task in pendingTasks)
            {
                Console.WriteLine(task);
            }
        }

        // NEW: View tasks by priority
        public void ViewTasksByPriority(string priority)
        {
            var priorityTasks = tasks.Where(t => t.Priority == priority).ToList();
            
            if (priorityTasks.Count == 0)
            {
                Console.WriteLine($"No tasks with {priority} priority found!");
                return;
            }

            Console.WriteLine($"\n=== {priority.ToUpper()} PRIORITY TASKS ===");
            foreach (var task in priorityTasks)
            {
                Console.WriteLine(task);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            bool running = true;

            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║   WELCOME TO TASK MANAGER APP      ║");
            Console.WriteLine("║        (WITH PRIORITY FEATURE)     ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            while (running)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View All Tasks");
                Console.WriteLine("3. View Pending Tasks");
                Console.WriteLine("4. View Tasks by Priority");  // NEW
                Console.WriteLine("5. Complete Task");
                Console.WriteLine("6. Delete Task");
                Console.WriteLine("7. Exit");
                Console.Write("\nChoose an option (1-7): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        Console.Write("Enter priority (High/Normal/Low) [Default: Normal]: ");
                        string priority = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(priority)) priority = "Normal";
                        taskManager.AddTask(description, priority);
                        break;

                    case "2":
                        taskManager.ViewTasks();
                        break;

                    case "3":
                        taskManager.ViewPendingTasks();
                        break;

                    case "4":  // NEW
                        Console.Write("Enter priority to view (High/Normal/Low): ");
                        string viewPriority = Console.ReadLine();
                        taskManager.ViewTasksByPriority(viewPriority);
                        break;

                    case "5":
                        Console.Write("Enter task ID to complete: ");
                        if (int.TryParse(Console.ReadLine(), out int completeId))
                        {
                            taskManager.CompleteTask(completeId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID format!");
                        }
                        break;

                    case "6":
                        Console.Write("Enter task ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteId))
                        {
                            taskManager.DeleteTask(deleteId);
                        }
                        else
                        {
                            Console.WriteLine("Invalid ID format!");
                        }
                        break;

                    case "7":
                        running = false;
                        Console.WriteLine("\nThank you for using Task Manager. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option! Please choose 1-7.");
                        break;
                }
            }
        }
    }
}