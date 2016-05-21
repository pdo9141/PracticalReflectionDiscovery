using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderRules.RuleChecker;
using OrderTaker.SharedObjects;

namespace OrderTaker
{
    public partial class _Default : Page
    {
        public static bool _isInitialized = false;
        public static OrderRuleChecker ruleChecker;
        public static Order order;
        public static List<Order> orders;
        public static List<Product> products;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!_isInitialized)
            {
                InitializeRuleChecker();
                InitializeData();
                _isInitialized = true;
            }

            if (!IsPostBack)
            {
                ddlCustomer.DataTextField = "FullName";
                ddlCustomer.DataValueField = "FullName";
                ddlCustomer.DataSource = orders.Select(o => o.Customer).OrderBy(p => p.Rating);
                ddlCustomer.DataBind();

                ddlProduct.DataTextField = "ProductName";
                ddlProduct.DataValueField = "ProductName";
                ddlProduct.DataSource = products;
                ddlProduct.DataBind();

                ltrUnitPrice.Text = products.FirstOrDefault(p => p.ProductName.Equals(ddlProduct.SelectedValue)).Price.ToString("C");

                order = orders.FirstOrDefault(o => o.Customer.FullName.Equals(ddlCustomer.SelectedValue));

                rptItemsAdded.DataSource = order.OrderItems;
                rptItemsAdded.DataBind();
            }
        }

        private void InitializeData()
        {
            orders = new List<Order>();
            products = new List<Product>();

            Order phillipOrder = new Order();
            phillipOrder.OrderItems = new List<OrderItem>();
            phillipOrder.Customer = new Person { FirstName = "Phillip", LastName = "Do", Rating = 0, CustomerSince = "1/1/1978" };
            orders.Add(phillipOrder);

            Order monyOrder = new Order();
            monyOrder.OrderItems = new List<OrderItem>();
            monyOrder.Customer = new Person { FirstName = "Mony", LastName = "Do", Rating = 7, CustomerSince = "2/2/1979" };
            orders.Add(monyOrder);

            Order masonOrder = new Order();
            masonOrder.OrderItems = new List<OrderItem>();
            masonOrder.Customer = new Person { FirstName = "Mason", LastName = "Do", Rating = 9, CustomerSince = "3/3/2009" };
            orders.Add(masonOrder);

            Order emmaOrder = new Order();
            emmaOrder.OrderItems = new List<OrderItem>();
            emmaOrder.Customer = new Person { FirstName = "Emma", LastName = "Do", Rating = 10, CustomerSince = "4/4/2010" };
            orders.Add(emmaOrder);

            products.Add(new Product { ProductName = "Universal Translator", Price = 81 });
            products.Add(new Product { ProductName = "Captain's Chair (Vinyl)", Price = 350 });
            products.Add(new Product { ProductName = "Captain's Chair (Leather)", Price = 550 });
            products.Add(new Product { ProductName = "Laser Beacon", Price = 689 });
            products.Add(new Product { ProductName = "Space Suit", Price = 8515 });
            products.Add(new Product { ProductName = "Warp Engine", Price = 70000 });
            products.Add(new Product { ProductName = "Moonbase", Price = 99999999 });
            products.Add(new Product { ProductName = "Starship", Price = 9000000 });
            products.Add(new Product { ProductName = "Name Badge: Phillip", Price = 18 });
            products.Add(new Product { ProductName = "Name Badge: Mony", Price = 18 });
            products.Add(new Product { ProductName = "Name Badge: Mason", Price = 18 });
            products.Add(new Product { ProductName = "Name Badge: Emma", Price = 18 });
            products.Add(new Product { ProductName = "Name Badge: Kate", Price = 18 });
            products.Add(new Product { ProductName = "Name Badge: Minka", Price = 18 });            
        }

        private void InitializeRuleChecker()
        {
            var rulePath = ConfigurationManager.AppSettings["RelativeRulePath"];
            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;

            if (String.IsNullOrEmpty(rulePath))
                rulePath = applicationPath;
            else
                rulePath = applicationPath + rulePath + Path.DirectorySeparatorChar;

            ruleChecker = new OrderRuleChecker(rulePath);
        }
        
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            int quantity = String.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToInt32(txtQuantity.Text);
            string productName = ddlProduct.SelectedValue;
            Product product = products.FirstOrDefault(p => p.ProductName.Equals(productName));
            order = orders.FirstOrDefault(o => o.Customer.FullName.Equals(ddlCustomer.SelectedValue));

            OrderItem orderItem = order.OrderItems.FirstOrDefault(oi => oi.ProductItem.ProductName.Equals(productName));
            if (orderItem == null)
                order.OrderItems.Add(new OrderItem { ProductItem = product, Quantity = quantity });
            else
                orderItem.Quantity += quantity;

            rptItemsAdded.DataSource = order.OrderItems;
            rptItemsAdded.DataBind();
        }

        protected void btnDeleteItem_Click(object sender, EventArgs e)
        {
            Button senderButton = sender as Button;
            string productName = senderButton.Attributes["ProductName"];

            OrderItem orderItem = order.OrderItems.FirstOrDefault(oi => oi.ProductItem.ProductName.Equals(productName));
            order.OrderItems.Remove(orderItem);

            rptItemsAdded.DataSource = order.OrderItems;
            rptItemsAdded.DataBind();
        }

        protected void btnSubmitOrder_Click(object sender, EventArgs e)
        {
            pldrMessages.Visible = false;
            pldrErrors.Visible = false;
            rptErrors.DataSource = null;
            ltrMessage.Text = String.Empty;

            var result = ruleChecker.CheckRules(order);

            if (!result)
            {
                rptErrors.DataSource = ruleChecker.BrokenRules;
                rptErrors.DataBind();
                pldrErrors.Visible = true;
            }
            else
            {
                pldrMessages.Visible = true;
                ltrMessage.Text = "Order Submitted";
            }
        }
        
        private void ResetOrder() {}

        protected void rptItemsAdded_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderItem orderItem = e.Item.DataItem as OrderItem;
                Literal ltrProductName = (Literal)e.Item.FindControl("ltrProductName");
                Literal ltrProductQuantity = (Literal)e.Item.FindControl("ltrProductQuantity");
                Literal ltrProductTotal = (Literal)e.Item.FindControl("ltrProductTotal");
                Button btnDeleteItem = (Button)e.Item.FindControl("btnDeleteItem");

                ltrProductName.Text = orderItem.ProductItem.ProductName;
                ltrProductQuantity.Text = orderItem.Quantity.ToString();
                ltrProductTotal.Text = (orderItem.Quantity * orderItem.ProductItem.Price).ToString("C");
                btnDeleteItem.Attributes.Add("ProductName", orderItem.ProductItem.ProductName);
            }
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            order = orders.FirstOrDefault(o => o.Customer.FullName.Equals(ddlCustomer.SelectedValue));
            rptItemsAdded.DataSource = order.OrderItems;
            rptItemsAdded.DataBind();
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            ltrUnitPrice.Text = products.FirstOrDefault(p => p.ProductName.Equals(ddlProduct.SelectedValue)).Price.ToString("C");
        }
    }
}