using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Xml.Serialization;
using DevExpress.Xpf.PivotGrid;

namespace DXPivotGrid_CustomSummary {
    public partial class MainPage : UserControl {
        string dataFileName = "DXPivotGrid_CustomSummary.nwind.xml";
        int minSum = 50;
        public MainPage() {
            InitializeComponent();

            // Parses an XML file and creates a collection of data items.
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(dataFileName);
            XmlSerializer s = new XmlSerializer(typeof(OrderData));
            object dataSource = s.Deserialize(stream);

            // Binds a pivot grid to this collection.
            pivotGridControl1.DataSource = dataSource;
        }
        private void pivotGridControl1_CustomSummary(object sender, PivotCustomSummaryEventArgs e) {
            if (e.DataField != fieldFreight) return;

            // A variable which counts the number of orders whose freight cost exceeds $50.
            int order50Count = 0;

            // Get the record set corresponding to the current cell.
            PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();

            // Iterate through the records and count the orders.
            for (int i = 0; i < ds.RowCount; i++) {
                PivotDrillDownDataRow row = ds[i];

                // Get the order's total sum.
                decimal orderSum = (decimal)row[fieldFreight];
                if (orderSum >= minSum) order50Count++;
            }

            // Calculate the percentage.
            if (ds.RowCount > 0) {
                e.CustomValue = (decimal)order50Count / ds.RowCount;
            }
        }
    }
}