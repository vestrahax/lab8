using Avalonia.Controls;
using HW_8.Models;
using HW_8.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace HW_8.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ObservableCollection<Task>> Tasks { get; set; }

        public MainWindowViewModel()
        {
            this.Tasks = new ObservableCollection<ObservableCollection<Task>>();
            for(int i = 0; i < 3; i++)
            {
                this.Tasks.Add(new ObservableCollection<Task>());
            }
        }

        public void AddTask(int numberOfList)
        {
            this.Tasks[numberOfList].Add(new Task("NEW TASK"));
        }


        public void ClearData()
        {
            this.Tasks.Clear();
            for(int i = 0; i < 3; i++)
            {
                this.Tasks.Add(new ObservableCollection<Task>());
            }
        }

        public async void SaveFileDialogue(Window parent)
        {
            var taskPath = new SaveFileDialog().ShowAsync(parent);
            string? path = await taskPath;
           
            if (path is not null)
            {
                this.SaveCollection(path);
            }
        }

        public async void LoadFileDialogue(Window parent)
        {
            var taskPath = new OpenFileDialog().ShowAsync(parent);
            string[]? path = await taskPath;
            if (path is not null)
            {
                this.ReadCollection(string.Join("/", path));
            }
        }

        public void SaveCollection(string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                var xs = new XmlSerializer(typeof(ObservableCollection<ObservableCollection<Task>>));
                xs.Serialize(writer, this.Tasks);
            }
        }

        public void ReadCollection(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ObservableCollection<Task>>));
            using (StreamReader sr = new StreamReader(fileName))
            {
                var tasks = xs.Deserialize(sr) as ObservableCollection<ObservableCollection<Task>>;
                this.Tasks.Clear();
                foreach (var task in tasks)
                {
                    this.Tasks.Add(task);
                }
            }
        }
        public void RemoveTask(Task task)
        {
            for (int i = 0; i < 3; i++)
            {
                this.Tasks[i].Remove(task);
            }
        }
        public void CloseWindow(Window parent)
       {
            parent.Close();
       }
    }
}
