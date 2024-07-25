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
using Newtonsoft.Json;

namespace CRUD_Application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void buttonInsert_Click(object sender, EventArgs e)  
        {
            string name = textBoxName.Text;
            string price = textBoxPrice.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(price))
            {
                MessageBox.Show("Please enter both name and price.");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var values = new { name = name, price = price };
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("https://gradiantz.com/POS/insertData.php", content);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Product inserted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert product.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void buttonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync("https://gradiantz.com/POS/getProducts.php");
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<List<Product>>(json);
                        dataGridViewProducts.DataSource = products;
                    }
                    else
                    {
                        MessageBox.Show("Failed to load products.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }


        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Price { get; set; }
        }

    }
}
