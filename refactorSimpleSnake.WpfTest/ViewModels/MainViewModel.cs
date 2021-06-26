using refactorSimpleSnake.WpfTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace refactorSimpleSnake.WpfTest.ViewModels
{
    public class MainViewModel:DependencyObject
    {
        public static GameSettings _settings;
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }
        public List<Record> Records
        {
            get {

                try
                {
                    using (var db = new SnakeContext())
                        return db.Records.OrderByDescending(r => r.Score).ToList();
                }
                catch (Exception)
                {
                    return new List<Record>();                    
                }

            }
        }
        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(MainViewModel), new PropertyMetadata("Username"));

        public ICommand ShowRecordsMenu { get => new DelegateCommand((obj) => {
            var window = new RecordsWindow();
            window.ShowDialog();
        });}
        public ICommand StartingGame { get {
                return new DelegateCommand((obj) => {
                    _settings = new GameSettings(50,50,20);                   
                   var game = new GameWindow();
                    game.Closed += Game_Closed;
                    game.ShowDialog();
                    
                });                    
            } 
        }

        private void Game_Closed(object sender, EventArgs e)
        {
            var game = (GameWindow)sender;
            var data = (GameViewModel)game.DataContext;
            if (data.Score > 0 && !string.IsNullOrWhiteSpace(Username))
            {
                Task.Factory.StartNew(() => {

                    try
                    {
                        using (var db = new SnakeContext())
                        {
                            if (db.Records.OrderBy(r => r.Score).First().Score >= data.Score) return;
                            var record = new Record() { Score = data.Score, Username = this.Username };
                            db.Records.Add(record);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception){}
                });
                
            }
        }
    }
}
