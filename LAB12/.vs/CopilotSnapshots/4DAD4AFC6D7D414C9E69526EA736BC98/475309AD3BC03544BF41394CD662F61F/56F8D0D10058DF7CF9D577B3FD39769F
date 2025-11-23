// Form1.cs (Complete Code for Publisher Setup and UI Initialization)

using System;
using System.Windows.Forms;
using System.Drawing; // Required for positioning and sizing controls

namespace OrderPipeline
{
    public partial class Form1 : Form
    {
        // --- 2A. CONTROL DECLARATIONS ---
        private TextBox txtCustomerName;
        private ComboBox cmbProduct;
        private NumericUpDown numQuantity;
        private Button btnProcessOrder;
        private Button btnShipOrder;
        private Label lblStatus;
        private Label lblName;
        private Label lblProduct;
        private Label lblQuantity;
        private CheckBox chkExpress;

        // --- 2B. DELEGATE AND EVENT DEFINITIONS (PUBLISHER) ---
        public delegate void OrderEventHandler(object sender, OrderEventArgs e);
        public delegate void ShipEventHandler(object sender, ShipEventArgs e);

        public event OrderEventHandler OrderCreated;
        public event OrderEventHandler OrderRejected;
        public event OrderEventHandler OrderConfirmed;

        public event ShipEventHandler OrderShipped;

        // A flag to track if the order was successfully confirmed (used in Step 3, but defined here)
        private bool isOrderConfirmed = false;

        public Form1()
        {
            // Ensure designer initialization runs
            InitializeComponent();

            // 1. Initialize the components (controls)
            InitializeControls();

            // 2. Set up event subscribers (Implementation of Step 3B)
            SubscribeToEvents();
        }

