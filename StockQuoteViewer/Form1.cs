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

            // 只處理視窗顯示範圍內的畫面
            for (var i = rowIndex; i < rowIndex + rowCount; i++)
            {
                var priceCell = dataGridView1.Rows[i].Cells[1];
                var amplitudeCell = dataGridView1.Rows[i].Cells[2];
                var amplitudePrice = Convert.ToDecimal(amplitudeCell.Value.ToString().Replace("%", ""));

                if (amplitudePrice > 0)
                {
                    priceCell.Style.ForeColor = Color.Red;
                    amplitudeCell.Style.ForeColor = Color.Red;
                }

                if (amplitudePrice < 0)
                {
                    priceCell.Style.ForeColor = Color.Green;
                    amplitudeCell.Style.ForeColor = Color.Green;
                }

                if (amplitudePrice == 0)
                {
                    priceCell.Style.ForeColor = Color.Black;
                    amplitudeCell.Style.ForeColor = Color.Black;
                }

                if (amplitudePrice >= 10m)
                {
                    priceCell.Style.BackColor = Color.Red;
                    priceCell.Style.ForeColor = Color.White;
                }
                else if (amplitudePrice <= -10m)
                {
                    priceCell.Style.BackColor = Color.Green;
                    priceCell.Style.ForeColor = Color.White;
                }
                else
                {
                    priceCell.Style.BackColor = Color.White;
                    priceCell.Style.ForeColor = Color.Black;
                }
            }
        }
    }
}