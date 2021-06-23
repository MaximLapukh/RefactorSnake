using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace refactorSimpleSnake.WpfTest.ViewModels
{
    public class MainViewModel
    {

        public static GameSettings _settings;


        public ICommand StartingGame { get {
                return new DelegateCommand((obj) => {
                    _settings = new GameSettings(50,50);
                   
                   var game = new GameWindow();
                   if (game.ShowDialog() == true)
                    {

                    }
                });                    
            } 
        }
     
    }
}