        private void InitializeControls()
        {
            // Setup Form (some properties set by designer too)
            this.Text = "Order Pipeline";
            this.Size = new Size(480, 420);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- Customer Name (Label and TextBox) ---
            lblName = new Label { Text = "Customer Name:", Location = new Point(20, 20), AutoSize = true };
            txtCustomerName = new TextBox { Name = "txtCustomerName", Location = new Point(150, 20), Width = 240, Text = "John Doe" };
            this.Controls.Add(lblName);
            this.Controls.Add(txtCustomerName);

            // --- Product (Label and ComboBox) ---
            lblProduct = new Label { Text = "Product:", Location = new Point(20, 60), AutoSize = true };
            cmbProduct = new ComboBox { Name = "cmbProduct", Location = new Point(150, 60), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbProduct.Items.AddRange(new object[] { "Laptop", "Mouse", "Keyboard" });
            cmbProduct.SelectedIndex = 0;
            this.Controls.Add(lblProduct);
            this.Controls.Add(cmbProduct);

            // --- Quantity (Label and NumericUpDown) ---
            lblQuantity = new Label { Text = "Quantity:", Location = new Point(20, 100), AutoSize = true };
            numQuantity = new NumericUpDown { Name = "numQuantity", Location = new Point(150, 100), Width = 80, Minimum = 0, Value = 1 };
            this.Controls.Add(lblQuantity);
            this.Controls.Add(numQuantity);

            // --- Express Checkbox ---
            chkExpress = new CheckBox { Name = "chkExpress", Text = "Express Delivery", Location = new Point(150, 140), AutoSize = true };
            // Dynamically manage subscriber when checkbox changed
            chkExpress.CheckedChanged += ChkExpress_CheckedChanged;
            this.Controls.Add(chkExpress);

            // --- Process Button ---
            btnProcessOrder = new Button { Name = "btnProcessOrder", Text = "Process Order", Location = new Point(20, 180), Width = 160 };
            btnProcessOrder.Click += new EventHandler(btnProcessOrder_Click); // Hook up the event handler
            this.Controls.Add(btnProcessOrder);

            // --- Ship Button ---
            btnShipOrder = new Button { Name = "btnShipOrder", Text = "Ship Order", Location = new Point(200, 180), Width = 160 };
            btnShipOrder.Click += new EventHandler(btnShipOrder_Click);
            this.Controls.Add(btnShipOrder);

            // --- Status Label ---
            lblStatus = new Label { Name = "lblStatus", Text = "Status: Ready", Location = new Point(20, 230), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            this.Controls.Add(lblStatus);
        }

        // --- 2B. PUBLISHER METHOD: Raises the initial event ---
        private void btnProcessOrder_Click(object sender, EventArgs e)
        {
            // 1. Get input data
            string customerName = txtCustomerName.Text;
            string product = cmbProduct.SelectedItem?.ToString();
            int quantity = (int)numQuantity.Value;

            // 2. Basic Input Check
            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(product))
            {
                lblStatus.Text = "Status: Please complete all fields.";
                return;
            }

            // Reset confirmation flag on new attempt
            isOrderConfirmed = false;

            // 3. Create and Populate Custom EventArgs
            OrderEventArgs args = new OrderEventArgs(customerName, product, quantity);

            // 4. Raise the OrderCreated event
            lblStatus.Text = "Status: Creating Order...";
            OrderCreated?.Invoke(this, args);
        }

        private void btnShipOrder_Click(object sender, EventArgs e)
        {
            if (!isOrderConfirmed)
            {
                lblStatus.Text = "Status: Cannot ship - no confirmed order.";
                MessageBox.Show("Please process and confirm an order before shipping.", "Ship Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prepare shipping args
            string product = cmbProduct.SelectedItem?.ToString();
            bool express = chkExpress.Checked;
            ShipEventArgs sArgs = new ShipEventArgs(product, express);

            // Manage subscribers: ensure NotifyCourier is only subscribed if express is checked
            if (express)
            {
                // Add notify courier if not already subscribed
                if (OrderShipped == null || !IsSubscriberAttached(NotifyCourier))
                {
                    OrderShipped += NotifyCourier;
                }
            }
            else
            {
                // Remove NotifyCourier if present
                OrderShipped -= NotifyCourier;
            }

            // Raise the shipping event
            lblStatus.Text = "Status: Shipping order...";
            OrderShipped?.Invoke(this, sArgs);
        }

        // Helper to detect if a given subscriber is already attached to OrderShipped
        private bool IsSubscriberAttached(ShipEventHandler handler)
        {
            if (OrderShipped == null) return false;
            foreach (Delegate d in OrderShipped.GetInvocationList())
            {
                if (d == (Delegate)handler) return true;
            }
            return false;
        }

        // Checkbox change used to dynamically subscribe/unsubscribe
        private void ChkExpress_CheckedChanged(object sender, EventArgs e)
        {
            if (chkExpress.Checked)
            {
                // subscribe now
                if (!IsSubscriberAttached(NotifyCourier))
                {
                    OrderShipped += NotifyCourier;
                }
            }
            else
            {
                // unsubscribe
                OrderShipped -= NotifyCourier;
            }
        }

        // --- METHODS FOR STEP 3 (SUBSCRIBERS) ---
        private void SubscribeToEvents()
        {
            // OrderCreated subscribers
            OrderCreated += ValidateOrder;
            OrderCreated += DisplayOrderInfo;

            // Chained event subscribers
            OrderRejected += ShowRejection;
            OrderConfirmed += ShowConfirmation;

            // Shipping subscriber: ShowDispatch always listens, NotifyCourier managed dynamically
            OrderShipped += ShowDispatch;
        }

        private void ValidateOrder(object sender, OrderEventArgs e)
        {
            // Checks if quantity > 0
            if (e.Quantity > 0)
            {
                lblStatus.Text = "Status: Order Validated.";
                isOrderConfirmed = true;

                // Chain event
                OrderConfirmed?.Invoke(this, e);
            }
            else
            {
                lblStatus.Text = "Status: Validation Failed (Quantity <= 0).";
                isOrderConfirmed = false;

                // Chain event
                OrderRejected?.Invoke(this, e);
            }
        }

        private void DisplayOrderInfo(object sender, OrderEventArgs e)
        {
            // Shows Message Box with order summary.
            MessageBox.Show(
                $"Order Summary:\nCustomer: {e.CustomerName}\nProduct: {e.Product}\nQuantity: {e.Quantity}",
                "Order Created",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void ShowRejection(object sender, OrderEventArgs e)
        {
            // updates the label
            lblStatus.Text = "Status: Order Invalid - Please retry.";
        }

        private void ShowConfirmation(object sender, OrderEventArgs e)
        {
            // changes label
            lblStatus.Text = $"Status: Order Processed Successfully for {e.CustomerName}.";
        }

        // SHIPPING EVENT SUBSCRIBERS
        private void ShowDispatch(object sender, ShipEventArgs e)
        {
            lblStatus.Text = $"Status: Product dispatched: {e.Product}";
        }

        private void NotifyCourier(object sender, ShipEventArgs e)
        {
            if (e.Express)
            {
                MessageBox.Show("Express delivery initiated!", "Courier Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Handler referenced by the designer; keep it to avoid missing method errors
        private void Form1_Load(object sender, EventArgs e)
        {
            // No-op for now; controls are initialized in InitializeControls
        }
    }
}