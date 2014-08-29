using System;
using System.Collections.Generic;
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
// using Microsoft.FSharp.Text.Parsing;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Win
{
    /// <summary>
    /// Frame 内へナビゲートするために利用する空欄ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 絵文字をパース
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickEmoji1(object sender, RoutedEventArgs e)
        {
            // string chu = "🚀🐶🐱👈🐶+\"♡\"+🐱";
            string chu = text1.Text;

            var lexbuf = Microsoft.FSharp.Text.Lexing.LexBuffer<char>.FromString(chu);
            var tokens = new List<Parser.token>();
            while (true)
            {
                var tk = Lexer.tokenize(lexbuf);
                if (tk == Parser.token.EOF) break;
                tokens.Add(tk);
            }

            int l = tokens.Count;

            lv.Items.Clear();
            foreach (var it in tokens)
            {
                Token tk = new Token();
                if (it.IsEMOJI)
                {
                    tk.Value = ((Parser.token.EMOJI)it).Item;
                }
                else if (it.IsSTRING)
                {
                    tk.Value = ((Parser.token.STRING)it).Item;
                }
                else if (it.IsNAME)
                {
                    tk.Value = ((Parser.token.NAME)it).Item;
                }
                else if (it.IsINT)
                {
                    tk.Value = ((Parser.token.INT)it).Item.ToString();
                }
                else
                {
                    tk.Value = it.Tag.ToString();
                }
                lv.Items.Add(tk);
            }
        }

        class Token
        {
            public string Value { get; set; }
            public override string ToString()
            {
                return this.Value;
            }
        }
    }
}
