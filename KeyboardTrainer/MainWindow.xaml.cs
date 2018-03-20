using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyboardTrainer
{
	/// <summary>
	/// Represents a key (button) of the graphical keyboard shown in the main window.
	/// </summary>
	class KeyboardButton
	{
		public string Value { get; private set; }
		public string ShiftValue { get; private set; }
		public UIElement GridElement { get; private set; }
		public TextBlock TextElement { get; private set; }

		/// <summary>
		/// Initializes a new instance of the KeyboardButton class.
		/// </summary>
		/// <param name="regularValue">Text on the button when Shift key is not pressed (digit or small letter)</param>
		/// <param name="shiftValue">Text on the button when Shift key is pressed (capital letter or special character in case of a digit button)</param>
		/// <param name="row">Row of the window grid where the button is located</param>
		/// <param name="column">Column of the window grid where the button is located</param>
		/// <param name="columnSpan">How many columns of the window grid the button occupies</param>
		/// <param name="backgroundColor">Background color of the keyboard button</param>
		public KeyboardButton(string regularValue, string shiftValue, int row, int column, int columnSpan, Color backgroundColor)
		{
			Value = regularValue;
			ShiftValue = shiftValue;

			Border border = new Border
			{
				Margin = new Thickness(2.0),
				//Padding = new Thickness(0, -4.0, 0, 0),
				BorderBrush = new SolidColorBrush(Colors.Black),
				BorderThickness = new Thickness(1.5),
				Background = new SolidColorBrush(backgroundColor),
				CornerRadius = new CornerRadius(7.0)
			};

			TextBlock textBlock = new TextBlock
			{
				Text = regularValue,
				FontSize = 24.0,
				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Center
			};

			border.Child = textBlock;
			Grid.SetRow(border, row);
			Grid.SetColumn(border, column);
			Grid.SetColumnSpan(border, columnSpan);

			TextElement = textBlock;
			GridElement = border;
		}

		/// <summary>
		/// Renews the text on the keyboard button when Shift or Caps Lock is pressed.
		/// </summary>
		/// <param name="shiftIsOn">True if Shift key is pressed, false otherwise</param>
		/// <param name="capsIsOn">True if Caps Lock key is toggled, false otherwise</param>
		public virtual void RefreshText(bool shiftIsOn, bool capsIsOn)
		{ }
	}

	/// <summary>
	/// A key (button) that represents a single letter.
	/// </summary>
	class LetterKey : KeyboardButton
	{
		public LetterKey(string value, int row, int column, Color backgroundColor)
			: base(value.ToLower(), value.ToUpper(), row, column, 2, backgroundColor)
		{ }

		public override void RefreshText(bool shiftIsOn, bool capsIsOn)
		{
			if (shiftIsOn ^ capsIsOn)
			{
				TextElement.Text = ShiftValue;
			}
			else
			{
				TextElement.Text = Value;
			}
		}
	}

	/// <summary>
	/// A key (button) that represents a single special character (which is not a digit or a letter). 
	/// </summary>
	class SpecialCharKey : KeyboardButton
	{
		public SpecialCharKey(string regularValue, string shiftValue, int row, int column, int columnSpan, Color backgroundColor)
			: base(regularValue, shiftValue, row, column, columnSpan, backgroundColor)
		{ }

		public override void RefreshText(bool shiftIsOn, bool capsIsOn)
		{
			if (shiftIsOn)
			{
				TextElement.Text = ShiftValue;
			}
			else
			{
				TextElement.Text = Value;
			}
		}
	}

	/// <summary>
	/// A key (button) that represents a digit. 
	/// </summary>
	class DigitKey : SpecialCharKey
	{
		public DigitKey(string regularValue, string shiftValue, int row, int column, Color backgroundColor)
			: base(regularValue, shiftValue, row, column, 2, backgroundColor)
		{ }
	}

	/// <summary>
	/// A key (button) that represents the Space key.
	/// </summary>
	class SpaceKey : SpecialCharKey
	{
		public SpaceKey(int row, int column, int columnSpan, Color backgroundColor)
			: base("Space", "Space", row, column, columnSpan, backgroundColor)
		{
			TextElement.FontSize = 16.0;
		}

		// GetTextValue " "
	}

	/// <summary>
	/// A key (button) that represents a control key (which is not a single printable character).
	/// </summary>
	class ControlKey : KeyboardButton
	{
		public ControlKey(string value, int row, int column, int columnSpan)
			: base(value, value, row, column, columnSpan, Colors.LightGray)
		{
			TextElement.FontSize = 16.0;
		}
	}


	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Dictionary<Key, KeyboardButton> keyboardButtons;

		public MainWindow()
		{
			InitializeComponent();
			keyboardButtons = new Dictionary<Key, KeyboardButton>();

			keyboardButtons[Key.Oem3] = new SpecialCharKey("`", "~", 0, 0, 2, Colors.Pink);
			keyboardButtons[Key.D1] = new DigitKey("1", "!", 0, 2, Colors.Pink);
			keyboardButtons[Key.D2] = new DigitKey("2", "@", 0, 4, Colors.Pink);
			keyboardButtons[Key.D3] = new DigitKey("3", "#", 0, 6, Colors.Yellow);
			keyboardButtons[Key.D4] = new DigitKey("4", "$", 0, 8, Colors.LawnGreen);
			keyboardButtons[Key.D5] = new DigitKey("5", "%", 0, 10, Colors.DeepSkyBlue);
			keyboardButtons[Key.D6] = new DigitKey("6", "^", 0, 12, Colors.DeepSkyBlue);
			keyboardButtons[Key.D7] = new DigitKey("7", "&", 0, 14, Colors.MediumVioletRed);
			keyboardButtons[Key.D8] = new DigitKey("8", "*", 0, 16, Colors.MediumVioletRed);
			keyboardButtons[Key.D9] = new DigitKey("9", "(", 0, 18, Colors.Pink);
			keyboardButtons[Key.D0] = new DigitKey("0", ")", 0, 20, Colors.Yellow);
			keyboardButtons[Key.OemMinus] = new SpecialCharKey("-", "_", 0, 22, 2, Colors.LawnGreen);
			keyboardButtons[Key.OemPlus] = new SpecialCharKey("=", "+", 0, 24, 2, Colors.LawnGreen);
			keyboardButtons[Key.Back] = new ControlKey("Backspace", 0, 26, 4);
			keyboardButtons[Key.Tab] = new ControlKey("Tab", 1, 0, 3);
			keyboardButtons[Key.Q] = new LetterKey("Q", 1, 3, Colors.Pink);
			keyboardButtons[Key.W] = new LetterKey("W", 1, 5, Colors.Yellow);
			keyboardButtons[Key.E] = new LetterKey("E", 1, 7, Colors.LawnGreen);
			keyboardButtons[Key.R] = new LetterKey("R", 1, 9, Colors.DeepSkyBlue);
			keyboardButtons[Key.T] = new LetterKey("T", 1, 11, Colors.DeepSkyBlue);
			keyboardButtons[Key.Y] = new LetterKey("Y", 1, 13, Colors.MediumVioletRed);
			keyboardButtons[Key.U] = new LetterKey("U", 1, 15, Colors.MediumVioletRed);
			keyboardButtons[Key.I] = new LetterKey("I", 1, 17, Colors.Pink);
			keyboardButtons[Key.O] = new LetterKey("O", 1, 19, Colors.Yellow);
			keyboardButtons[Key.P] = new LetterKey("p", 1, 21, Colors.LawnGreen);
			keyboardButtons[Key.OemOpenBrackets] = new SpecialCharKey("[", "{", 1, 23, 2, Colors.LawnGreen);
			keyboardButtons[Key.OemCloseBrackets] = new SpecialCharKey("]", "}", 1, 25, 2, Colors.LawnGreen);
			keyboardButtons[Key.Oem5] = new SpecialCharKey("\\", "|", 1, 27, 3, Colors.LawnGreen);
			keyboardButtons[Key.CapsLock] = new ControlKey("Caps Lock", 2, 0, 4);
			keyboardButtons[Key.A] = new LetterKey("A", 2, 4, Colors.Pink);
			keyboardButtons[Key.S] = new LetterKey("S", 2, 6, Colors.Yellow);
			keyboardButtons[Key.D] = new LetterKey("D", 2, 8, Colors.LawnGreen);
			keyboardButtons[Key.F] = new LetterKey("F", 2, 10, Colors.DeepSkyBlue);
			keyboardButtons[Key.G] = new LetterKey("G", 2, 12, Colors.DeepSkyBlue);
			keyboardButtons[Key.H] = new LetterKey("H", 2, 14, Colors.MediumVioletRed);
			keyboardButtons[Key.J] = new LetterKey("J", 2, 16, Colors.MediumVioletRed);
			keyboardButtons[Key.K] = new LetterKey("K", 2, 18, Colors.Pink);
			keyboardButtons[Key.L] = new LetterKey("L", 2, 20, Colors.Yellow);
			keyboardButtons[Key.OemSemicolon] = new SpecialCharKey(";", ":", 2, 22, 2, Colors.LawnGreen);
			keyboardButtons[Key.OemQuotes] = new SpecialCharKey("'", "\"", 2, 24, 2, Colors.LawnGreen);
			keyboardButtons[Key.Enter] = new ControlKey("Enter", 2, 26, 4);
			keyboardButtons[Key.LeftShift] = new ControlKey("Shift", 3, 0, 5);
			keyboardButtons[Key.Z] = new LetterKey("Z", 3, 5, Colors.Pink);
			keyboardButtons[Key.X] = new LetterKey("X", 3, 7, Colors.Yellow);
			keyboardButtons[Key.C] = new LetterKey("C", 3, 9, Colors.LawnGreen);
			keyboardButtons[Key.V] = new LetterKey("V", 3, 11, Colors.DeepSkyBlue);
			keyboardButtons[Key.B] = new LetterKey("B", 3, 13, Colors.DeepSkyBlue);
			keyboardButtons[Key.N] = new LetterKey("N", 3, 15, Colors.MediumVioletRed);
			keyboardButtons[Key.M] = new LetterKey("M", 3, 17, Colors.MediumVioletRed);
			keyboardButtons[Key.OemComma] = new SpecialCharKey(",", "<", 3, 19, 2, Colors.Pink);
			keyboardButtons[Key.OemPeriod] = new SpecialCharKey(".", ">", 3, 21, 2, Colors.Yellow);
			keyboardButtons[Key.OemQuestion] = new SpecialCharKey("/", "?", 3, 23, 2, Colors.LawnGreen);
			keyboardButtons[Key.RightShift] = new ControlKey("Shift", 3, 25, 5);
			keyboardButtons[Key.LeftCtrl] = new ControlKey("Ctrl", 4, 0, 3);
			keyboardButtons[Key.LWin] = new ControlKey("Win", 4, 3, 3);
			keyboardButtons[Key.LeftAlt] = new ControlKey("Alt", 4, 6, 3);
			keyboardButtons[Key.Space] = new SpaceKey(4, 9, 12, Colors.Orange);
			keyboardButtons[Key.RightAlt] = new ControlKey("Alt", 4, 21, 3);
			keyboardButtons[Key.RWin] = new ControlKey("Win", 4, 24, 3);
			keyboardButtons[Key.RightCtrl] = new ControlKey("Ctrl", 4, 27, 3);

			foreach (KeyboardButton keyboardButton in keyboardButtons.Values)
			{
				keyboardGrid.Children.Add(keyboardButton.GridElement);
			}
		}


		private void btnStart_Click(object sender, RoutedEventArgs e)
		{
			btnStop.IsEnabled = true;
			btnStart.IsEnabled = false;
			cbCaseSensitive.IsEnabled = false;
			sliderDifficulty.IsEnabled = false;
			tbTypedText.Text = "";
		}

		private void btnStop_Click(object sender, RoutedEventArgs e)
		{
			btnStop.IsEnabled = false;
			btnStart.IsEnabled = true;
			cbCaseSensitive.IsEnabled = true;
			sliderDifficulty.IsEnabled = true;
		}


		private void mainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (keyboardButtons.ContainsKey(e.Key))
				keyboardButtons[e.Key].GridElement.Effect = new DropShadowEffect();
		}

		private void mainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (keyboardButtons.ContainsKey(e.Key))
				keyboardButtons[e.Key].GridElement.Effect = null;
		}


		private void mainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.Key == Key.CapsLock)
			{
				RefreshKeyboard();
			}
			//MessageBox.Show(e.Key.ToString(), "Key Down");
		}

		private void mainWindow_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
			{
				RefreshKeyboard();
			}
		}

		private void RefreshKeyboard()
		{
			bool shiftIsOn = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
			bool capsIsOn = Keyboard.IsKeyToggled(Key.CapsLock);
			foreach (KeyboardButton keyboardButton in keyboardButtons.Values)
			{
				keyboardButton.RefreshText(shiftIsOn, capsIsOn);
			}
		}

	}
}
