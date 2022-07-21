using System.Diagnostics;

namespace StockQuoteViewer
{
    public partial class Form1 : Form
    {
        private int _millisecondsDelay;
        private SynchronizationContext _uiContext;
        CancellationTokenSource cts = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _uiContext = SynchronizationContext.Current;

            BindingStockView();
            InitialRefreshTime();
            RefreshView();
        }

        private void BindingStockView()
        {
            var viewSource = new ViewSource();
            dataGridView1.DataSource = viewSource.Stocks;
        }

        private void InitialRefreshTime()
        {
            _millisecondsDelay = (int)numericUpDown1.Value;
        }

        private void RefreshView()
        {
            Task.Run(async delegate
            {
                try
                {
                    while (cts.Token.IsCancellationRequested == false)
                    {
                        _uiContext.Post(o => { dataGridView1.Refresh(); }, null);
                        await Task.Delay(_millisecondsDelay, cts.Token);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    throw;
                }
            }, cts.Token);
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ReStartView();
        }

        private void numericUpDown1_MouseDown(object sender, MouseEventArgs e)
        {
            ReStartView();
        }

        private void ReStartView()
        {
            try
            {
                cts.Cancel();
                cts = new CancellationTokenSource();

                if (numericUpDown1.Value is > 10 and < 300)
                {
                    _millisecondsDelay = (int)numericUpDown1.Value;
                }

                RefreshView();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            ReStartView();
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var rowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
            var columnCount = dataGridView1.Columns.GetColumnCount(DataGridViewElementStates.Displayed);
            var cellCount = dataGridView1.GetCellCount(DataGridViewElementStates.Displayed);
            var rowCount = cellCount / columnCount;

            for (int i = rowIndex; i < rowIndex + rowCount; i++)
            {
                var dataGridViewCell = dataGridView1.Rows[i].Cells[2];
                if (Convert.ToDecimal(dataGridViewCell.Value.ToString().Replace("%", "")) > 0)
                {
                    dataGridViewCell.Style.ForeColor = Color.Red;
                }

                if (Convert.ToDecimal(dataGridViewCell.Value.ToString().Replace("%", "")) < 0)
                {
                    dataGridViewCell.Style.ForeColor = Color.Green;
                }

                if (Convert.ToDecimal(dataGridViewCell.Value.ToString().Replace("%", "")) == 0)
                {
                    dataGridViewCell.Style.ForeColor = Color.Black;
                }
            }
        }
    }
}