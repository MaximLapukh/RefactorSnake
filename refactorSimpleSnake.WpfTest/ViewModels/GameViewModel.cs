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
        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }       
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(GameViewModel), new PropertyMetadata(0));

        private Canvas space;
        private Snake snake;
        private IGame game;

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
        public ICommand StopAndContinue => new DelegateCommand((obj) =>
        {
            game.isStop = !game.isStop;
        });
        public GameViewModel(GameWindow window)
        {
            HeightWindow = MainViewModel._settings._height * CellSize;
            WidthWindow = MainViewModel._settings._width * CellSize;
            space = window.DynSpace;

            game = new Game(MainViewModel._settings, new FactoryWallsAround(),new EatToCreate());
            game.Update += ChangedView;
            game.Stop += GameOver;
            game.Start();

            foreach (var wall in game.GetStaticObjs())
            {
                window.Background.Children.Add(CreateViewObj(wall));
            }
            snake =  game.AddSnake(new Vector2(1, 9), 3, Direction.up);
        }
        private void GameOver(object sender, EventArgs e)
        {
            if (!snake.die) return;
            MessageBox.Show($"Score: {snake.score}","Game Over",MessageBoxButton.OK,MessageBoxImage.Information);
        }
        private Rectangle CreateViewObj(GameObject gObj)
        {
            var viewObj =  new Rectangle()
            {
                Width = CellSize,
                Height = CellSize,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            viewObj.Fill = GetColorType(gObj.GetType());
            Canvas.SetLeft(viewObj, gObj.position.x * CellSize);
            Canvas.SetTop(viewObj, gObj.position.y * CellSize);
            return viewObj;
        }
        private void ChangedView(object sender, GameEventArgs e)
        {            
            Action action = new Action(() =>
            {
                Score = snake.score;
                space.Children.Clear();
                foreach (var gObj in e.dynObjs)
                {
                    foreach (var child in gObj.ToList())
                    {       
                        space.Children.Add(CreateViewObj(child));
                    }
                }
            });
            if (!Dispatcher.CheckAccess()) Dispatcher.Invoke(action);
            else action();
        }
        private Brush GetColorType(Type type)
        {
            return type switch
            {
                { Name: "Food" } => Brushes.GreenYellow,
                { Name: "Snake" } => Brushes.Red,
                { Name: "Segment" } => Brushes.Orange,
                { Name: "Wall" } => Brushes.White,
                _ => Brushes.Purple
            };
        }
    }
}
