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
        public const int CellSize = 10;
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
        private Canvas dynSpace;
        private Snake snake;


        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Score.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(GameViewModel), new PropertyMetadata(0));


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
        public GameViewModel(GameWindow window)
        {
            HeightWindow = MainViewModel._settings._height * CellSize;
            WidthWindow = MainViewModel._settings._width * CellSize;
            dynSpace = window.DynSpace;

            IGame game = new Game(MainViewModel._settings, new FactoryWallsAround(), new EatToCreate());
            game.Update += ChangedView;
            foreach (var wall in InitWalls(game.GetStaticObjs()))
            {
                window.Background.Children.Add(wall);
            }
           game.Start();
           snake =  game.AddSnake(new Vector2(1, 9), 3, Direction.up);
        }
        private IEnumerable<Rectangle> InitWalls(List<GameObject> walls)
        {
            foreach (var wall in walls)
            {
                var viewWall = CreateViewObj(wall);
                viewWall.Fill = Brushes.White;
                yield return viewWall;
            }
        }
        private Rectangle CreateViewObj(GameObject gObj)
        {
            var viewObj =  new Rectangle()
            {
                Width = CellSize,
                Height = CellSize,
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(viewObj, gObj.position.x * CellSize);
            Canvas.SetTop(viewObj, gObj.position.y * CellSize);
            return viewObj;
        }
        private void ChangedView(object sender, EventArgs e)
        {
            
            var game = sender as Game;
            var list = game.GetDynamicObjs();
            Action action = new Action(() =>
            {
                Score = snake.score;
                dynSpace.Children.Clear();
                foreach (var gObj in list)
                {
                    var children = gObj.ToList();
                    foreach (var child in children)
                    {
                        var viewgObj = CreateViewObj(child);
                        Type type = child.GetType();
                        //switch
                        if (type == typeof(Food))                        
                            viewgObj.Fill = Brushes.GreenYellow;                        
                        else if (type == typeof(Snake))
                            viewgObj.Fill = Brushes.Red;
                        else if (type == typeof(Segment))
                            viewgObj.Fill = Brushes.Orange;

                        dynSpace.Children.Add(viewgObj);
                    }
                }
            });
            if (!Dispatcher.CheckAccess()) Dispatcher.Invoke(action);
            else action();
        }
    }
}
