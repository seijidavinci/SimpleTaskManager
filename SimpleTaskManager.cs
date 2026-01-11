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

        public Task(int id, string description)
        {
            Id = id;
            Description = description;
            IsCompleted = false;
            CreatedAt = DateTime.Now;
        }

        public override string ToString()
        {
            string status = IsCompleted ? "[✓]" : "[ ]";
            return $"{status} {Id}. {Description} (Created: {CreatedAt:MM/dd/yyyy HH:mm})";
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

        public void AddTask(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Task description cannot be empty!");
                return;
            }

            Task newTask = new Task(nextId++, description);
            tasks.Add(newTask);
            Console.WriteLine($"✓ Task added successfully! (ID: {newTask.Id})");
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            bool running = true;

            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║   WELCOME TO TASK MANAGER APP      ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            while (running)
            {
                Console.WriteLine("\n--- MENU ---");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View All Tasks");
                Console.WriteLine("3. View Pending Tasks");
                Console.WriteLine("4. Complete Task");
                Console.WriteLine("5. Delete Task");
                Console.WriteLine("6. Exit");
                Console.Write("\nChoose an option (1-6): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter task description: ");
                        string description = Console.ReadLine();
                        taskManager.AddTask(description);
                        break;

                    case "2":
                        taskManager.ViewTasks();
                        break;

                    case "3":
                        taskManager.ViewPendingTasks();
                        break;

                    case "4":
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

                    case "5":
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

                    case "6":
                        running = false;
                        Console.WriteLine("\nThank you for using Task Manager. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option! Please choose 1-6.");
                        break;
                }
            }
        }
    }
}