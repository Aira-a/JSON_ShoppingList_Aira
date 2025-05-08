using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing;

public class ViewForm : Form
{
    private ListView listView;
    private Button backButton, refreshButton;
    private Label titleLabel;

    public ViewForm()
    {
        this.Text = "View Shopping List";
        this.Width = 380;
        this.Height = 400;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.BackColor = Color.FromArgb(255, 249, 240);

        titleLabel = new Label
        {
            Text = "🧾 Your Shopping List",
            Top = 10,
            Left = 10,
            Width = 300,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            ForeColor = Color.FromArgb(60, 60, 60)
        };

        listView = new ListView
        {
            Top = 40,
            Left = 10,
            Width = 340,
            Height = 240,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            BackColor = Color.WhiteSmoke,
            ForeColor = Color.FromArgb(60, 60, 60),
            Font = new Font("Segoe UI", 10)
        };

        listView.Columns.Add("ID", 60);
        listView.Columns.Add("Name", 150);
        listView.Columns.Add("Price", 100);

        refreshButton = new Button
        {
            Text = "🔄 Refresh",
            Top = 300,
            Left = 10,
            Width = 100,
            BackColor = Color.FromArgb(33, 150, 243),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        backButton = new Button
        {
            Text = "↩️ Back",
            Top = 300,
            Left = 130,
            Width = 100,
            BackColor = Color.FromArgb(255, 152, 0),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        refreshButton.Click += (s, e) => LoadItems();

        backButton.Click += (s, e) =>
        {
            this.Hide();
            new AddForm().Show();
        };

        Controls.Add(titleLabel);
        Controls.Add(listView);
        Controls.Add(refreshButton);
        Controls.Add(backButton);

        LoadItems(); // Load on open
    }

    private void LoadItems()
    {
        listView.Items.Clear();
        string path = "shoppinglist.json";

        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var items = JsonConvert.DeserializeObject<List<GroceryItem>>(json);

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var row = new ListViewItem(new[] {
                        item.ID.ToString(),
                        item.Name,
                        $"${item.Price:F2}"
                    });
                    listView.Items.Add(row);
                }
            }
            else
            {
                listView.Items.Add(new ListViewItem(new[] { "", "No items found", "" }));
            }
        }
        else
        {
            listView.Items.Add(new ListViewItem(new[] { "", "No shopping list file found", "" }));
        }
    }
}
