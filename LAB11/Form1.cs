using System;
using System.Drawing;
using System.Windows.Forms;

namespace LAB11
{
    public partial class Form1 : Form
    {
        // Custom delegate for color changed event
        public delegate void ColorChangedEventHandler(object sender, ColorEventArgs e);
        // Custom delegate for text changed event
        public delegate void TextChangedEventHandler(object sender, EventArgs e);

        // Events
        public event ColorChangedEventHandler ColorChangedEvent;
        public event TextChangedEventHandler TextChangedEvent;

        public Form1()
        {
            InitializeComponent();
            // Subscribe internal handlers
            this.ColorChangedEvent += UpdateLabelColor;
            this.ColorChangedEvent += ShowNotification;

            this.TextChangedEvent += UpdateLabelText;
        }

        private void btnChangeColor_Click(object sender, EventArgs e)
        {
            // Read selected color from combo box and raise event with ColorEventArgs
            string selected = cmbColors.SelectedItem?.ToString() ?? "Red";
            OnColorChanged(new ColorEventArgs(selected));
        }

        private void btnChangeText_Click(object sender, EventArgs e)
        {
            OnTextChanged(EventArgs.Empty);
        }

        // Event invokers
        protected virtual void OnColorChanged(ColorEventArgs e)
        {
            ColorChangedEvent?.Invoke(this, e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            TextChangedEvent?.Invoke(this, e);
        }

        // Event handlers / subscribers
        private void UpdateLabelColor(object sender, ColorEventArgs e)
        {
            Color c = Color.Black;
            switch (e.ColorName)
            {
                case "Red": c = Color.Red; break;
                case "Green": c = Color.Green; break;
                case "Blue": c = Color.Blue; break;
            }
            lblMessage.ForeColor = c;
        }

        private void ShowNotification(object sender, ColorEventArgs e)
        {
            MessageBox.Show(this, $"Selected color: {e.ColorName}", "Color Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateLabelText(object sender, EventArgs e)
        {
            lblMessage.Text = DateTime.Now.ToString("F");
        }
    }
}
