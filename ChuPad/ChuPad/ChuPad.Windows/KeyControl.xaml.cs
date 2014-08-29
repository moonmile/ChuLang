using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace ChuPad
{
    public sealed partial class KeyControl : UserControl
    {
        public KeyControl()
        {
            this.InitializeComponent();

            makeKeycode();
        }

        void makeKeycode()

        {
            KCs = new KC[] {
                new KC( VirtualKey.Number1, "1", "!"),
                new KC( VirtualKey.Number2, "2", "\""),
                new KC( VirtualKey.Number3, "3", "#"),
                new KC( VirtualKey.Number4, "4", "$"),
                new KC( VirtualKey.Number5, "5", "%"),
                new KC( VirtualKey.Number6, "6", "&"),
                new KC( VirtualKey.Number7, "7", "'"),
                new KC( VirtualKey.Number8, "8", "("),
                new KC( VirtualKey.Number9, "9", ")"),
                new KC( VirtualKey.Number0, "0", ""),
                new KC((VirtualKey)189 , "-", "="),
                new KC((VirtualKey)222, "^", "~"),
                new KC((VirtualKey)220, "\\", "|"),

                new KC( VirtualKey.Q, "q", "Q"),
                new KC( VirtualKey.W, "w", "W"),
                new KC( VirtualKey.E, "e", "E"),
                new KC( VirtualKey.R, "r", "R"),
                new KC( VirtualKey.T, "t", "T"),
                new KC( VirtualKey.Y, "y", "Y"),
                new KC( VirtualKey.U, "u", "U"),
                new KC( VirtualKey.I, "i", "I"),
                new KC( VirtualKey.O, "o", "O"),
                new KC( VirtualKey.P, "p", "P"),
                new KC((VirtualKey)192, "@", "`"),
                new KC((VirtualKey)219, "[", "{"),

                new KC( VirtualKey.A, "a", "A"),
                new KC( VirtualKey.S, "s", "S"),
                new KC( VirtualKey.D, "d", "D"),
                new KC( VirtualKey.F, "f", "F"),
                new KC( VirtualKey.G, "g", "G"),
                new KC( VirtualKey.H, "h", "H"),
                new KC( VirtualKey.J, "j", "J"),
                new KC( VirtualKey.K, "k", "K"),
                new KC( VirtualKey.L, "l", "L"),
                new KC( (VirtualKey)187, ";", "+"),
                new KC( (VirtualKey)186, ":", "*"),
                new KC( (VirtualKey)221, "]", "}"),

                new KC( VirtualKey.Z, "z", "Z"),
                new KC( VirtualKey.X, "x", "X"),
                new KC( VirtualKey.C, "c", "C"),
                new KC( VirtualKey.V, "v", "V"),
                new KC( VirtualKey.B, "b", "B"),
                new KC( VirtualKey.N, "n", "N"),
                new KC( VirtualKey.M, "m", "M"),
                new KC( (VirtualKey)188, ",", "<"),
                new KC( (VirtualKey)190, ".", ">"),
                new KC( (VirtualKey)191, "/", "?"),
                new KC( (VirtualKey)226, "\\", "_"),
            
                // 特殊キー
                new KC( VirtualKey.Space, " ", " "),
                new KC( VirtualKey.Enter, "ENTER", "ENTER"),
                new KC( VirtualKey.Back, "BS", "BS"),
                new KC( VirtualKey.Insert, "INS", "INS"),
                new KC( VirtualKey.Delete, "DEL", "DEL"),
                new KC( VirtualKey.Home, "HOME", "HOME"),
                new KC( VirtualKey.End, "END", "END"),
                new KC( VirtualKey.PageUp, "PAGEUP", "PAGEUP"),
                new KC( VirtualKey.PageDown, "PAGEDOWN", "PAGEDOWN"),
                new KC( VirtualKey.Up, "UP", "UP"),
                new KC( VirtualKey.Down, "DOWN", "DOWN"),
                new KC( VirtualKey.Left, "LEFT", "LEFT"),
                new KC( VirtualKey.Right, "RIGHT", "RIGHT"),

                // 数字キーパッド
                new KC( VirtualKey.NumberKeyLock, "NUMLOCK", "NUMLOCK"),
                new KC( VirtualKey.Divide, "/", "/"),
                new KC( VirtualKey.Multiply, "*", "*"),
                new KC( VirtualKey.Subtract, "-", "-"),
                new KC( VirtualKey.Add, "+", "+"),
                new KC( VirtualKey.NumberPad0, "0", "0"),
                new KC( VirtualKey.NumberPad1, "1", "1"),
                new KC( VirtualKey.NumberPad2, "2", "2"),
                new KC( VirtualKey.NumberPad3, "3", "3"),
                new KC( VirtualKey.NumberPad4, "4", "4"),
                new KC( VirtualKey.NumberPad5, "5", "5"),
                new KC( VirtualKey.NumberPad6, "6", "6"),
                new KC( VirtualKey.NumberPad7, "7", "7"),
                new KC( VirtualKey.NumberPad8, "8", "8"),
                new KC( VirtualKey.NumberPad8, "9", "9"),
            };
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            // base.OnKeyDown(e);
            OnKey(e);
        }
        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            // base.OnKeyUp(e);
            OnKey(e);
        }
        private void OnKey(KeyRoutedEventArgs e)
        {
            // if (e.KeyStatus.WasKeyDown == true) return;

            if (e.Key == Windows.System.VirtualKey.Shift)
            {
                isShift = !e.KeyStatus.IsKeyReleased;
                return;
            }
            if (e.Key == Windows.System.VirtualKey.Menu)
            {
                isAlt = !e.KeyStatus.IsKeyReleased;
                return;
            }
            if (e.Key == Windows.System.VirtualKey.Control)
            {
                isCtrl = !e.KeyStatus.IsKeyReleased;
                return;
            }
            if (e.KeyStatus.WasKeyDown == true) return;
            if (e.KeyStatus.ScanCode == 0) return;  // Enter外し

            Debug.WriteLine("key {0} {1} {2} {3} {4}",
                e.Key,
                isShift, isCtrl, isAlt,
                e.KeyStatus.ScanCode
                );

            try
            {
                var key = KCs.First(kc => kc.Key == e.Key);
                if (OnKeyPush != null)
                {
                    string ch = "";
                    if ( key.Normal.Length == 1 ) {
                        ch = isShift == false ? key.Normal : key.Shift;
                    } else {
                        ch = key.Normal;
                    }
                    OnKeyPush(ch, isShift, isCtrl, isAlt);
                }
                /*
                if (key.Normal.Length == 1)
                {
                    ch = isShift == false ? key.Normal : key.Shift;
                    text1.Text += ch;
                }
                else
                {
                    switch (key.Normal)
                    {
                        case "BS": text1.Text = cutEmoji(text1.Text); break;
                        case "DEL": text1.Text = cutEmoji(text1.Text); break;
                    }
                }
                */
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: keycode error {0} {1}", e.Key, ex.Message );
            }
        }

        // public event Action<string, bool, bool, bool> OnKeyPush;
        public delegate void KeyPushHandler(string ch, bool shift, bool ctrl, bool alt);
        public event KeyPushHandler OnKeyPush;
        public class KC
        {
            public Windows.System.VirtualKey Key { get; set; }
            public string Normal { get; set; }
            public string Shift { get; set; }
            public KC(Windows.System.VirtualKey key, string normal, string shift)
            {
                this.Key = key;
                this.Normal = normal;
                this.Shift = shift;
            }
        }
        KC[] KCs;
        bool isShift = false;
        bool isCtrl = false;
        bool isAlt = false;



    }
}
