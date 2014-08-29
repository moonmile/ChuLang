using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.System;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace ChuPad
{
    /// <summary>
    /// Frame 内へナビゲートするために利用する空欄ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var lst = new TextBlock[] {
                kLet, kLeft, kRight, kCat, kDog, 
                kIf, kThen, kElse, kEqual, kNEqual,
                kFor, kTo, kFun, kTrue, kFalse,
                kList, kHead, kTail, kPrint, kCase,
            };
            foreach (var it in lst)
                it.Tapped += it_Tapped;
            
            kClear.Tapped += (s,e) => { text1.Text = ""; };
            kMoveLeft.Tapped += (s, e) => {
                text1.Text = cutEmoji(text1.Text);
            };
            kMoveRight.Tapped += (s,e ) => {};
            kEval.Tapped += (s,e) =>{};

            //強制的にフォーカスを設定する
            this.keycode.Focus(FocusState.Keyboard);
            this.keycode.LostFocus += (s, e) => this.keycode.Focus(FocusState.Keyboard);
            this.keycode.IsTabStop = true;
            this.keycode.OnKeyPush += keycode_OnKeyPush;
        }

        void keycode_OnKeyPush(string ch, bool shift, bool ctrl, bool alt)
        {
            if (ch.Length == 1)
            {
                text1.Text += ch;
            }
            else
            {
                switch (ch)
                {
                    case "BS": text1.Text = cutEmoji(text1.Text); break;
                    case "DEL": text1.Text = cutEmoji(text1.Text); break;
                    case "ENTER": text1.Text += "\n"; break;
                }
            }
        }

        /// <summary>
        /// 絵文字の2文字目を判別して削除
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string cutEmoji(string text)
        {
            if (text.Length > 0)
            {
                var c = text[text.Length - 1];
                if ('\uDC00' <= c && c <= '\uDFFF')
                    text = text.Substring(0, text.Length - 2);
                else
                    text = text.Substring(0, text.Length - 1);

            }
            return text;
        }

        /// <summary>
        /// 絵文字をタップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void it_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var tb = sender as TextBlock;
            text1.Text += tb.Text;
        }
    }
}
