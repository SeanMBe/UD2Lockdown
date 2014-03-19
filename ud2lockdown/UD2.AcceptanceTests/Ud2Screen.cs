using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace UD2.AcceptanceTests
{
    public class Ud2Screen
    {
        private readonly Window _window;

        public Ud2Screen()
        {
            var application = Application.Launch(@"..\..\..\UD2\bin\Debug\ud2.exe");
            this._window = application.GetWindow("UD2", InitializeOption.NoCache);    
        }

        public Button CustomerSearch { get { return this._window.Get<Button>("get_customer_button"); } }
        public TextBox CustomerId { get { return this._window.Get<TextBox>("customer_id_textbox"); } }
        public TextBox CustomerResult { get { return this._window.Get<TextBox>("customer_result_textbox"); } }
    }
}