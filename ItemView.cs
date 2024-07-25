using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_Application
{
    public partial class ItemView : Form
    {
        public ItemView()
        {
            InitializeComponent();
            LoadData();
            //SetupDataGridView();

            lblDate.Text = DateTime.Now.ToLongDateString();
            

        }
        private void SetupDataGridView()
        {
            dataGridView1.ColumnCount = 5;

            dataGridView1.Columns[0].Name = "Item_ID";
            dataGridView1.Columns[1].Name = "Barcode";
            dataGridView1.Columns[2].Name = "ItemName";
            dataGridView1.Columns[3].Name = "Qty";
            dataGridView1.Columns[4].Name = "Price";
        }

        private async void LoadData()
        {
            try
            {
                List<Item> items = await GetDataAsync();
                AddRowsToDataGridView(items);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async Task<List<Item>> GetDataAsync()
        {
            string url = "https://gradiantz.com/POS/fetch_Data.php"; // Replace with your server URL
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<Item>>(response);
            }
        }

        private void AddRowsToDataGridView(List<Item> items)
        {
            foreach (var item in items)
            {
                dataGridView1.Rows.Add(item.Item_ID, item.Barcode, item.name, item.Qty, item.Price);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }
    }
    public class Item
    {
        public string Item_ID { get; set; }
        public string Barcode { get; set; }
        public string name { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}