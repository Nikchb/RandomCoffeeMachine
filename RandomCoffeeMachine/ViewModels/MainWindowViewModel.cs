using Newtonsoft.Json;
using RandomCoffeeMachine.Models;
using RandomCoffeeMachine.Services;
using ReactiveUI;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RandomCoffeeMachine.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [JsonProperty]
        public List<Pair> Pairs = new List<Pair>();

        [JsonProperty]
        public ObservableCollection<string> Users { get; set; } = new ObservableCollection<string>();

        [JsonProperty]
        public string RandomCoffee
        {
            get => randomCoffee;
            set => this.RaiseAndSetIfChanged(ref randomCoffee, value);
        }
        [JsonIgnore]
        private string randomCoffee;

        [JsonIgnore]
        public string NewUserName
        {
            get => newUserName;
            set => this.RaiseAndSetIfChanged(ref newUserName, value);
        }
        [JsonIgnore]
        private string newUserName;

        [JsonIgnore]
        public string SelectedUser
        {
            get => selectedUser;
            set => this.RaiseAndSetIfChanged(ref selectedUser, value);
        }
        [JsonIgnore]
        private string selectedUser;

        [JsonIgnore]
        public ReactiveCommand<Unit, Task> AddUserCommand { get; }
        private async Task AddUser()
        {
            if (string.IsNullOrWhiteSpace(NewUserName))
            {
                return;
            }
            Users.Add(NewUserName);
            NewUserName = "";
            await LocalStorageService.SaveAsync(this, "data.json");
        }

        [JsonIgnore]
        public ReactiveCommand<Unit, Task> RemoveUserCommand { get; }
        private async Task RemoveUser()
        {
            if (string.IsNullOrWhiteSpace(SelectedUser))
            {
                return;
            }
            Users.Remove(SelectedUser);
            SelectedUser = "";
            await LocalStorageService.SaveAsync(this, "data.json");
        }

        [JsonIgnore]
        public ReactiveCommand<Unit, Task> MakeRandomCoffeeCommand { get; }
        private async Task MakeRandomCoffee()
        {
            if (Users.Count < 2)
            {
                return;
            }
            
            var round = Pairs.Count > 0 ? Pairs.Last().Round : 0;

            List<Pair> pairs;
            var allPairs = GetAllPairs().OrderBy(v=>Guid.NewGuid());

            do
            {
                round++;
                Pairs = Pairs.Where(v => round - v.Round <= 10).ToList();
                var freePairs = allPairs.Where(v => Pairs.Contains(v) == false).ToList();
                pairs = new List<Pair>();
                for (int i = 0; i < freePairs.Count; i++)
                {
                    if (pairs.All(v => v.HasCrossing(freePairs[i]) == false))
                    {
                        pairs.Add(freePairs[i]);
                    }
                }
            } while (pairs.Count < Users.Count / 2);
                  
            
            RandomCoffee = "Random Coffee";
            for(int i=0; i<pairs.Count; i++)
            {
                RandomCoffee += $"\n{i + 1}. {pairs[i].User1} - {pairs[i].User2}";
            }
            
            if (Users.Count % 2 != 0)
            {
                var usedUsers = new List<string>();
                usedUsers.AddRange(pairs.Select(v => v.User1));
                usedUsers.AddRange(pairs.Select(v => v.User2));
                var user = Users.First(v => usedUsers.Contains(v) == false);
                var lastPair = pairs.Last();
                pairs.Add(new Pair { User1 = user, User2 = lastPair.User1 });
                pairs.Add(new Pair { User1 = user, User2 = lastPair.User2 });
                RandomCoffee += $" - {user}";
            }
            pairs.ForEach(v => v.Round = round);
            Pairs.AddRange(pairs);            
            await LocalStorageService.SaveAsync(this, "data.json");
        }   
        
        private List<Pair> GetAllPairs()
        {
            var pairs = new List<Pair>();
            for(int i =0; i<Users.Count - 1; i++)
            {
                for(int j = i + 1; j<Users.Count; j++)
                {
                    pairs.Add(new Pair { User1 = Users[i], User2 = Users[j] });
                }
            }
            return pairs;
        }

        public MainWindowViewModel()
        {
            AddUserCommand = ReactiveCommand.Create(AddUser);
            RemoveUserCommand = ReactiveCommand.Create(RemoveUser);
            MakeRandomCoffeeCommand = ReactiveCommand.Create(MakeRandomCoffee);
        }
    }
}
