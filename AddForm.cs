using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

public class AddForm : Form
{
    private TextBox nameBox, idBox, priceBox;
    private Button addButton, saveButton, goToViewButton;
    private ListView listView;
    private Label itemCountLabel;
    private List<GroceryItem> items = new List<GroceryItem>();

    public AddForm()
    {
        this.Text = "Add Groceries";
        this.Width = 380;
        this.Height = 440;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;

        Label titleLabel = new Label
        {
            Text = "🛒 Grocery List",
            Top = 10,
            Left = 10,
            Width = 300,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(60, 60, 60)
        };

        Label nameLabel = new Label { Text = "Item Name:", Top = 50, Left = 10 };
        nameBox = new TextBox { Top = 70, Left = 10, Width = 340 };

        Label idLabel = new Label { Text = "Item ID:", Top = 100, Left = 10 };
        idBox = new TextBox { Top = 120, Left = 10, Width = 160 };

        Label priceLabel = new Label { Text = "Price:", Top = 100, Left = 190 };
        priceBox = new TextBox { Top = 120, Left = 190, Width = 160 };

        addButton = new Button { Text = "Add", Top = 150, Left = 10, Width = 340 };

        listView = new ListView
        {
            Top = 190,
            Left = 10,
            Width = 340,
            Height = 140,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true
        };

        listView.Columns.Add("ID", 60);
        listView.Columns.Add("Name", 150);
        listView.Columns.Add("Price", 100);

        itemCountLabel = new Label
        {
            Text = "Items: 0 / 5",
            Top = 335,
            Left = 10,
            AutoSize = true,
            ForeColor = Color.FromArgb(80, 80, 80),
            Font = new Font("Segoe UI", 9, FontStyle.Italic)
        };

        saveButton = new Button
        {
            Text = "💾 Save",
            Top = 360,
            Left = 10,
            Width = 100,
            BackColor = Color.FromArgb(33, 150, 243),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        goToViewButton = new Button
        {
            Text = "📋 View List",
            Top = 360,
            Left = 130,
            Width = 100,
            BackColor = Color.FromArgb(255, 152, 0),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        addButton.Click += (s, e) =>
        {
            string name = nameBox.Text.Trim();
            string idText = idBox.Text.Trim();
            string priceText = priceBox.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(idText) || string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Please fill in all fields: Name, ID, and Price.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(idText, out int id) || !decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Invalid ID or Price format.", "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (items.Count >= 5)
            {
                MessageBox.Show("You can only add up to 5 items.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
                addButton.Enabled = false;
                return;
            }

            var newItem = new GroceryItem { ID = id, Name = name, Price = price };
            items.Add(newItem);
            var row = new ListViewItem(new[] { id.ToString(), name, $"${price:F2}" });
            listView.Items.Add(row);

            nameBox.Clear();
            idBox.Clear();
            priceBox.Clear();
            UpdateItemCount();
        };

        saveButton.Click += (s, e) =>
        {
            if (items.Count == 0)
            {
                MessageBox.Show("List is empty. Please add at least one item before saving.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string json = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText("shoppinglist.json", json);
            MessageBox.Show("✅ Grocery list saved to shoppinglist.json!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        goToViewButton.Click += (s, e) =>
        {
            this.Hide();
            new ViewForm().Show();
        };

        Controls.Add(titleLabel);
        Controls.Add(nameLabel);
        Controls.Add(nameBox);
        Controls.Add(idLabel);
        Controls.Add(idBox);
        Controls.Add(priceLabel);
        Controls.Add(priceBox);
        Controls.Add(addButton);
        Controls.Add(listView);
        Controls.Add(itemCountLabel);
        Controls.Add(saveButton);
        Controls.Add(goToViewButton);
    }

    private void UpdateItemCount()
    {
        itemCountLabel.Text = $"Items: {items.Count} / 5";
        if (items.Count >= 5) addButton.Enabled = false;
    }
}
