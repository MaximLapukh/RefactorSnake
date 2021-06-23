using refactorSimpleSnake.FactoryFoods;
using refactorSimpleSnake.FactoryWalls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace refactorSimpleSnake.WpfTest.ViewModels
{
    public class GameViewModel:DependencyObject
    {
        public int WidthWindow
        {
            get { return (int)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(int), typeof(GameViewModel), new PropertyMetadata(0));
        public int HeightWindow
        {
            get { return (int)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(int), typeof(GameViewModel), new PropertyMetadata(0));
        private Canvas gameField;
        private Snake snake;
        public ICommand InputUp => new DelegateCommand((o) =>
        {
            snake.direction = Direction.down;
        });
        public ICommand InputDown => new DelegateCommand((o) =>
        {
            snake.direction = Direction.up;
        });
        public ICommand InputRight => new DelegateCommand((o) =>
        {
            snake.direction = Direction.right;
        });
        public ICommand InputLeft => new DelegateCommand((o) =>
        {
            snake.direction = Direction.left;
        });
        public GameViewModel()
        {

        }
        public GameViewModel(GameWindow window)
        {
            HeightWindow = MainViewModel._settings._height * 10;
            WidthWindow = MainViewModel._settings._width * 10;
            gameField = window.dynamicField;

            Game game = new Game(MainViewModel._settings, new FactoryWallsAround(), new EatToCreate());
            game.Update += ChangedView;
            ViewWalls(window, game.GetStaticObjs());

            game.Start();
           snake =  game.AddSnake(new Vector2(1, 9), 3, Direction.up);
        }

        private static void ViewWalls(GameWindow window, List<GameObject> walls)
        {
            foreach (var wall in walls)
            {
                var viewWall = new Rectangle()
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(viewWall, wall.position.x * 10);
                Canvas.SetTop(viewWall, wall.position.y * 10);
                window.Field.Children.Add(viewWall);
            }
        }

        private void ChangedView(object sender, EventArgs e)
        {
            var game = sender as Game;
            var dynObjs = game.GetDynamicObjs();

            Action action = new Action(() =>
            {
                gameField.Children.Clear();

                foreach (var gObj in dynObjs)
                {
                    var list_gObj = gObj.ToList();
                    foreach (var item in list_gObj)
                    {
                        var viewgObj = new Rectangle()
                        {
                            Width = 10,
                            Height = 10,
                            Fill = Brushes.White,
                            Stroke = Brushes.Black,
                            StrokeThickness = 0.5

                        };
                        if (item is Food)
                        {
                            viewgObj.Fill = Brushes.GreenYellow;
                        }
                        else if (item is Snake || item is Segment)
                        {
                            viewgObj.Fill = Brushes.Red;
                        }

                        Canvas.SetLeft(viewgObj, item.position.x * 10);
                        Canvas.SetTop(viewgObj, item.position.y * 10);

                        gameField.Children.Add(viewgObj);
                    }
                }
            });
            if (!Dispatcher.CheckAccess()) Dispatcher.Invoke(action);
            else action();
        }
    }
}
